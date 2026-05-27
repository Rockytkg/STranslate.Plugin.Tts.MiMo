# AGENTS.md — STranslate.Plugin.Tts.MiMo

## What this is

A WPF plugin for [STranslate](https://github.com/Rockytkg/STranslate) that provides text-to-speech via Xiaomi MiMo API. Single project, no solution file.

## Build

```bash
# Debug (outputs to ../../../../.artifacts/Debug/Plugins/STranslate.Plugin.Tts.MiMo/)
dotnet build STranslate.Plugin.Tts.MiMo/STranslate.Plugin.Tts.MiMo.csproj

# Release (outputs to STranslate.Plugin.Tts.MiMo/.artifacts/, creates .spkg)
dotnet build STranslate.Plugin.Tts.MiMo/STranslate.Plugin.Tts.MiMo.csproj --configuration Release
```

**Requirements**: .NET 10.0 SDK, Windows (WPF project with `EnableWindowsTargeting=true`).

**No tests exist.** No linter/formatter config. No `.editorconfig`.

## Architecture

```
Main.cs              → ITtsPlugin implementation (entry point)
Settings.cs          → POCO config model, persisted via IPluginContext
ViewModel/
  SettingsViewModel.cs → MVVM ViewModel, auto-saves on property change
View/
  SettingsView.xaml  → WPF settings UI (inkore/ui-wpf controls)
  SettingsView.xaml.cs → Empty code-behind
Languages/
  *.xaml             → WPF ResourceDictionary (UI strings)
  *.json             → Plugin metadata translations
plugin.json          → Plugin manifest (ID, name, dll entry)
```

## Key patterns

- **Plugin interface**: `Main : ITtsPlugin` — implement `Init()`, `GetSettingUI()`, `PlayAudioAsync()`, `Dispose()`
- **Host services via `IPluginContext`**: `LoadSettingStorage<T>()`, `HttpService.PostAsync()`, `AudioPlayer.PlayAsync()`, `Snackbar.Show*()`, `GetTranslation()`
- **MVVM**: Uses `CommunityToolkit.Mvvm` with `[ObservableProperty]` source generators. Settings auto-save via `PropertyChanged` subscription.
- **Localization**: Dual format — `.xaml` ResourceDictionary for WPF `{DynamicResource}`, `.json` for plugin metadata. Key prefix: `STranslate_Plugin_Tts_MiMo_`. All 5 languages: en, ja, ko, zh-cn, zh-tw.
- **API quirk**: MiMo TTS uses Chat Completions format (`/v1/chat/completions`), not a dedicated TTS endpoint. Two API versions:
  - **V2.5** (default): Uses `user` message for natural language style control (director mode), `assistant` message for text to synthesize
  - **V2**: Uses `<style>标签</style>` prefix in assistant content for style injection
  - Model detection: `Settings.Model.StartsWith("mimo-v2.5")` determines message format

## Release

Tag push → GitHub Actions builds → publishes `.spkg` to GitHub Releases. CI uses `dotnet-version: 10.0.x` on `windows-latest`.

## Gotchas

- **Debug output path** is `../../../../.artifacts/Debug/Plugins/STranslate.Plugin.Tts.MiMo/` — deeply nested so STranslate can load the plugin directly during development.
- **Release `EnableAutoPackage=true`** in .csproj triggers `.spkg` packaging automatically.
- **No .sln file** — build the .csproj directly.
- **WPF-only** — cannot build on Linux/macOS without `EnableWindowsTargeting`.
- **`STranslate.Plugin` NuGet** (v1.0.8) is the host SDK — provides all interfaces (`ITtsPlugin`, `IPluginContext`, etc.).

