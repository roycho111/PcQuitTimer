using PcQuitTimer.Models;

namespace PcQuitTimer.Services;

public interface ISchedulerService
{
    List<ScheduleEntry> Load();
    void Save(List<ScheduleEntry> entries);
}
