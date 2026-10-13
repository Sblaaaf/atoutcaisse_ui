using System.Globalization;
using DGsys.UI;
using DGsys.UI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// Formatage français partout (montants « 1 234,56 € », dates JJ/MM/AAAA).
var fr = CultureInfo.GetCultureInfo("fr-FR");
CultureInfo.DefaultThreadCurrentCulture = fr;
CultureInfo.DefaultThreadCurrentUICulture = fr;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<DgsysApp>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Ici, enregistre tes vraies implémentations AVANT AddDgsysUi() pour remplacer les mocks, ex. :
// builder.Services.AddScoped<IClientService, ApiClientService>();
builder.Services.AddDgsysUi();

await builder.Build().RunAsync();
