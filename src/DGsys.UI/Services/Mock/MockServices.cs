using DGsys.UI.Models;

namespace DGsys.UI.Services.Mock;

public sealed class MockClientService : IClientService
{
    public Task<IReadOnlyList<ClientRow>> GetClientsAsync() => Task.FromResult<IReadOnlyList<ClientRow>>(new ClientRow[]
    {
        new(1, "", "", "aaaaaOLLIVIER", "", "dgsys", "dfgfgdfgdgfgd"),
        new(2, "", "", "Nom aaaaa", "Prenom aaaaa", "0", ""),
        new(3, "", "", "Devis", "", "", ""),
        new(4, "", "", "", "", "zzzzzz", "zzzzzz"),
        new(11, "", "", "OLLIVIER", "Maxens", "", ""),
        new(12, "", "", "TEST", "Planing", "", ""),
        new(13, "", "", "TATZRTZR", "dfgdfgfdg", "", ""),
        new(14, "", "", "bhhjh", "", "", ""),
        new(15, "", "", "GouvTest", "", "aaaa", ""),
        new(16, "", "M.", "LUNDI", "Neuf", "", ""),
        new(17, "", "M.", "DIXNEUF", "Decembre", "", ""),
        new(18, "", "", "test extra1", "", "", ""),
    });

    public Task<IReadOnlyList<UnpaidInvoice>> GetUnpaidInvoicesAsync() => Task.FromResult<IReadOnlyList<UnpaidInvoice>>(new UnpaidInvoice[]
    {
        new(new(2026, 1, 14, 9, 51, 0), 564, "Particulier", "", "OLLIVIER Maxens", 9.99m, 9.99m, ""),
        new(new(2026, 1, 13, 11, 18, 0), 563, "Particulier", "", "OLLIVIER Maxens", -9.99m, -9.99m, "Carte CB"),
        new(new(2026, 1, 12, 14, 6, 0), 562, "Particulier", "", "Livraison Test", 22.00m, 22.00m, "Carte CB"),
        new(new(2026, 4, 17, 14, 8, 0), 703, "Particulier", "", "teste prog fid 2", 61.20m, 61.20m, "Carte CB"),
        new(new(2026, 2, 12, 15, 31, 0), 637, "Particulier", "", "test imp liv", 142.00m, 142.00m, "Carte CB"),
        new(new(2026, 2, 12, 15, 32, 0), 638, "Particulier", "", "test imp liv", 47.49m, 47.49m, "Carte CB"),
        new(new(2026, 2, 12, 15, 31, 0), 636, "Particulier", "", "test imp liv", 294.40m, 294.40m, "Carte CB"),
    });

    public Task<IReadOnlyList<ActiveEvent>> GetActiveEventsAsync() => Task.FromResult<IReadOnlyList<ActiveEvent>>(new ActiveEvent[]
    {
        new("aa", null, null),
        new("test", new(2026, 9, 5), new(2026, 9, 6)),
    });

    public Task<IReadOnlyList<string>> GetEtatComptesModelesAsync() =>
        Task.FromResult<IReadOnlyList<string>>(new[] { "eWallets_Dates_TEST", "Comptes_Mensuels", "Clients_Actifs" });
}

public sealed class MockArticleService : IArticleService
{
    public Task<OrderTracking> GetOrderTrackingAsync() => Task.FromResult(new OrderTracking(NonLivrees: 0, Partielles: 5, EnRetard: 0));
    public Task<StockSummary> GetStockSummaryAsync() => Task.FromResult(new StockSummary(Ok: 54, Surveillance: 15, Approvisionnement: 1, Urgent: 45, NonGeres: 21));
}

public sealed class MockObjectifsService : IObjectifsService
{
    private ObjectifsSettings _state = Seed();

    public Task<ObjectifsSettings> GetAsync() => Task.FromResult(Clone(_state));

    public Task SaveAsync(ObjectifsSettings settings)
    {
        _state = Clone(settings);
        return Task.CompletedTask;
    }

    private static ObjectifsSettings Clone(ObjectifsSettings s) => new()
    {
        ObjectifVisuelActif = s.ObjectifVisuelActif,
        Jours = s.Jours.Select(j => new ObjectifJour { Jour = j.Jour, Bas = j.Bas, Moyen = j.Moyen, Haut = j.Haut }).ToList(),
        PopupBas = s.PopupBas, PopupMoyen = s.PopupMoyen, PopupHaut = s.PopupHaut,
        HeureChangementJournee = s.HeureChangementJournee,
        AfficherValeursMonetaires = s.AfficherValeursMonetaires,
        AfficherBarre = s.AfficherBarre,
        UtiliserConsommationsArchivees = s.UtiliserConsommationsArchivees,
        TexteBas = s.TexteBas, TexteMoyen = s.TexteMoyen, TexteHaut = s.TexteHaut,
    };

    private static ObjectifsSettings Seed() => new()
    {
        ObjectifVisuelActif = true,
        Jours =
        {
            new() { Jour = "Lundi", Bas = 100, Moyen = 150, Haut = 200 },
            new() { Jour = "Mardi", Bas = 110, Moyen = 160, Haut = 210 },
            new() { Jour = "Mercredi", Bas = 450, Moyen = 863, Haut = 1563 },
            new() { Jour = "Jeudi", Bas = 130, Moyen = 180, Haut = 230 },
            new() { Jour = "Vendredi", Bas = 140, Moyen = 190, Haut = 240 },
            new() { Jour = "Samedi", Bas = 300, Moyen = 350, Haut = 400 },
            new() { Jour = "Dimanche", Bas = 400, Moyen = 450, Haut = 500 },
        },
        PopupBas = true, PopupMoyen = true, PopupHaut = true,
        HeureChangementJournee = TimeOnly.MinValue,
        AfficherValeursMonetaires = true, AfficherBarre = true, UtiliserConsommationsArchivees = true,
        TexteBas = "Objectif minimum atteint !\n\n…et ce n'est que le début.",
        TexteMoyen = "Objectif moyen atteint !!\n\nOn continue l'effort, Bravo !",
        TexteHaut = "Objectif haut atteint !!!\n\nFÉLICITATIONS !\n\nPrime pour tout le monde !",
    };
}

public sealed class MockClotureService : IClotureService
{
    public Task<IReadOnlyList<Cloture>> GetAsync(ClotureKind kind)
    {
        var rows = new List<Cloture>();
        switch (kind)
        {
            case ClotureKind.Journaliere:
                var start = new DateTime(2026, 10, 1, 10, 8, 0);
                for (var n = 339; n >= 300; n--)
                {
                    var d = start.AddDays(-(339 - n) * 1.2);
                    rows.Add(new(n, new(2027, 1, 1), d, d.AddHours(5.6), 0));
                }
                break;
            case ClotureKind.Mensuelle:
                for (var m = 9; m >= 1; m--)
                    rows.Add(new(m, new(2026, m, 28), new(2026, m, 1, 0, 0, 0), new(2026, m, 28, 23, 59, 0), 0));
                break;
            case ClotureKind.Annuelle:
                rows.Add(new(2025, new(2025, 12, 31), new(2025, 1, 1), new(2025, 12, 31, 23, 59, 0), 0));
                break;
        }
        return Task.FromResult<IReadOnlyList<Cloture>>(rows);
    }
}
