namespace DGsys.UI.Models;

/// <summary>Couleur de tuile article (palette de l'écran de caisse).</summary>
public enum TileColor { White, Yellow, Green, Blue, Pink }

public sealed record CatalogCategory(string Id, string Label);

public sealed record CatalogFolder(string Id, string Label, IReadOnlyList<CatalogItem> Items);

public sealed record CatalogItem(string Id, string Label, decimal Price, TileColor Color, string CategoryId, bool Favorite = false);

public sealed record Payment(string Method, decimal Amount);

public enum DeliveryStatus { PasPret, Pret, Livre }

public sealed record DeliveryOrder(
    string Emplacement, int Numero, DateTime Date, DeliveryStatus Status, IReadOnlyList<string> Tags,
    decimal ARegler, string Client, int Points, bool Cadeau);

public sealed record CaisseUser(string Name, bool Active);

/// <summary>
/// Disposition configurable de l'écran d'encaissement : ordre et largeur relative des widgets
/// de la bande haute. Base du futur paramétrage (et glisser-déposer).
/// </summary>
public sealed record CaisseLayoutConfig(IReadOnlyList<WidgetSlot> TopRow);

/// <param name="Key">Clé du widget dans <c>CaisseWidgets.Registry</c>.</param>
/// <param name="Width">Largeur relative (unités fr de CSS Grid).</param>
public sealed record WidgetSlot(string Key, double Width);
