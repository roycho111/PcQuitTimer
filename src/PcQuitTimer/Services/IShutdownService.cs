using PcQuitTimer.Models;

namespace PcQuitTimer.Services;

public interface IShutdownService
{
    void Execute(PowerAction action);
}
