using System.Globalization;

namespace DGsys.UI.Components.Grid;

public enum FilterOperator { Equals, NotEquals, StartsWith, EndsWith, Contains, NotContains, LessOrEqual, GreaterOrEqual }

/// <summary>Filtre « contextuel » d'une colonne (équivalent du filtre de colonne des tables WinDev).</summary>
public sealed record ColumnFilter(FilterOperator Operator, string Value)
{
    private static readonly CultureInfo Fr = CultureInfo.GetCultureInfo("fr-FR");

    public static string Label(FilterOperator op) => op switch
    {
        FilterOperator.Equals => "Est égal à ...",
        FilterOperator.NotEquals => "Est différent de ...",
        FilterOperator.StartsWith => "Commence par ...",
        FilterOperator.EndsWith => "Se termine par ...",
        FilterOperator.Contains => "Contient ...",
        FilterOperator.NotContains => "Ne contient pas ...",
        FilterOperator.LessOrEqual => "Est inférieur ou égal à ...",
        FilterOperator.GreaterOrEqual => "Est supérieur ou égal à ...",
        _ => op.ToString(),
    };

    /// <param name="raw">Valeur brute de la cellule (pour les comparaisons numériques/dates).</param>
    /// <param name="display">Texte affiché de la cellule (pour les comparaisons textuelles).</param>
    public bool Matches(object? raw, string display)
    {
        var cmp = StringComparison.CurrentCultureIgnoreCase;
        // Les comparaisons de texte sont insensibles à la casse et à la culture fr-FR.
        var text = display ?? "";
        var v = Value ?? "";
        return Operator switch
        {
            FilterOperator.Equals => string.Equals(text, v, cmp),
            FilterOperator.NotEquals => !string.Equals(text, v, cmp),
            FilterOperator.StartsWith => text.StartsWith(v, cmp),
            FilterOperator.EndsWith => text.EndsWith(v, cmp),
            FilterOperator.Contains => text.Contains(v, cmp),
            FilterOperator.NotContains => !text.Contains(v, cmp),
            FilterOperator.LessOrEqual => Compare(raw, text, v) <= 0,
            FilterOperator.GreaterOrEqual => Compare(raw, text, v) >= 0,
            _ => true,
        };
    }

    private static int Compare(object? raw, string text, string value)
    {
        if (raw is not null && TryNumber(raw, out var a) && decimal.TryParse(value, NumberStyles.Any, Fr, out var b))
            return a.CompareTo(b);
        if (raw is DateTime d && DateTime.TryParse(value, Fr, DateTimeStyles.None, out var dv))
            return d.CompareTo(dv);
        return string.Compare(text, value, StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool TryNumber(object raw, out decimal n)
    {
        switch (raw)
        {
            case decimal m: n = m; return true;
            case int or long or short or byte: n = Convert.ToDecimal(raw); return true;
            case double or float: n = Convert.ToDecimal(raw); return true;
            default: n = 0; return false;
        }
    }
}
