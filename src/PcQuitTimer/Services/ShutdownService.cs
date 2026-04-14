using System.Diagnostics;
using System.Runtime.InteropServices;
using PcQuitTimer.Models;

namespace PcQuitTimer.Services;

public partial class ShutdownService : IShutdownService
{
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool LockWorkStation();

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ExitWindowsEx(uint uFlags, uint dwReason);

    [LibraryImport("powrprof.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.U1)]
    private static partial bool SetSuspendState(
        [MarshalAs(UnmanagedType.Bool)] bool hibernate,
        [MarshalAs(UnmanagedType.Bool)] bool forceCritical,
        [MarshalAs(UnmanagedType.Bool)] bool disableWakeEvent);

    public void Execute(PowerAction action)
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
}
