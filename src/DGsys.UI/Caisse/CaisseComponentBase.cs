using DGsys.UI.Services;
using Microsoft.AspNetCore.Components;

namespace DGsys.UI.Caisse;

/// <summary>Base des composants de caisse : se redessine quand le ticket ou l'UI de caisse change.</summary>
public abstract class CaisseComponentBase : ComponentBase, IDisposable
{
    [Inject] protected TicketState Ticket { get; set; } = default!;
    [Inject] protected CaisseUiState Ui { get; set; } = default!;

    protected override void OnInitialized()
    {
        Ticket.Changed += Refresh;
        Ui.Changed += Refresh;
    }

    private void Refresh() => InvokeAsync(StateHasChanged);

    public virtual void Dispose()
    {
        Ticket.Changed -= Refresh;
        Ui.Changed -= Refresh;
        GC.SuppressFinalize(this);
    }

    protected static readonly System.Globalization.CultureInfo Fr = System.Globalization.CultureInfo.GetCultureInfo("fr-FR");
    protected static string Eur(decimal v) => v.ToString("#,##0.00 €", Fr);
}
