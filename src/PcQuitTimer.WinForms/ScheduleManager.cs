using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace PcQuitTimer;

public class ScheduleEntry
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public PowerAction Action { get; set; } = PowerAction.Shutdown;
    public int Hour { get; set; } = 23;
    public int Minute { get; set; } = 0;
    public bool IsEnabled { get; set; } = true;
    public List<DayOfWeek> Days { get; set; } = new();

    [XmlIgnore]
    public string DaysText
    {
        get
        {
            if (Days.Count == 7) return "매일";
            var names = new List<string>();
            foreach (var d in Days)
            {
                names.Add(d switch
                {
                    DayOfWeek.Monday => "월",
                    DayOfWeek.Tuesday => "화",
                    DayOfWeek.Wednesday => "수",
                    DayOfWeek.Thursday => "목",
                    DayOfWeek.Friday => "금",
                    DayOfWeek.Saturday => "토",
                    DayOfWeek.Sunday => "일",
                    _ => ""
                });
            }
            return string.Join(", ", names);
        }
    }

    public override string ToString()
    {
        var enabled = IsEnabled ? "●" : "○";
        return $"{enabled}  {Hour:D2}:{Minute:D2}  [{ShutdownHelper.GetDisplayName(Action)}]  {DaysText}";
    }
}

public static class ScheduleManager
{
    private static readonly string ConfigPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "PcQuitTimer",
        "schedules.xml");

    private static readonly XmlSerializer Serializer = new(typeof(List<ScheduleEntry>));

    public static List<ScheduleEntry> Load()
    {
        if (!File.Exists(ConfigPath))
            return new List<ScheduleEntry>();

        using var stream = File.OpenRead(ConfigPath);
        return Serializer.Deserialize(stream) as List<ScheduleEntry> ?? new List<ScheduleEntry>();
    }

    public static void Save(List<ScheduleEntry> entries)
    {
        var dir = Path.GetDirectoryName(ConfigPath)!;
        Directory.CreateDirectory(dir);

        using var stream = File.Create(ConfigPath);
        Serializer.Serialize(stream, entries);
    }
}
