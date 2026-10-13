using DGsys.UI.Models;

namespace DGsys.UI.Services.Mock;

public sealed class MockCatalogService : ICatalogService
{
    private static readonly CatalogCategory[] Categories =
    {
        new("favoris", "Favoris"), new("menu", "Menu"), new("entrees", "Entrées"), new("plats", "Plats"),
        new("desserts", "Desserts"), new("boissons", "Boissons"), new("accomp", "Accompagnements"),
    };

    private static readonly CatalogItem[] Items = BuildItems();

    private static CatalogItem[] BuildItems()
    {
        var y = TileColor.Yellow; var g = TileColor.Green; var b = TileColor.Blue; var p = TileColor.Pink; var w = TileColor.White;
        var list = new List<CatalogItem>
        {
            new("croissant", "Croissant au beurre", 15.67m, y, "desserts", true),
            new("painchoc", "Pain au chocolat", 10.75m, g, "desserts", true),
            new("chausson", "Chausson aux pommes", 19.39m, b, "desserts", true),
            new("brioche", "Brioche sucre", 7.99m, p, "desserts", true),
            new("painraisins", "Pain aux raisins", 20.09m, w, "desserts", true),
            new("baguette", "Baguette Tradition", 15.92m, y, "accomp", true),
            new("baguettecer", "Baguette Céréales", 24.62m, g, "accomp", true),
            new("eclaircafe", "Éclair au café", 22.24m, b, "desserts", true),
            new("eclairchoc", "Éclair au chocolat", 11.34m, p, "desserts", true),
            new("tartelette", "Tartelette fraise", 8.76m, w, "desserts", true),
            new("tartecitron", "Tarte citron meringuée", 20.35m, y, "desserts", true),
            new("millefeuille", "Millefeuille vanille", 9.96m, g, "desserts", true),
            new("flan", "Flan parisien", 10.38m, b, "desserts", true),
            new("macaron", "Macaron framboise", 24.27m, p, "desserts", true),
            new("cookie", "Cookie pépites", 18.19m, w, "desserts", true),
            new("expresso", "Expresso", 16.70m, y, "boissons", true),
            new("allonge", "Café Allongé", 9.39m, g, "boissons", true),
            new("creme", "Café Crème", 9.47m, b, "boissons", true),
            new("cappuccino", "Cappuccino", 16.20m, p, "boissons", true),
            new("latte", "Latte Macchiato", 20.77m, w, "boissons", true),
            new("chocolat", "Chocolat Chaud", 6.11m, y, "boissons", true),
            new("menthe", "Thé Menthe", 23.06m, g, "boissons", true),
            new("thevert", "Thé Vert", 12.68m, b, "boissons", true),
            new("camomille", "Infusion Camomille", 14.16m, p, "boissons", true),
            new("viennois", "Café Viennois", 13.66m, w, "boissons", true),
            new("coca", "Coca-Cola 33cl", 8.34m, y, "boissons", true),
            new("cocazero", "Coca-Cola Zéro 33cl", 19.69m, g, "boissons", true),
            new("menuchef", "Menu du Chef", 3.31m, w, "menu"),
            new("cafeexpress", "Café Express", 6.31m, y, "boissons"),
            new("cesar", "Salade César", 4.50m, g, "entrees"),
            new("entrecote", "Entrecôte Frites", 10.28m, b, "plats"),
            new("tiramisu", "Tiramisu Maison", 5.29m, p, "desserts"),
            new("burger", "Burger vegan", 15.24m, w, "plats"),
        };
        // Articles supplémentaires pour illustrer la pagination.
        for (var i = 1; i <= 30; i++)
            list.Add(new($"plat{i}", $"Plat du jour {i}", 9.50m + i, (TileColor)(i % 5), "plats", true));
        return list.ToArray();
    }

    public Task<IReadOnlyList<CatalogCategory>> GetCategoriesAsync() => Task.FromResult<IReadOnlyList<CatalogCategory>>(Categories);

    public Task<IReadOnlyList<CatalogFolder>> GetFoldersAsync()
    {
        var sizes = new[] { 11, 23, 19, 10, 26 };
        var folders = sizes.Select((n, i) => new CatalogFolder($"dossier{i + 1}", $"Dossier {i + 1}",
            Enumerable.Range(1, n).Select(k => new CatalogItem($"d{i + 1}-{k}", $"Article {i + 1}.{k}", 2m + k * 0.5m, (TileColor)(k % 5), "favoris")).ToList()))
            .ToList();
        return Task.FromResult<IReadOnlyList<CatalogFolder>>(folders);
    }

    public Task<IReadOnlyList<CatalogItem>> GetItemsAsync() => Task.FromResult<IReadOnlyList<CatalogItem>>(Items);
}

public sealed class MockDeliveryService : IDeliveryService
{
    public Task<IReadOnlyList<string>> GetZonesAsync() =>
        Task.FromResult<IReadOnlyList<string>>(new[] { "Salle Principale", "RDC", "Terrasse" });

    public Task<IReadOnlyList<DeliveryOrder>> GetOrdersAsync(string zone)
    {
        var d = new DateTime(2026, 10, 13, 12, 0, 0);
        IReadOnlyList<DeliveryOrder> orders = new DeliveryOrder[]
        {
            new("Emp 1", 21, d, DeliveryStatus.PasPret, new[] { "Livraison" }, 21.40m, "DGSYS Bordanave", 112, true),
            new("Liv 4", 24, d, DeliveryStatus.PasPret, new[] { "À Emporter", "Web" }, 88.00m, "Marc Zuk", 56, false),
            new("Emp 5", 25, d, DeliveryStatus.PasPret, new[] { "Livraison" }, 10.50m, "Sophie Fonfec", 421, true),
            new("Emp 2", 22, d, DeliveryStatus.Pret, new[] { "Livraison" }, 34.00m, "Jean Martin", 12, false),
            new("Liv 6", 26, d, DeliveryStatus.Pret, new[] { "À Emporter" }, 15.20m, "Lucie Durand", 80, false),
            new("Emp 3", 23, d, DeliveryStatus.Livre, new[] { "Livraison" }, 0m, "Paul Bernard", 230, true),
        };
        return Task.FromResult(orders);
    }
}

public sealed class MockCaisseLayoutService : ICaisseLayoutService
{
    // Disposition par défaut = celle de la capture. Un écran de paramétrage pourra la modifier.
    public Task<CaisseLayoutConfig> GetAsync() => Task.FromResult(new CaisseLayoutConfig(new WidgetSlot[]
    {
        new("ticket-info", 1.1),
        new("ticket-lines", 2.1),
        new("actions", 1.1),
        new("users", 1.1),
    }));
}
