using System.Globalization;

namespace DGsys.UI.Services;

/// <summary>
/// Pavé numérique de la zone Règlement (calculatrice simple : + - * / =, virgule décimale).
/// Logique pure, testable. Évaluation gauche → droite, comme une calculatrice de caisse.
/// </summary>
public sealed class PaymentKeypad
{
    private static readonly CultureInfo Fr = CultureInfo.GetCultureInfo("fr-FR");
    private const int MaxDigits = 12;

    private decimal? _accumulator;
    private char? _pending;
    private string _entry = "";

    /// <summary>Saisie courante (ex. "12,5"), vide si rien n'est saisi.</summary>
    public string Entry => _entry;

    /// <summary>Valeur affichée formatée en euros.</summary>
    public string Display => Value.ToString("#,##0.00 €", Fr);

    public decimal Value => _entry.Length > 0
        ? decimal.Parse(_entry.TrimEnd(','), NumberStyles.Number, Fr)
        : _accumulator ?? 0m;

    public bool IsEmpty => _entry.Length == 0 && _accumulator is null;

    public void Press(string key)
    {
        switch (key)
        {
            case "RAZ": Reset(); break;
            case ",":
                if (!_entry.Contains(',')) _entry = (_entry.Length == 0 ? "0" : _entry) + ",";
                break;
            case "+" or "-" or "*" or "/":
                Commit();
                _pending = key[0];
                break;
            case "=":
                Commit();
                _pending = null;
                break;
            default:
                if (key.Length == 1 && char.IsDigit(key[0]))
                {
                    var digits = _entry.Count(char.IsDigit);
                    var decimals = _entry.Contains(',') ? _entry.Length - _entry.IndexOf(',') - 1 : 0;
                    if (digits >= MaxDigits || decimals >= 2) return;
                    _entry = _entry == "0" ? key : _entry + key;
                }
                break;
        }
    }

    public void Reset()
    {
        _accumulator = null;
        _pending = null;
        _entry = "";
    }

    private void Commit()
    {
        if (_entry.Length == 0) return;
        var v = decimal.Parse(_entry.TrimEnd(','), NumberStyles.Number, Fr);
        _accumulator = (_accumulator, _pending) switch
        {
            (null, _) => v,
            (var a, '+') => a + v,
            (var a, '-') => a - v,
            (var a, '*') => Math.Round(a!.Value * v, 2, MidpointRounding.AwayFromZero),
            (var a, '/') => v == 0 ? a : Math.Round(a!.Value / v, 2, MidpointRounding.AwayFromZero),
            _ => v,
        };
        _entry = "";
    }
}
