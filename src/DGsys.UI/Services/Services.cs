using DGsys.UI.Models;

namespace DGsys.UI.Services;

// Contrats consommés par l'UI. Aucune logique métier ici : l'implémentation réelle
// (API REST, EF Core, gRPC…) remplacera les Mock via AddDgsysUi() + DI.

public interface IClientService
{
    Task<IReadOnlyList<ClientRow>> GetClientsAsync();
    Task<IReadOnlyList<UnpaidInvoice>> GetUnpaidInvoicesAsync();
    Task<IReadOnlyList<ActiveEvent>> GetActiveEventsAsync();
    Task<IReadOnlyList<string>> GetEtatComptesModelesAsync();
}

public interface IArticleService
{
    Task<OrderTracking> GetOrderTrackingAsync();
    Task<StockSummary> GetStockSummaryAsync();
}

public interface IObjectifsService
{
    Task<ObjectifsSettings> GetAsync();
    Task SaveAsync(ObjectifsSettings settings);
}

public interface IClotureService
{
    Task<IReadOnlyList<Cloture>> GetAsync(ClotureKind kind);
}

/// <summary>Contrôles de fenêtre (hôte desktop). Par défaut : no-op dans le navigateur.</summary>
public interface IWindowHost
{
    bool IsAvailable { get; }
    void Minimize();
    void ToggleFullscreen();
    void Close();
}

public sealed class NoopWindowHost : IWindowHost
{
    public bool IsAvailable => false;
    public void Minimize() { }
    public void ToggleFullscreen() { }
    public void Close() { }
}
