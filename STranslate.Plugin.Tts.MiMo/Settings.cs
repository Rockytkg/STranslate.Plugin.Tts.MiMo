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
    /// 指定使用的TTS模型，默认为 <c>mimo-v2-tts</c>。
    /// 不同的模型可能支持不同的音色和功能。
    /// </value>
    public string Model { get; set; } = "mimo-v2-tts";

    /// <summary>
    /// 语音音色标识
    /// </summary>
    /// <value>
    /// 指定合成语音的音色。可选值包括：
    /// <list type="bullet">
    ///   <item><c>mimo_default</c> - 默认音色</item>
    ///   <item><c>default_zh</c> - 中文女声</item>
    ///   <item><c>default_en</c> - 英文女声</item>
    /// </list>
    /// </value>
    public string Voice { get; set; } = "mimo_default";

    /// <summary>
    /// 语音风格标签
    /// </summary>
    /// <value>
    /// 控制语音的表达方式，支持多种风格参数：
    /// <list type="bullet">
    ///   <item>语速控制：如 <c> speed:0.8 </c></item>
    ///   <item>情绪变化：如 <c> emotion:happy </c></item>
    ///   <item>角色扮演：如 <c> 林黛玉 </c></item>
    ///   <item>方言口音：如 <c> dialect:cantonese </c></item>
    /// </list>
    /// 多个风格可组合使用，格式为 <c>&lt;style&gt;风格&lt;/style&gt;</c>。
    /// </value>
    public string Style { get; set; } = "林黛玉";
}
