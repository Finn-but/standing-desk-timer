# Standing Desk Timer

A Windows desktop app built with WinUI 3 that reminds you to alternate between sitting and standing at your desk.

## Requirements

- Windows 10 version 1903 (build 17763) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with the following workloads:
  - **.NET desktop development**
  - **Windows application development** (includes Windows App SDK / WinUI 3 tools)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Windows App SDK](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/downloads) (restored automatically via NuGet)

## Build

### Via Visual Studio

1. Open `StandingDeskTimer.sln` in Visual Studio 2022.
2. Select your target platform (`x86`, `x64`, or `ARM64`) from the platform dropdown.
3. Press **F5** to build and run, or **Ctrl+Shift+B** to build only.

### Via Command Line

```powershell
# Restore NuGet packages
dotnet restore StandingDeskTimer.sln

# Build (replace x64 with x86 or ARM64 as needed)
dotnet build StandingDeskTimer.sln -p:Platform=x64 -c Debug
```

## Publish (self-contained)

Publish profiles for each platform are located in `StandingDeskTimer/Properties/PublishProfiles/`.

```powershell
# Publish for x64
dotnet publish StandingDeskTimer/StandingDeskTimer.csproj -p:PublishProfile=win10-x64.pubxml

# Publish for x86
dotnet publish StandingDeskTimer/StandingDeskTimer.csproj -p:PublishProfile=win10-x86.pubxml

# Publish for ARM64
dotnet publish StandingDeskTimer/StandingDeskTimer.csproj -p:PublishProfile=win10-arm64.pubxml
```

Output is written to `StandingDeskTimer/bin/<Configuration>/net8.0-windows10.0.19041.0/<platform>/publish/`.

## Project Structure

```
StandingDeskTimer/
├── App.xaml / App.xaml.cs       # Application entry point
├── MainWindow.xaml / .cs        # Main UI and timer logic
├── Package.appxmanifest         # MSIX package manifest
├── Assets/                      # App icons and images
└── Properties/PublishProfiles/  # Publish configuration per platform
```

## Tech Stack

- **.NET 8** — runtime
- **WinUI 3** (Windows App SDK 1.6) — UI framework
- **MSIX packaging** — single-project packaging via `EnableMsixTooling`
