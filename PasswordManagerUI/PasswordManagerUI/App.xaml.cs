using Microsoft.UI.Xaml;
using PasswordManager.Core;

namespace PasswordManagerUI;

public sealed partial class App : Application
{
    public static DependencyContainer Container { get; private set; } = null!;
    private Window _mainWindow = null!;

    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        Container = new DependencyContainer("vault.db", "meta.json");

        _mainWindow = new Window();

        var frame = new Frame();
        frame.Navigate(typeof(LoginPage));

        _mainWindow.Content = frame;
        _mainWindow.Title   = "Password Manager";
        _mainWindow.Activate();
    }

    public static void InitializeLogging()
    {
        
    }
}
