using DGsys.UI.Caisse.Widgets;
using DGsys.UI.Components;
using DGsys.UI.Components.Grid;
using DGsys.UI.Pages;
using DGsys.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DGsys.UI.Tests;

public class ComponentTests : TestContext
{
    public ComponentTests()
    {
        Services.AddDgsysUi();
        JSInterop.Mode = JSRuntimeMode.Loose; // FocusAsync() dans Dialog / menus
    }

    private sealed record Row(string Nom, decimal Montant);

    private static readonly GridColumn<Row>[] Cols =
    {
        new() { Key = "nom", Title = "Nom", Value = r => r.Nom },
        new() { Key = "montant", Title = "Montant", Value = r => r.Montant, Format = "C2", Align = GridAlign.Right, Aggregate = true },
    };

    private static readonly Row[] Rows = { new("Zoé", 10m), new("Alice", 2.5m), new("Marc", 7.5m) };

    [Fact]
    public void Grille_trie_au_clic_sur_l_entete()
    {
        var cut = RenderComponent<DgGrid<Row>>(p => p.Add(x => x.Columns, Cols).Add(x => x.Items, Rows));
        cut.FindAll(".grid__sort")[0].Click();
        var names = cut.FindAll("tbody tr td:first-child").Select(td => td.TextContent).ToArray();
        Assert.Equal(new[] { "Alice", "Marc", "Zoé" }, names);
        Assert.Equal("ascending", cut.FindAll("th")[0].GetAttribute("aria-sort"));
    }

    [Fact]
    public void Grille_affiche_la_somme_en_pied()
    {
        var cut = RenderComponent<DgGrid<Row>>(p => p.Add(x => x.Columns, Cols).Add(x => x.Items, Rows).Add(x => x.Footer, GridFooter.Sum));
        var foot = cut.Find("tfoot").TextContent.Replace(' ', ' ').Replace(' ', ' ');
        Assert.Contains("Somme", foot);
        Assert.Contains("20,00 €", foot);
    }

    [Fact]
    public void Grille_filtre_contextuel_contient()
    {
        var cut = RenderComponent<DgGrid<Row>>(p => p.Add(x => x.Columns, Cols).Add(x => x.Items, Rows));
        cut.Find("button[aria-label='Filtrer la colonne Nom']").Click();
        cut.FindAll(".colmenu__item").First(b => b.TextContent.Contains("Contient")).Click();
        cut.Find(".colmenu__cond input").Input("ar");
        cut.Find(".colmenu__cond").Submit();
        Assert.Single(cut.FindAll("tbody tr"));
        Assert.Contains("Marc", cut.Find("tbody").TextContent);
    }

    [Fact]
    public void Toggle_expose_role_switch_et_bascule()
    {
        var value = false;
        var cut = RenderComponent<Toggle>(p => p.Add(x => x.AriaLabel, "Pop-up").Add(x => x.Value, false)
            .Add(x => x.ValueChanged, v => value = v));
        Assert.Equal("false", cut.Find("[role=switch]").GetAttribute("aria-checked"));
        cut.Find("button").Click();
        Assert.True(value);
    }

    [Fact]
    public void Widget_lignes_affiche_le_total_du_ticket()
    {
        var ticket = Services.GetRequiredService<TicketState>();
        ticket.Add(new("x", "Expresso", 1.80m, Models.TileColor.Yellow, "b"), 3);
        var cut = RenderComponent<TicketLinesWidget>();
        Assert.Contains("3 article(s)", cut.Markup);
        Assert.Contains("5,40", cut.Find(".tlines__total").TextContent);
    }

    [Fact]
    public void Page_objectifs_enregistre_les_modifications()
    {
        var cut = RenderComponent<Objectifs>();
        cut.WaitForElement("input[aria-label='Objectif bas Lundi']").Change("123.45");
        cut.FindAll("button").First(b => b.TextContent.Trim() == "Valider").Click();
        Assert.Contains("Objectifs enregistrés", cut.Markup);
        var saved = Services.GetRequiredService<IObjectifsService>().GetAsync().Result;
        Assert.Equal(123.45m, saved.Jours[0].Bas);
    }
}
