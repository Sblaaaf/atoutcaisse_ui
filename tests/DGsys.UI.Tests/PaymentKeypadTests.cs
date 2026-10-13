using DGsys.UI.Services;

namespace DGsys.UI.Tests;

public class PaymentKeypadTests
{
    private static PaymentKeypad Type(params string[] keys)
    {
        var pad = new PaymentKeypad();
        foreach (var k in keys) pad.Press(k);
        return pad;
    }

    [Fact]
    public void Saisie_decimale_avec_virgule() =>
        Assert.Equal(12.5m, Type("1", "2", ",", "5").Value);

    [Fact]
    public void Limite_a_deux_decimales() =>
        Assert.Equal(1.23m, Type("1", ",", "2", "3", "9").Value);

    [Fact]
    public void Virgule_seule_donne_zero_virgule() =>
        Assert.Equal("0,", Type(",").Entry);

    [Theory]
    [InlineData("+", 15)]
    [InlineData("-", 5)]
    [InlineData("*", 50)]
    [InlineData("/", 2)]
    public void Operations_de_base(string op, decimal expected) =>
        Assert.Equal(expected, Type("1", "0", op, "5", "=").Value);

    [Fact]
    public void Evaluation_gauche_a_droite() =>
        Assert.Equal(20m, Type("2", "+", "3", "*", "4", "=").Value); // (2+3)*4, comme une calculatrice de caisse

    [Fact]
    public void Division_par_zero_conserve_la_valeur() =>
        Assert.Equal(10m, Type("1", "0", "/", "0", "=").Value);

    [Fact]
    public void RAZ_vide_tout()
    {
        var pad = Type("9", "+", "1", "RAZ");
        Assert.True(pad.IsEmpty);
        Assert.Equal(0m, pad.Value);
    }

    [Fact]
    public void Affichage_au_format_francais() =>
        Assert.Equal("1 234,50 €", Type("1", "2", "3", "4", ",", "5").Display.Replace(' ', ' ').Replace(' ', ' '));
}
