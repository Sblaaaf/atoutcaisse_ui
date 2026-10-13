using DGsys.UI.Models;

namespace DGsys.UI.Services;

/// <summary>Ligne du ticket en cours.</summary>
public sealed class TicketLine
{
    public const int MarkCount = 5;

    public required CatalogItem Item { get; init; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal Total => UnitPrice * Quantity;
    /// <summary>Case « sélection » (1re colonne) puis colonnes de marquage (sens métier à confirmer).</summary>
    public bool Selected { get; set; }
    public bool[] Marks { get; } = new bool[MarkCount];
}

/// <summary>
/// État du ticket en cours : lignes, règlements, reste dû.
/// Logique pure, sans UI → testable unitairement et réutilisable côté serveur.
/// Les composants s'abonnent à <see cref="Changed"/> pour se rafraîchir.
/// </summary>
public sealed class TicketState
{
    private readonly List<TicketLine> _lines = new();
    private readonly List<Payment> _payments = new();

    public string Emplacement { get; set; } = "Emp 1";
    public int Numero { get; set; } = 21;
    public string Client { get; set; } = "Nom Prénom";
    public string Profil { get; set; } = "";
    public DateTime OpenedAt { get; set; } = DateTime.Now;

    public IReadOnlyList<TicketLine> Lines => _lines;
    public IReadOnlyList<Payment> Payments => _payments;
    public TicketLine? Current { get; private set; }

    public int ArticleCount => _lines.Sum(l => l.Quantity);
    public decimal Total => _lines.Sum(l => l.Total);
    public decimal Paid => _payments.Sum(p => p.Amount);
    public decimal Remaining => Math.Max(0, Total - Paid);
    /// <summary>Monnaie à rendre si le client a donné plus que le total.</summary>
    public decimal Change => Math.Max(0, Paid - Total);

    public event Action? Changed;

    /// <summary>Ajoute un article ; s'il est déjà la ligne courante, incrémente sa quantité.</summary>
    public void Add(CatalogItem item, int quantity = 1)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        var line = _lines.FirstOrDefault(l => l.Item.Id == item.Id && l.UnitPrice == item.Price);
        if (line is null)
        {
            line = new TicketLine { Item = item, UnitPrice = item.Price, Quantity = quantity };
            _lines.Add(line);
        }
        else
        {
            line.Quantity += quantity;
        }
        Current = line;
        Notify();
    }

    public void Select(TicketLine line) { Current = line; Notify(); }

    public void RemoveCurrent()
    {
        if (Current is null) return;
        _lines.Remove(Current);
        Current = _lines.LastOrDefault();
        Notify();
    }

    public void DuplicateCurrent()
    {
        if (Current is not null) Add(Current.Item);
    }

    /// <summary>Remise en pourcentage sur la ligne courante (0–100).</summary>
    public void DiscountCurrent(decimal percent)
    {
        if (Current is null) return;
        if (percent is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(percent));
        Current.UnitPrice = Math.Round(Current.Item.Price * (1 - percent / 100m), 2, MidpointRounding.AwayFromZero);
        Notify();
    }

    /// <summary>Enregistre un règlement. Montant null ou ≤ 0 → règle le reste dû.</summary>
    public Payment? Pay(string method, decimal? amount = null)
    {
        var value = amount is > 0 ? amount.Value : Remaining;
        if (value <= 0) return null;
        var p = new Payment(method, value);
        _payments.Add(p);
        Notify();
        return p;
    }

    public void CancelLastPayment()
    {
        if (_payments.Count == 0) return;
        _payments.RemoveAt(_payments.Count - 1);
        Notify();
    }

    public void Clear()
    {
        _lines.Clear();
        _payments.Clear();
        Current = null;
        Notify();
    }

    public void Notify() => Changed?.Invoke();
}
