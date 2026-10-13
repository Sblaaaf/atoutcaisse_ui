using DGsys.UI.Models;
using DGsys.UI.Services;

namespace DGsys.UI.Tests;

public class TicketStateTests
{
    private static readonly CatalogItem Cafe = new("cafe", "Expresso", 1.80m, TileColor.Yellow, "boissons");
    private static readonly CatalogItem Croissant = new("croissant", "Croissant", 1.20m, TileColor.Green, "desserts");

    [Fact]
    public void Ajouter_deux_fois_le_meme_article_incremente_la_quantite()
    {
        var t = new TicketState();
        t.Add(Cafe);
        t.Add(Cafe);
        Assert.Single(t.Lines);
        Assert.Equal(2, t.Lines[0].Quantity);
        Assert.Equal(3.60m, t.Total);
    }

    [Fact]
    public void Reglement_partiel_puis_reste()
    {
        var t = new TicketState();
        t.Add(Cafe, 2);
        t.Add(Croissant);           // total 4,80
        t.Pay("Espèces", 2m);
        Assert.Equal(2.80m, t.Remaining);
        t.Pay("CB");                // sans montant → règle le reste
        Assert.Equal(0m, t.Remaining);
        Assert.Equal(4.80m, t.Paid);
    }

    [Fact]
    public void Rendu_monnaie_quand_le_client_donne_plus()
    {
        var t = new TicketState();
        t.Add(Croissant);
        t.Pay("Espèces", 5m);
        Assert.Equal(3.80m, t.Change);
        Assert.Equal(0m, t.Remaining);
    }

    [Fact]
    public void Remise_sur_ligne_courante_arrondie_au_centime()
    {
        var t = new TicketState();
        t.Add(Cafe);
        t.DiscountCurrent(10);
        Assert.Equal(1.62m, t.Total);
    }

    [Fact]
    public void Supprimer_la_ligne_courante_selectionne_la_precedente()
    {
        var t = new TicketState();
        t.Add(Cafe);
        t.Add(Croissant);
        t.RemoveCurrent();
        Assert.Same(t.Lines[0], t.Current);
        Assert.Equal("cafe", t.Current!.Item.Id);
    }

    [Fact]
    public void Annuler_le_dernier_reglement()
    {
        var t = new TicketState();
        t.Add(Cafe);
        t.Pay("CB", 1m);
        t.Pay("Espèces", 0.5m);
        t.CancelLastPayment();
        Assert.Equal(1m, t.Paid);
    }

    [Fact]
    public void Changed_est_declenche_a_chaque_modification()
    {
        var t = new TicketState();
        var count = 0;
        t.Changed += () => count++;
        t.Add(Cafe);
        t.Pay("CB");
        t.Clear();
        Assert.Equal(3, count);
    }

    [Fact]
    public void Quantite_negative_refusee() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new TicketState().Add(Cafe, 0));
}
