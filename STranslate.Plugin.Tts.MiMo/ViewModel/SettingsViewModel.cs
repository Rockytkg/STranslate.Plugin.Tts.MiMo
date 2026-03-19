using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace STranslate.Plugin.Tts.MiMo.ViewModel;

/// <summary>
/// 音色选项记录，用于语音合成时的音色选择
/// </summary>
/// <param name="Name">显示名称</param>
/// <param name="Value">API调用时使用的音色标识</param>
public record VoiceItem(string Name, string Value);

/// <summary>
/// 设置页面的视图模型，负责UI与配置数据的双向绑定
/// </summary>
public partial class SettingsViewModel : ObservableObject, IDisposable
{
    private readonly IPluginContext _context;
    private readonly Settings _settings;

    /// <summary>
    /// MiMo API接口地址
    /// </summary>
    [ObservableProperty]
    public partial string Url { get; set; }

    /// <summary>
    /// 小米MiMo平台的API密钥，用于身份验证
    /// </summary>
    [ObservableProperty]
    public partial string ApiKey { get; set; }

    /// <summary>
    /// 语音合成模型标识符
    /// </summary>
    [ObservableProperty]
    public partial string Model { get; set; }

    /// <summary>
    /// 当前选中的音色选项
    /// </summary>
    [ObservableProperty]
    public partial VoiceItem? SelectedVoice { get; set; }

    /// <summary>
    /// 语音风格标签，支持语速控制、情绪变化、角色扮演、方言等
    /// </summary>
    [ObservableProperty]
    public partial string Style { get; set; }

    /// <summary>
    /// 可选的音色列表
    /// </summary>
    public ObservableCollection<VoiceItem> Voices { get; } =
    [
        new("MiMo-默认", "mimo_default"),
        new("MiMo-中文女声", "default_zh"),
        new("MiMo-英文女声", "default_en")
    ];

    /// <summary>
    /// 初始化视图模型，加载保存的设置并绑定属性变更事件
    /// </summary>
    /// <param name="context">插件上下文，用于访问配置存储和本地化</param>
    /// <param name="settings">插件配置实例</param>
    public SettingsViewModel(IPluginContext context, Settings settings)
    {
        _context = context;
        _settings = settings;

        // 从持久化配置加载初始值
        Url = settings.Url;
        ApiKey = settings.ApiKey;
        Model = settings.Model;
        SelectedVoice = Voices.FirstOrDefault(v => v.Value == settings.Voice) ?? Voices[0];
        Style = settings.Style;

        // 订阅属性变更，实现自动保存
        PropertyChanged += OnPropertyChanged;
    }

    /// <summary>
    /// 处理属性变更事件，将变更同步回配置并持久化
    /// </summary>
    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Url):
                _settings.Url = Url;
                break;
            case nameof(ApiKey):
                _settings.ApiKey = ApiKey;
                break;
            case nameof(Model):
                _settings.Model = Model;
                break;
            case nameof(SelectedVoice):
                _settings.Voice = SelectedVoice?.Value ?? "mimo_default";
                break;
            case nameof(Style):
                _settings.Style = Style;
                break;
        }

        // 属性变更时自动保存配置
        _context.SaveSettingStorage<Settings>();
    }

    /// <summary>
    /// 释放资源，取消事件订阅
    /// </summary>
    public void Dispose()
    {
        PropertyChanged -= OnPropertyChanged;
    }
}
