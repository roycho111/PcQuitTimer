using System.Collections.ObjectModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PcQuitTimer.Models;
using PcQuitTimer.Services;

namespace PcQuitTimer.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IShutdownService _shutdownService;
    private readonly ISchedulerService _schedulerService;
    private readonly DispatcherTimer _countdownTimer;
    private readonly DispatcherTimer _scheduleTimer;
    private DateTime _targetTime;

    public MainViewModel(IShutdownService shutdownService, ISchedulerService schedulerService)
    {
        _shutdownService = shutdownService;
        _schedulerService = schedulerService;

        _countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _countdownTimer.Tick += OnCountdownTick;

        _scheduleTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };
        _scheduleTimer.Tick += OnScheduleCheck;
        _scheduleTimer.Start();

        var entries = _schedulerService.Load();
        foreach (var entry in entries)
            Schedules.Add(entry);
    }

    // Timer tab properties
    [ObservableProperty]
    private TimerMode _selectedTimerMode = TimerMode.Countdown;

    [ObservableProperty]
    private PowerAction _selectedPowerAction = PowerAction.Shutdown;

    [ObservableProperty]
    private int _hours;

    [ObservableProperty]
    private int _minutes;

    [ObservableProperty]
    private int _seconds;

    [ObservableProperty]
    private DateTime _targetDate = DateTime.Today;

    [ObservableProperty]
    private int _targetHour;

    [ObservableProperty]
    private int _targetMinute;

    [ObservableProperty]
    private string _remainingTime = "00:00:00";

    [ObservableProperty]
    private bool _isRunning;

    [ObservableProperty]
    private double _progress;

    private TimeSpan _totalDuration;

    // Schedule tab properties
    [ObservableProperty]
    private PowerAction _scheduleAction = PowerAction.Shutdown;

    [ObservableProperty]
    private int _scheduleHour = 23;

    [ObservableProperty]
    private int _scheduleMinute;

    [ObservableProperty]
    private bool _scheduleMon = true;

    [ObservableProperty]
    private bool _scheduleTue = true;

    [ObservableProperty]
    private bool _scheduleWed = true;

    [ObservableProperty]
    private bool _scheduleThu = true;

    [ObservableProperty]
    private bool _scheduleFri = true;

    [ObservableProperty]
    private bool _scheduleSat;

    [ObservableProperty]
    private bool _scheduleSun;

    public ObservableCollection<ScheduleEntry> Schedules { get; } = [];

    public Array PowerActions => Enum.GetValues<PowerAction>();
    public Array TimerModes => Enum.GetValues<TimerMode>();

    [RelayCommand]
    private void StartTimer()
    {
        if (IsRunning) return;

        if (SelectedTimerMode == TimerMode.Countdown)
        {
            var total = TimeSpan.FromHours(Hours) + TimeSpan.FromMinutes(Minutes) + TimeSpan.FromSeconds(Seconds);
            if (total <= TimeSpan.Zero) return;

            _totalDuration = total;
            _targetTime = DateTime.Now + total;
        }
        else
        {
            var target = TargetDate.Date.AddHours(TargetHour).AddMinutes(TargetMinute);
            if (target <= DateTime.Now) return;

            _totalDuration = target - DateTime.Now;
            _targetTime = target;
        }

        IsRunning = true;
        Progress = 100;
        _countdownTimer.Start();
    }

    [RelayCommand]
    private void StopTimer()
    {
        _countdownTimer.Stop();
        IsRunning = false;
        RemainingTime = "00:00:00";
        Progress = 0;
    }

    [RelayCommand]
    private void AddSchedule()
    {
        var days = new HashSet<DayOfWeek>();
        if (ScheduleMon) days.Add(DayOfWeek.Monday);
        if (ScheduleTue) days.Add(DayOfWeek.Tuesday);
        if (ScheduleWed) days.Add(DayOfWeek.Wednesday);
        if (ScheduleThu) days.Add(DayOfWeek.Thursday);
        if (ScheduleFri) days.Add(DayOfWeek.Friday);
        if (ScheduleSat) days.Add(DayOfWeek.Saturday);
        if (ScheduleSun) days.Add(DayOfWeek.Sunday);

        if (days.Count == 0) return;

        var entry = new ScheduleEntry
        {
            Action = ScheduleAction,
            Time = new TimeOnly(ScheduleHour, ScheduleMinute),
            Days = days,
            IsEnabled = true
        };

        Schedules.Add(entry);
        SaveSchedules();
    }

    [RelayCommand]
    private void RemoveSchedule(ScheduleEntry entry)
    {
        Schedules.Remove(entry);
        SaveSchedules();
    }

    [RelayCommand]
    private void ToggleSchedule(ScheduleEntry entry)
    {
        entry.IsEnabled = !entry.IsEnabled;
        SaveSchedules();
    }

    private void OnCountdownTick(object? sender, EventArgs e)
    {
        var remaining = _targetTime - DateTime.Now;

        if (remaining <= TimeSpan.Zero)
        {
            StopTimer();
            _shutdownService.Execute(SelectedPowerAction);
            return;
        }

        RemainingTime = remaining.ToString(@"hh\:mm\:ss");
        Progress = remaining / _totalDuration * 100;
    }

    private void OnScheduleCheck(object? sender, EventArgs e)
    {
        var now = DateTime.Now;
        var currentTime = TimeOnly.FromDateTime(now);

        foreach (var schedule in Schedules)
        {
            if (!schedule.IsEnabled) continue;
            if (!schedule.Days.Contains(now.DayOfWeek)) continue;

            var diff = Math.Abs((currentTime.ToTimeSpan() - schedule.Time.ToTimeSpan()).TotalSeconds);
            if (diff < 35)
            {
                _shutdownService.Execute(schedule.Action);
                return;
            }
        }
    }

    private void SaveSchedules()
    {
        _schedulerService.Save([.. Schedules]);
    }
}
