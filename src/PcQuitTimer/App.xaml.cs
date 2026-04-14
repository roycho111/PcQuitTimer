using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PcQuitTimer.Services;
using PcQuitTimer.ViewModels;
using PcQuitTimer.Views;

namespace PcQuitTimer;

public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IShutdownService, ShutdownService>();
        services.AddSingleton<ISchedulerService, SchedulerService>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();
        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}
