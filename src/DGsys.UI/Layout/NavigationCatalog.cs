namespace DGsys.UI.Layout;

public sealed record MenuEntry(string Label, string Href, string Icon);
public sealed record MenuSection(string? Title, IReadOnlyList<MenuEntry> Entries);
public sealed record MenuModule(string Key, string Label, string Icon, string Href, string HomeLabel, IReadOnlyList<MenuSection> Sections);

/// <summary>Source unique de la navigation : menu latéral ET pages d'accueil de module.</summary>
public static class NavigationCatalog
{
    private static readonly IReadOnlyList<MenuSection> None = Array.Empty<MenuSection>();

    public static IReadOnlyList<MenuModule> Modules { get; } = new MenuModule[]
    {
        new("dashboard", "Tableau de bord", "house", "/dashboard", "Accueil", None),
        new("client", "Client", "user", "/clients", "Accueil Relation Client", new MenuSection[]
        {
            new("Listes", new MenuEntry[]
            {
                new("Client", "/clients/liste", "address-card"),
                new("Évènements", "/clients/evenements", "file-lines"),
                new("Devis", "/clients/devis", "calendar-days"),
                new("Factures impayées", "/clients/factures-impayees", "file-invoice-dollar"),
            }),
            new("Fonctionnalités", new MenuEntry[]
            {
                new("Recherche multi-critères", "/clients/recherche", "magnifying-glass-plus"),
                new("Fidélité", "/clients/fidelite", "user-check"),
                new("État des comptes", "/clients/etat-comptes", "table-cells-large"),
                new("Clôture / RAZ", "/clients/cloture-raz", "user-slash"),
            }),
            new("Configurations", new MenuEntry[]
            {
                new("Paramètres par défaut", "/clients/parametres", "user-gear"),
            }),
        }),
        new("articles", "Articles", "bag-shopping", "/articles", "Accueil Gestion Article", new MenuSection[]
        {
            new("Listes", new MenuEntry[]
            {
                new("Fournisseur", "/articles/fournisseurs", "truck"),
                new("Produits Achetés", "/articles/achetes", "box"),
                new("Produits Vendus", "/articles/vendus", "basket-shopping"),
            }),
            new("Fonctionnalités", new MenuEntry[]
            {
                new("Inventaire", "/articles/inventaire", "clipboard-list"),
                new("Statistiques", "/articles/statistiques", "chart-pie"),
                new("Impression étiquettes", "/articles/etiquettes", "barcode"),
            }),
            new("Suivi des commandes", new MenuEntry[]
            {
                new("Proposition de commande", "/articles/propositions", "file-pen"),
                new("Commande", "/articles/commandes", "magnifying-glass"),
                new("Bon de livraison", "/articles/bons-livraison", "envelope-circle-check"),
            }),
        }),
        new("comptabilite", "Comptabilité", "file-invoice-dollar", "/comptabilite", "Accueil Comptabilité", None),
        new("personnel", "Personnel", "users", "/personnel", "Accueil Personnel", None),
        new("caisse", "Caisse", "cash-register", "/caisse", "Accueil Caisse", new MenuSection[]
        {
            new("Fonctionnalités utilisateur", new MenuEntry[]
            {
                new("Encaissement", "/caisse/encaissement", "cash-register"),
                new("Création utilisateur", "/caisse/utilisateurs", "cash-register"),
                new("Création profil de prix", "/caisse/profils-prix", "tag"),
                new("Gestion point de vente", "/caisse/points-de-vente", "desktop"),
            }),
            new("Configuration", new MenuEntry[]
            {
                new("Écran de services", "/caisse/ecran-services", "desktop"),
                new("Zones", "/caisse/zones", "location-dot"),
                new("Objectifs", "/caisse/objectifs", "bullseye"),
                new("Fonds de caisse", "/caisse/fonds-de-caisse", "money-bill-wave"),
                new("Liste des activités", "/caisse/activites", "list-check"),
                new("Moyen de travail", "/caisse/moyens-de-travail", "scissors"),
                new("Planning", "/caisse/planning", "calendar-days"),
                new("Clôtures", "/caisse/clotures", "file-invoice-dollar"),
            }),
        }),
        new("configuration", "Configuration", "gear", "/configuration", "Accueil Configuration", None),
    };

    public static MenuModule? ModuleFor(string path)
    {
        path = Normalize(path);
        return Modules.FirstOrDefault(m => path == m.Href || path.StartsWith(m.Href + "/", StringComparison.OrdinalIgnoreCase));
    }

    public static string Normalize(string path)
    {
        var q = path.IndexOfAny(new[] { '?', '#' });
        if (q >= 0) path = path[..q];
        path = "/" + path.Trim('/');
        return path.ToLowerInvariant();
    }
}
