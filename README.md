# STranslate 小米 MiMo 语音合成插件

基于 [小米 MiMo](https://platform.xiaomimimo.com) 语音合成 API 的 STranslate TTS 插件，支持多样化发音风格。

## 📦 安装

1. 下载最新的 `.spkg` 文件（在 [Releases](https://github.com/Rockytkg/STranslate.Plugin.Tts.MiMo/releases) 页面）
2. 在 STranslate 中进入 **设置** → **插件** → **安装插件**
3. 选择下载的 `.spkg` 文件并重启 STranslate

## 前置条件

需要注册 [小米 MiMo 开放平台](https://platform.xiaomimimo.com) 账号并获取 API Key。

> 📖 详细使用说明请参考 [官方文档](https://platform.xiaomimimo.com/#/docs/usage-guide/speech-synthesis)

## ⚙️ 配置

| 参数 | 默认值 | 说明 |
|------|--------|------|
| 接口地址 | `https://api.xiaomimimo.com/v1/chat/completions` | MiMo API 地址 |
| API Key | - | 小米 MiMo 平台的 API Key |
| 模型 | `mimo-v2-tts` | 语音合成模型 |
| 声音 | `mimo_default` | 发音音色 |
| 音频格式 | `wav` | 输出音频格式 |

## 使用方式

安装插件后，在 STranslate 中选中文本，使用 TTS 功能即可调用小米 MiMo 进行语音合成。

## 📄 许可证

[MIT](LICENSE)
