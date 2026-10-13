using System.Globalization;
using System.Windows;

namespace DGsys.Wpf;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var fr = CultureInfo.GetCultureInfo("fr-FR");
        CultureInfo.DefaultThreadCurrentCulture = fr;
        CultureInfo.DefaultThreadCurrentUICulture = fr;
        base.OnStartup(e);
    }
}
