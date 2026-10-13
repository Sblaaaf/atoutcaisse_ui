using DGsys.UI.Models;

namespace DGsys.UI.Services;

public interface ICatalogService
{
    Task<IReadOnlyList<CatalogCategory>> GetCategoriesAsync();
    Task<IReadOnlyList<CatalogFolder>> GetFoldersAsync();
    Task<IReadOnlyList<CatalogItem>> GetItemsAsync();
}

public interface IDeliveryService
{
    Task<IReadOnlyList<DeliveryOrder>> GetOrdersAsync(string zone);
    Task<IReadOnlyList<string>> GetZonesAsync();
}

public interface ICaisseLayoutService
{
    Task<CaisseLayoutConfig> GetAsync();
}
