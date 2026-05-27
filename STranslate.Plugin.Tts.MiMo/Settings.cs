namespace STranslate.Plugin.Tts.MiMo;

/// <summary>
/// 插件配置模型，定义语音合成服务的各项参数
/// </summary>
/// <remarks>
/// 配置通过 <see cref="IPluginContext.LoadSettingStorage{T}"/> 持久化存储。
/// 属性变更时由 <see cref="ViewModel.SettingsViewModel"/> 自动保存。
/// </remarks>
public class Settings
{
    /// <summary>
    /// MiMo API接口地址
    /// </summary>
    /// <value>
    /// 默认为小米MiMo开放平台的Chat Completions端点。
    /// 可根据需要替换为代理地址或其他兼容API。
    /// </value>
    public string Url { get; set; } = "https://api.xiaomimimo.com/v1/chat/completions";

    /// <summary>
    /// 小米MiMo平台的API密钥
    /// </summary>
    /// <value>
    /// 用于API身份验证。请前往
    /// https://platform.xiaomimimo.com 获取API Key。
    /// </value>
    public string ApiKey { get; set; } = "";

    /// <summary>
    /// 语音合成模型标识符
    /// </summary>
    /// <value>
    /// 指定使用的TTS模型，默认为 <c>mimo-v2.5-tts</c>。
    /// 支持的模型：
    /// <list type="bullet">
    ///   <item><c>mimo-v2.5-tts</c> - V2.5预置音色（推荐）</item>
    ///   <item><c>mimo-v2-tts</c> - V2旧版模型</item>
    /// </list>
    /// </value>
    public string Model { get; set; } = "mimo-v2.5-tts";

    /// <summary>
    /// 语音音色标识
    /// </summary>
    /// <value>
    /// 指定合成语音的音色。可选值包括：
    /// <list type="bullet">
    ///   <item><c>Mia</c> - V2.5英文女声（默认）</item>
    ///   <item><c>mimo_default</c> - 默认音色</item>
    ///   <item><c>default_zh</c> - 中文女声</item>
    ///   <item><c>default_en</c> - 英文女声</item>
    /// </list>
    /// </value>
    public string Voice { get; set; } = "Mia";

    /// <summary>
    /// 语音风格控制
    /// </summary>
    /// <value>
    /// V2.5模型：自然语言风格控制（导演模式），如"Speak slightly fast, cheerful tone"
    /// V2模型：风格标签，如"林黛玉"、"emotion:happy"，自动包装为&lt;style&gt;标签
    /// </value>
    public string Style { get; set; } = "";
}
