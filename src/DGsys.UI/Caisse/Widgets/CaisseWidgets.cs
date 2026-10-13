namespace DGsys.UI.Caisse.Widgets;

/// <summary>
/// Registre des widgets disponibles sur l'écran d'encaissement.
/// La disposition (ordre, largeur) vient de <c>ICaisseLayoutService</c> : ajouter un widget =
/// créer le composant + l'inscrire ici, sans toucher à la page.
/// </summary>
public static class CaisseWidgets
{
    public static IReadOnlyDictionary<string, Type> Registry { get; } = new Dictionary<string, Type>
    {
        ["ticket-info"] = typeof(TicketInfoWidget),
        ["ticket-lines"] = typeof(TicketLinesWidget),
        ["actions"] = typeof(ActionsWidget),
        ["users"] = typeof(UsersWidget),
    };
}
