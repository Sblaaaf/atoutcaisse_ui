using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace DGsys.UI.Components.Grid;

public enum GridAlign { Left, Center, Right }

[Flags]
public enum GridFooter { None = 0, Sum = 1, Average = 2 }

public sealed class GridColumn<T>
{
    private static readonly CultureInfo Fr = CultureInfo.GetCultureInfo("fr-FR");

    public required string Key { get; init; }
    public required string Title { get; init; }
    public required Func<T, object?> Value { get; init; }

    /// <summary>Format .NET (ex. "C2", "dd/MM/yyyy HH:mm"), appliqué en culture fr-FR.</summary>
    public string? Format { get; init; }
    public GridAlign Align { get; init; } = GridAlign.Left;
    public string? Width { get; init; }
    public bool Sortable { get; init; } = true;
    public bool Filterable { get; init; } = true;
    /// <summary>Colonnes agrégées dans le pied de grille (Somme / Moyenne).</summary>
    public bool Aggregate { get; init; }
    public RenderFragment<T>? Template { get; init; }

    public string Display(T item) => Display(Value(item));

    public string Display(object? v) => v switch
    {
        null => "",
        IFormattable f when Format is not null => f.ToString(Format, Fr),
        IFormattable f => f.ToString(null, Fr) ?? "",
        _ => v.ToString() ?? "",
    };

    public bool TryNumber(T item, out decimal n)
    {
        switch (Value(item))
        {
            case decimal m: n = m; return true;
            case int or long or short or byte or double or float: n = Convert.ToDecimal(Value(item)); return true;
            default: n = 0; return false;
        }
    }

    public string? AggregateText(IEnumerable<T> rows, GridFooter kind)
    {
        var nums = new List<decimal>();
        foreach (var r in rows) if (TryNumber(r, out var n)) nums.Add(n);
        if (nums.Count == 0) return null;
        var result = kind == GridFooter.Sum ? nums.Sum() : nums.Average();
        return Display((object)result);
    }

    public static int CompareValues(object? a, object? b)
    {
        if (a is null && b is null) return 0;
        if (a is null) return -1;
        if (b is null) return 1;
        if (a is IComparable ca && a.GetType() == b.GetType()) return ca.CompareTo(b);
        return string.Compare(a.ToString(), b.ToString(), StringComparison.CurrentCultureIgnoreCase);
    }
}
