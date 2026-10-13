using System.Windows;
using DGsys.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DGsys.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var services = new ServiceCollection();
        services.AddWpfBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools(); // F12 dans la fenêtre
#endif
        // Services spécifiques au poste (avant AddDgsysUi pour remplacer les implémentations par défaut).
        services.AddSingleton<IWindowHost>(new WpfWindowHost(this));
        // services.AddSingleton<ICashDrawer, EscPosCashDrawer>(); // ← futur : matériel de caisse
        services.AddDgsysUi();

        Resources.Add("services", services.BuildServiceProvider());
        WebView.Services = (IServiceProvider)Resources["services"];
    }
}

/// <summary>Contrôles de fenêtre appelés depuis la barre haute Blazor.</summary>
internal sealed class WpfWindowHost(Window window) : IWindowHost
{
    public bool IsAvailable => true;

    // Les appels Blazor arrivent hors du thread UI WPF : on repasse par le Dispatcher.
    public void Minimize() => window.Dispatcher.Invoke(() => window.WindowState = WindowState.Minimized);

    public void ToggleFullscreen() => window.Dispatcher.Invoke(() =>
        window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);

    public void Close() => window.Dispatcher.Invoke(window.Close);
}
