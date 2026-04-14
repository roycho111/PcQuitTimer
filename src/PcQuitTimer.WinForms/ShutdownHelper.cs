using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PcQuitTimer;

public enum PowerAction
{
    Shutdown,
    Restart,
    Sleep,
    Hibernate,
    LogOff,
    Lock
}

public static class ShutdownHelper
{
    [DllImport("user32.dll")]
    private static extern bool LockWorkStation();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

    [DllImport("powrprof.dll", SetLastError = true)]
    private static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

    public static void Execute(PowerAction action)
    {
        switch (action)
        {
            case PowerAction.Shutdown:
                Process.Start("shutdown", "/s /t 0");
                break;
            case PowerAction.Restart:
                Process.Start("shutdown", "/r /t 0");
                break;
            case PowerAction.Sleep:
                SetSuspendState(false, true, true);
                break;
            case PowerAction.Hibernate:
                SetSuspendState(true, true, true);
                break;
            case PowerAction.LogOff:
                ExitWindowsEx(0, 0);
                break;
            case PowerAction.Lock:
                LockWorkStation();
                break;
        }
    }

    public static string GetDisplayName(PowerAction action) => Strings.GetActionName(action);
}
