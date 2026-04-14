using System.IO;
using System.Text.Json;
using PcQuitTimer.Models;

namespace PcQuitTimer.Services;

public class SchedulerService : ISchedulerService
{
    private static readonly string ConfigPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "PcQuitTimer",
        "schedules.json");

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    public List<ScheduleEntry> Load()
    {
        if (!File.Exists(ConfigPath))
            return [];

        var json = File.ReadAllText(ConfigPath);
        return JsonSerializer.Deserialize<List<ScheduleEntry>>(json, JsonOptions) ?? [];
    }

    public void Save(List<ScheduleEntry> entries)
    {
        var dir = Path.GetDirectoryName(ConfigPath)!;
        Directory.CreateDirectory(dir);

        var json = JsonSerializer.Serialize(entries, JsonOptions);
        File.WriteAllText(ConfigPath, json);
    }
}
