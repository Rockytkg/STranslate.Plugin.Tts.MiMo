using STranslate.Plugin.Tts.MiMo.View;
using STranslate.Plugin.Tts.MiMo.ViewModel;
using System.Text.Json;
using System.Windows.Controls;

namespace STranslate.Plugin.Tts.MiMo;

/// <summary>
/// 小米MiMo语音合成插件的主入口类
/// </summary>
/// <remarks>
/// 实现 <see cref="ITtsPlugin"/> 接口，提供文本转语音功能。
/// 通过调用小米MiMo开放平台的语音合成API，将文本转换为语音播放。
/// </remarks>
public class Main : ITtsPlugin
{
    private Control? _settingUi;
    private SettingsViewModel? _viewModel;
    private Settings Settings { get; set; } = null!;
    private IPluginContext Context { get; set; } = null!;

    /// <summary>
    /// 获取插件的设置界面
    /// </summary>
    /// <returns>包含设置UI的控件</returns>
    /// <remarks>
    /// 首次调用时创建 SettingsView 并绑定 ViewModel，之后返回缓存实例
    /// </remarks>
    public Control GetSettingUI()
    {
        _viewModel ??= new SettingsViewModel(Context, Settings);
        _settingUi ??= new SettingsView { DataContext = _viewModel };
        return _settingUi;
    }

    /// <summary>
    /// 初始化插件，加载持久化的配置
    /// </summary>
    /// <param name="context">插件运行时上下文</param>
    public void Init(IPluginContext context)
    {
        Context = context;
        Settings = context.LoadSettingStorage<Settings>();
    }

    /// <summary>
    /// 释放插件占用的资源
    /// </summary>
    public void Dispose() => _viewModel?.Dispose();

    /// <summary>
    /// 播放文本的语音合成
    /// </summary>
    /// <param name="text">要转换的文本内容</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>表示异步操作的任务</returns>
    /// <remarks>
    /// 工作流程：
    /// 1. 验证API Key和文本内容
    /// 2. 根据设置注入风格标签
    /// 3. 调用MiMo Chat Completions API
    /// 4. 解析返回的Base64编码音频数据
    /// 5. 解码并通过AudioPlayer播放
    /// </remarks>
    public async Task PlayAudioAsync(string text, CancellationToken cancellationToken = default)
    {
        // 验证API Key是否已配置
        if (string.IsNullOrWhiteSpace(Settings.ApiKey))
        {
            Context.Snackbar.ShowError(Context.GetTranslation("STranslate_Plugin_Tts_MiMo_ApiKey_Empty"));
            return;
        }

        // 验证文本内容是否为空
        if (string.IsNullOrWhiteSpace(text))
        {
            Context.Snackbar.ShowWarning(Context.GetTranslation("STranslate_Plugin_Tts_MiMo_Text_Empty"));
            return;
        }

        // 注入风格标签：MiMo API要求将风格包装在<style>标签中
        var styleText = !string.IsNullOrWhiteSpace(Settings.Style)
            ? $"<style>{Settings.Style}</style>{text}"
            : text;

        try
        {
            // 构造API请求体（Chat Completions格式）
            var requestBody = new
            {
                model = Settings.Model,
                messages = new[]
                {
                    new { role = "user", content = "" },
                    new { role = "assistant", content = styleText }
                },
                audio = new
                {
                    format = "mp3",
                    voice = Settings.Voice
                }
            };

            // 配置HTTP请求头
            var option = new Options
            {
                Headers = new Dictionary<string, string>
                {
                    ["api-key"] = Settings.ApiKey,
                    ["Content-Type"] = "application/json"
                }
            };

            // 发送请求到MiMo API
            var response = await Context.HttpService.PostAsync(
                Settings.Url,
                requestBody,
                option,
                cancellationToken);

            // 解析JSON响应
            using var doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            // 处理API返回的错误
            if (root.TryGetProperty("error", out var errorElement))
            {
                var errorMessage = errorElement.GetProperty("message").GetString() ?? "Unknown error";
                Context.Snackbar.ShowError(errorMessage);
                return;
            }

            // 提取音频数据（Base64编码）
            var audioData = root
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("audio")
                .GetProperty("data")
                .GetString();

            // 验证音频数据是否为空
            if (string.IsNullOrEmpty(audioData))
            {
                Context.Snackbar.ShowWarning(Context.GetTranslation("STranslate_Plugin_Tts_MiMo_Audio_Empty"));
                return;
            }

            // 解码Base64并播放音频
            var audioBytes = Convert.FromBase64String(audioData);
            await Context.AudioPlayer.PlayAsync(audioBytes, cancellationToken);
        }
        catch (JsonException)
        {
            // JSON解析失败（API响应格式错误）
            Context.Snackbar.ShowError(Context.GetTranslation("STranslate_Plugin_Tts_MiMo_Parse_Error"));
        }
        catch (FormatException)
        {
            // Base64解码失败（音频数据损坏）
            Context.Snackbar.ShowError(Context.GetTranslation("STranslate_Plugin_Tts_MiMo_Decode_Error"));
        }
        catch (Exception ex)
        {
            // 其他异常
            Context.Snackbar.ShowError(ex.Message);
        }
    }
}
