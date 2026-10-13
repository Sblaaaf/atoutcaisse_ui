using DGsys.UI.Services.Mock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DGsys.UI.Services;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Enregistre les services par défaut (données factices).
    /// À appeler EN DERNIER : TryAdd n'écrase pas les implémentations déjà enregistrées par l'hôte.
    /// </summary>
    public static IServiceCollection AddDgsysUi(this IServiceCollection services)
    {
        services.TryAddScoped<IClientService, MockClientService>();
        services.TryAddScoped<IArticleService, MockArticleService>();
        services.TryAddScoped<IObjectifsService, MockObjectifsService>();
        services.TryAddScoped<IClotureService, MockClotureService>();
        services.TryAddScoped<ICatalogService, MockCatalogService>();
        services.TryAddScoped<IDeliveryService, MockDeliveryService>();
        services.TryAddScoped<ICaisseLayoutService, MockCaisseLayoutService>();
        // Un ticket en cours par session/fenêtre de caisse.
        services.TryAddScoped<TicketState>();
        services.TryAddScoped<CaisseUiState>();
        services.TryAddSingleton<IWindowHost, NoopWindowHost>();
        return services;
    }
}
