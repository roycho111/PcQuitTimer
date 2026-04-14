using System.Windows;
using PcQuitTimer.ViewModels;

namespace PcQuitTimer.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
