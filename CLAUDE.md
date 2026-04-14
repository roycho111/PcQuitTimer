# PcQuitTimer

A Windows desktop application that schedules PC shutdown, restart, sleep, and other power actions at a specified date/time or after a countdown.

## Tech Stack

- **Runtime**: .NET 10
- **UI Framework**: WPF (Windows Presentation Foundation) with XAML
- **Architecture**: MVVM (Model-View-ViewModel)
- **UI Library**: MaterialDesignThemes (MaterialDesignInXAML)
- **Language**: C#

### Legacy (to be migrated)

The original codebase uses .NET Framework 4.6.1 + WinForms (`Main/` directory). It is kept for reference but no longer the active project.

## Project Structure

```
PcQuitTimer.sln
src/
  PcQuitTimer/
    App.xaml                  # Application entry point
    Models/                   # Data models (TimerConfig, ScheduleEntry, etc.)
    ViewModels/               # MVVM ViewModels
    Views/                    # XAML views
    Services/                 # Business logic (ShutdownService, SchedulerService, etc.)
    Converters/               # XAML value converters
```

## Core Features

### 1. Timer Modes
- **Countdown**: set hours, minutes, seconds and start a countdown
- **Specific date/time**: pick a target date and time using a DateTimePicker

### 2. Power Actions
Support the following actions (user selects one before starting the timer):
- **Shutdown** (`shutdown /s /t 0`)
- **Restart** (`shutdown /r /t 0`)
- **Sleep** (`SetSuspendState(false, true, true)`)
- **Hibernate** (`SetSuspendState(true, true, true)`)
- **Log off** (`ExitWindowsEx(0, 0)`)
- **Lock** (`LockWorkStation()`)

### 3. Recurring Schedules
- Daily or weekly recurring shutdown schedules
- Persist schedules to a local JSON config file
- Schedules should survive app restarts

### 4. Real-time Countdown Display
- Show remaining time (HH:MM:SS) prominently in the UI
- Update every second using a `DispatcherTimer`

## Build & Run

```bash
dotnet build
dotnet run --project src/PcQuitTimer
```

Requires:
- .NET 10 SDK
- Windows 10/11
- Visual Studio 2022+ (recommended for XAML designer)

## Development Conventions

- **Naming**: PascalCase for public members, `_camelCase` for private fields
- **Pattern**: strict MVVM — no code-behind logic in Views except minimal UI wiring
- **Data binding**: use `INotifyPropertyChanged` or CommunityToolkit.Mvvm source generators
- **Commands**: use `ICommand` / `RelayCommand` for button actions
- **DI**: use `Microsoft.Extensions.DependencyInjection` for service registration
- **Shutdown execution**: encapsulate all power actions behind an `IShutdownService` interface for testability
