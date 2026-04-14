using System.Text.Json.Serialization;

namespace PcQuitTimer.Models;

public class ScheduleEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public PowerAction Action { get; set; } = PowerAction.Shutdown;
    public TimeOnly Time { get; set; } = new(23, 0);
    public bool IsEnabled { get; set; } = true;
    public HashSet<DayOfWeek> Days { get; set; } = [DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday];

    [JsonIgnore]
    public string DaysDisplay => Days.Count == 7
        ? "Every day"
        : string.Join(", ", Days.OrderBy(d => d).Select(d => d.ToString()[..3]));
}
