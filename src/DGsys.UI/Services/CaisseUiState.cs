namespace DGsys.UI.Services;

public enum DrawerTab { Client, Zone, Livraison, Attente }

/// <summary>
/// État d'interface partagé entre le layout de caisse et l'écran d'encaissement
/// (volet latéral, messages, CA du jour). Distinct de <see cref="TicketState"/> qui reste purement métier.
/// </summary>
public sealed class CaisseUiState
{
    public bool DrawerOpen { get; private set; }
    public DrawerTab Tab { get; private set; } = DrawerTab.Livraison;
    public string? Message { get; private set; }
    /// <summary>CA encaissé depuis l'ouverture (alimente la barre d'objectif).</summary>
    public decimal DailyRevenue { get; private set; }

    public event Action? Changed;

    public void OpenDrawer(DrawerTab tab) { Tab = tab; DrawerOpen = true; Changed?.Invoke(); }
    public void ToggleDrawer() { DrawerOpen = !DrawerOpen; Changed?.Invoke(); }
    public void CloseDrawer() { DrawerOpen = false; Changed?.Invoke(); }
    public void Notify(string message) { Message = message; Changed?.Invoke(); }
    public void ClearMessage() { Message = null; Changed?.Invoke(); }
    public void AddRevenue(decimal amount) { DailyRevenue += amount; Changed?.Invoke(); }
}
