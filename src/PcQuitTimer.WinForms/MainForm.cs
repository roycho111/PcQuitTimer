using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PcQuitTimer;

public class MainForm : Form
{
    // Timer tab controls
    private TabControl _tabControl;
    private RadioButton _rbCountdown, _rbSpecificTime;
    private NumericUpDown _nudHours, _nudMinutes, _nudSeconds;
    private DateTimePicker _dtpDate, _dtpTime;
    private Panel _panelCountdown, _panelSpecific;
    private ComboBox _cboAction;
    private Label _lblRemaining, _lblHours, _lblMinutes, _lblSeconds, _lblAction;
    private ProgressBar _progressBar;
    private Button _btnStart, _btnStop, _btnLang;

    // Schedule tab controls
    private NumericUpDown _nudSchHour, _nudSchMinute;
    private ComboBox _cboSchAction;
    private CheckBox _chkMon, _chkTue, _chkWed, _chkThu, _chkFri, _chkSat, _chkSun;
    private ListBox _lstSchedules;
    private Button _btnAddSchedule, _btnRemoveSchedule, _btnToggleSchedule;
    private Label _lblSchTime;

    // State
    private readonly Timer _countdownTimer = new();
    private readonly Timer _scheduleTimer = new();
    private DateTime _targetTime;
    private TimeSpan _totalDuration;
    private List<ScheduleEntry> _schedules;

    public MainForm()
    {
        InitializeForm();
        InitializeTimerTab();
        InitializeScheduleTab();
        SetupTimers();
        LoadSchedules();

        Strings.LanguageChanged += ApplyLanguage;
    }

    private void InitializeForm()
    {
        Text = "PC Quit Timer";
        Size = new Size(460, 480);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9F);

        _tabControl = new TabControl
        {
            Dock = DockStyle.Fill
        };
        Controls.Add(_tabControl);
    }

    private void InitializeTimerTab()
    {
        var timerTab = new TabPage(Strings.TabTimer);
        _tabControl.TabPages.Add(timerTab);

        var y = 15;

        // Language toggle button (top-right)
        _btnLang = new Button
        {
            Text = Strings.LangToggle,
            Location = new Point(370, y),
            Size = new Size(40, 25),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 8F, FontStyle.Bold)
        };
        _btnLang.FlatAppearance.BorderColor = Color.Gray;
        _btnLang.Click += (_, _) => Strings.SetKorean(!Strings.IsKorean);
        timerTab.Controls.Add(_btnLang);

        // Timer mode
        _rbCountdown = new RadioButton { Text = Strings.Countdown, Location = new Point(20, y), AutoSize = true, Checked = true };
        _rbSpecificTime = new RadioButton { Text = Strings.SpecificTime, Location = new Point(140, y), AutoSize = true };
        _rbCountdown.CheckedChanged += (_, _) => ToggleTimerMode();
        timerTab.Controls.AddRange(new Control[] { _rbCountdown, _rbSpecificTime });

        y += 35;

        // Countdown panel
        _panelCountdown = new Panel { Location = new Point(20, y), Size = new Size(400, 45) };
        _nudHours = CreateNumericUpDown(0, 0, 99, 70);
        _nudMinutes = CreateNumericUpDown(80, 0, 59, 70);
        _nudSeconds = CreateNumericUpDown(160, 0, 59, 70);
        _lblHours = new Label { Text = Strings.Hours, Location = new Point(42, 25), AutoSize = true };
        _lblMinutes = new Label { Text = Strings.Minutes, Location = new Point(107, 25), AutoSize = true };
        _lblSeconds = new Label { Text = Strings.Seconds, Location = new Point(195, 25), AutoSize = true };
        _panelCountdown.Controls.AddRange(new Control[]
        {
            _nudHours, _lblHours,
            _nudMinutes, _lblMinutes,
            _nudSeconds, _lblSeconds
        });
        timerTab.Controls.Add(_panelCountdown);

        // Specific time panel
        _panelSpecific = new Panel { Location = new Point(20, y), Size = new Size(400, 45), Visible = false };
        _dtpDate = new DateTimePicker { Location = new Point(0, 0), Width = 140, Format = DateTimePickerFormat.Short };
        _dtpTime = new DateTimePicker { Location = new Point(150, 0), Width = 100, Format = DateTimePickerFormat.Time, ShowUpDown = true };
        _panelSpecific.Controls.AddRange(new Control[] { _dtpDate, _dtpTime });
        timerTab.Controls.Add(_panelSpecific);

        y += 55;

        // Action
        _lblAction = new Label { Text = Strings.Action, Location = new Point(20, y + 3), AutoSize = true };
        timerTab.Controls.Add(_lblAction);
        _cboAction = new ComboBox
        {
            Location = new Point(80, y),
            Width = 120,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        foreach (PowerAction action in Enum.GetValues(typeof(PowerAction)))
            _cboAction.Items.Add(new ActionItem(action));
        _cboAction.SelectedIndex = 0;
        timerTab.Controls.Add(_cboAction);

        y += 50;

        // Remaining time
        _lblRemaining = new Label
        {
            Text = "00:00:00",
            Font = new Font("Segoe UI", 36F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point(20, y),
            Size = new Size(400, 70)
        };
        timerTab.Controls.Add(_lblRemaining);

        y += 75;

        // Progress bar
        _progressBar = new ProgressBar
        {
            Location = new Point(20, y),
            Size = new Size(400, 10),
            Maximum = 1000
        };
        timerTab.Controls.Add(_progressBar);

        y += 30;

        // Buttons
        _btnStart = new Button
        {
            Text = Strings.Start,
            Location = new Point(130, y),
            Size = new Size(90, 35),
            BackColor = Color.FromArgb(33, 150, 243),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        _btnStart.FlatAppearance.BorderSize = 0;
        _btnStart.Click += OnStart;

        _btnStop = new Button
        {
            Text = Strings.Stop,
            Location = new Point(230, y),
            Size = new Size(90, 35),
            BackColor = Color.FromArgb(244, 67, 54),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Enabled = false
        };
        _btnStop.FlatAppearance.BorderSize = 0;
        _btnStop.Click += OnStop;

        timerTab.Controls.AddRange(new Control[] { _btnStart, _btnStop });
    }

    private void InitializeScheduleTab()
    {
        var scheduleTab = new TabPage(Strings.TabSchedule);
        _tabControl.TabPages.Add(scheduleTab);

        var y = 15;

        // Time input
        _lblSchTime = new Label { Text = Strings.Time, Location = new Point(20, y + 3), AutoSize = true };
        scheduleTab.Controls.Add(_lblSchTime);
        _nudSchHour = CreateNumericUpDown(70, 23, 23, 55, y);
        scheduleTab.Controls.Add(new Label { Text = ":", Location = new Point(130, y + 3), AutoSize = true });
        _nudSchMinute = CreateNumericUpDown(143, 0, 59, 55, y);
        scheduleTab.Controls.Add(_nudSchHour);
        scheduleTab.Controls.Add(_nudSchMinute);

        // Action
        _cboSchAction = new ComboBox
        {
            Location = new Point(210, y),
            Width = 100,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        foreach (PowerAction action in Enum.GetValues(typeof(PowerAction)))
            _cboSchAction.Items.Add(new ActionItem(action));
        _cboSchAction.SelectedIndex = 0;
        scheduleTab.Controls.Add(_cboSchAction);

        y += 35;

        // Day checkboxes
        var dayX = 20;
        _chkMon = AddDayCheckBox(scheduleTab, Strings.Mon, dayX, y, true); dayX += 55;
        _chkTue = AddDayCheckBox(scheduleTab, Strings.Tue, dayX, y, true); dayX += 55;
        _chkWed = AddDayCheckBox(scheduleTab, Strings.Wed, dayX, y, true); dayX += 55;
        _chkThu = AddDayCheckBox(scheduleTab, Strings.Thu, dayX, y, true); dayX += 55;
        _chkFri = AddDayCheckBox(scheduleTab, Strings.Fri, dayX, y, true); dayX += 55;
        _chkSat = AddDayCheckBox(scheduleTab, Strings.Sat, dayX, y, false); dayX += 55;
        _chkSun = AddDayCheckBox(scheduleTab, Strings.Sun, dayX, y, false);

        y += 35;

        // Add/Remove buttons
        _btnAddSchedule = new Button { Text = Strings.Add, Location = new Point(20, y), Size = new Size(70, 30) };
        _btnAddSchedule.Click += OnAddSchedule;
        _btnToggleSchedule = new Button { Text = Strings.Toggle, Location = new Point(95, y), Size = new Size(80, 30) };
        _btnToggleSchedule.Click += OnToggleSchedule;
        _btnRemoveSchedule = new Button { Text = Strings.Remove, Location = new Point(180, y), Size = new Size(80, 30) };
        _btnRemoveSchedule.Click += OnRemoveSchedule;
        scheduleTab.Controls.AddRange(new Control[] { _btnAddSchedule, _btnToggleSchedule, _btnRemoveSchedule });

        y += 40;

        // Schedule list
        _lstSchedules = new ListBox
        {
            Location = new Point(20, y),
            Size = new Size(395, 260),
            Font = new Font("Segoe UI", 10F)
        };
        scheduleTab.Controls.Add(_lstSchedules);
    }

    private void ApplyLanguage()
    {
        // Tabs
        _tabControl.TabPages[0].Text = Strings.TabTimer;
        _tabControl.TabPages[1].Text = Strings.TabSchedule;

        // Timer tab
        _btnLang.Text = Strings.LangToggle;
        _rbCountdown.Text = Strings.Countdown;
        _rbSpecificTime.Text = Strings.SpecificTime;
        _lblHours.Text = Strings.Hours;
        _lblMinutes.Text = Strings.Minutes;
        _lblSeconds.Text = Strings.Seconds;
        _lblAction.Text = Strings.Action;
        _btnStart.Text = Strings.Start;
        _btnStop.Text = Strings.Stop;

        // Schedule tab
        _lblSchTime.Text = Strings.Time;
        _btnAddSchedule.Text = Strings.Add;
        _btnToggleSchedule.Text = Strings.Toggle;
        _btnRemoveSchedule.Text = Strings.Remove;
        _chkMon.Text = Strings.Mon;
        _chkTue.Text = Strings.Tue;
        _chkWed.Text = Strings.Wed;
        _chkThu.Text = Strings.Thu;
        _chkFri.Text = Strings.Fri;
        _chkSat.Text = Strings.Sat;
        _chkSun.Text = Strings.Sun;

        // Refresh combo boxes
        RefreshComboBox(_cboAction);
        RefreshComboBox(_cboSchAction);

        // Refresh schedule list (day names change)
        RefreshScheduleList();
    }

    private static void RefreshComboBox(ComboBox combo)
    {
        var selected = combo.SelectedIndex;
        combo.Items.Clear();
        foreach (PowerAction action in Enum.GetValues(typeof(PowerAction)))
            combo.Items.Add(new ActionItem(action));
        combo.SelectedIndex = selected;
    }

    private void SetupTimers()
    {
        _countdownTimer.Interval = 1000;
        _countdownTimer.Tick += OnCountdownTick;

        _scheduleTimer.Interval = 30000;
        _scheduleTimer.Tick += OnScheduleCheck;
        _scheduleTimer.Start();
    }

    private void LoadSchedules()
    {
        _schedules = ScheduleManager.Load();
        RefreshScheduleList();
    }

    private void RefreshScheduleList()
    {
        _lstSchedules.Items.Clear();
        foreach (var s in _schedules)
            _lstSchedules.Items.Add(s);
    }

    private void ToggleTimerMode()
    {
        _panelCountdown.Visible = _rbCountdown.Checked;
        _panelSpecific.Visible = _rbSpecificTime.Checked;
    }

    private void OnStart(object sender, EventArgs e)
    {
        if (_rbCountdown.Checked)
        {
            var total = TimeSpan.FromHours((double)_nudHours.Value)
                      + TimeSpan.FromMinutes((double)_nudMinutes.Value)
                      + TimeSpan.FromSeconds((double)_nudSeconds.Value);
            if (total <= TimeSpan.Zero) { MessageBox.Show(Strings.MsgEnterTime, Strings.MsgNotice); return; }
            _totalDuration = total;
            _targetTime = DateTime.Now + total;
        }
        else
        {
            var target = _dtpDate.Value.Date + _dtpTime.Value.TimeOfDay;
            if (target <= DateTime.Now) { MessageBox.Show(Strings.MsgFutureTime, Strings.MsgNotice); return; }
            _totalDuration = target - DateTime.Now;
            _targetTime = target;
        }

        _btnStart.Enabled = false;
        _btnStop.Enabled = true;
        _progressBar.Value = _progressBar.Maximum;
        _countdownTimer.Start();
    }

    private void OnStop(object sender, EventArgs e)
    {
        _countdownTimer.Stop();
        _btnStart.Enabled = true;
        _btnStop.Enabled = false;
        _lblRemaining.Text = "00:00:00";
        _progressBar.Value = 0;
    }

    private void OnCountdownTick(object sender, EventArgs e)
    {
        var remaining = _targetTime - DateTime.Now;
        if (remaining <= TimeSpan.Zero)
        {
            OnStop(this, EventArgs.Empty);
            var action = ((ActionItem)_cboAction.SelectedItem).Action;
            ShutdownHelper.Execute(action);
            return;
        }

        _lblRemaining.Text = remaining.ToString(@"hh\:mm\:ss");
        var ratio = remaining.TotalSeconds / _totalDuration.TotalSeconds;
        _progressBar.Value = Math.Max(0, Math.Min(_progressBar.Maximum, (int)(ratio * _progressBar.Maximum)));
    }

    private void OnScheduleCheck(object sender, EventArgs e)
    {
        var now = DateTime.Now;
        foreach (var schedule in _schedules)
        {
            if (!schedule.IsEnabled) continue;
            if (!schedule.Days.Contains(now.DayOfWeek)) continue;
            var diff = Math.Abs((now.TimeOfDay - new TimeSpan(schedule.Hour, schedule.Minute, 0)).TotalSeconds);
            if (diff < 35)
            {
                ShutdownHelper.Execute(schedule.Action);
                return;
            }
        }
    }

    private void OnAddSchedule(object sender, EventArgs e)
    {
        var days = new List<DayOfWeek>();
        if (_chkMon.Checked) days.Add(DayOfWeek.Monday);
        if (_chkTue.Checked) days.Add(DayOfWeek.Tuesday);
        if (_chkWed.Checked) days.Add(DayOfWeek.Wednesday);
        if (_chkThu.Checked) days.Add(DayOfWeek.Thursday);
        if (_chkFri.Checked) days.Add(DayOfWeek.Friday);
        if (_chkSat.Checked) days.Add(DayOfWeek.Saturday);
        if (_chkSun.Checked) days.Add(DayOfWeek.Sunday);

        if (days.Count == 0) { MessageBox.Show(Strings.MsgSelectDay, Strings.MsgNotice); return; }

        _schedules.Add(new ScheduleEntry
        {
            Action = ((ActionItem)_cboSchAction.SelectedItem).Action,
            Hour = (int)_nudSchHour.Value,
            Minute = (int)_nudSchMinute.Value,
            Days = days
        });
        ScheduleManager.Save(_schedules);
        RefreshScheduleList();
    }

    private void OnRemoveSchedule(object sender, EventArgs e)
    {
        if (_lstSchedules.SelectedIndex < 0) return;
        _schedules.RemoveAt(_lstSchedules.SelectedIndex);
        ScheduleManager.Save(_schedules);
        RefreshScheduleList();
    }

    private void OnToggleSchedule(object sender, EventArgs e)
    {
        if (_lstSchedules.SelectedIndex < 0) return;
        var entry = _schedules[_lstSchedules.SelectedIndex];
        entry.IsEnabled = !entry.IsEnabled;
        ScheduleManager.Save(_schedules);
        RefreshScheduleList();
    }

    // Helpers
    private static NumericUpDown CreateNumericUpDown(int x, decimal value, decimal max, int width, int y = 0)
    {
        return new NumericUpDown
        {
            Location = new Point(x, y),
            Width = width,
            Minimum = 0,
            Maximum = max,
            Value = value,
            TextAlign = HorizontalAlignment.Center,
            Font = new Font("Segoe UI", 12F)
        };
    }

    private static CheckBox AddDayCheckBox(TabPage parent, string text, int x, int y, bool isChecked)
    {
        var chk = new CheckBox { Text = text, Location = new Point(x, y), AutoSize = true, Checked = isChecked };
        parent.Controls.Add(chk);
        return chk;
    }

    private class ActionItem
    {
        public PowerAction Action { get; }
        public ActionItem(PowerAction action) => Action = action;
        public override string ToString() => ShutdownHelper.GetDisplayName(Action);
    }
}
