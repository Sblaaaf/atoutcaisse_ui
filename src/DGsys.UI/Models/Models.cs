namespace DGsys.UI.Models;

public sealed record ClientRow(int Id, string Code, string Civilite, string Nom, string Prenom, string Societe, string Pseudo);

public sealed record UnpaidInvoice(
    DateTime Date, int Numero, string Statut, string Societe, string Client,
    decimal Montant, decimal ResteARegler, string TypeReglement);

public sealed record ActiveEvent(string Nom, DateTime? Debut, DateTime? Fin);

public sealed record OrderTracking(int NonLivrees, int Partielles, int EnRetard);

public sealed record StockSummary(int Ok, int Surveillance, int Approvisionnement, int Urgent, int NonGeres);

public sealed record Cloture(int Numero, DateTime Date, DateTime Debut, DateTime Fin, int IdClotureMensuelle);

public enum ClotureKind { Journaliere, Mensuelle, Annuelle, ValidationPointsDeVente }

/// <summary>Objectifs de CA pour un jour de la semaine (bas / moyen / haut).</summary>
public sealed class ObjectifJour
{
    public required string Jour { get; init; }
    public decimal Bas { get; set; }
    public decimal Moyen { get; set; }
    public decimal Haut { get; set; }
}

public sealed class ObjectifsSettings
{
    public bool ObjectifVisuelActif { get; set; }
    public List<ObjectifJour> Jours { get; set; } = new();
    public bool PopupBas { get; set; }
    public bool PopupMoyen { get; set; }
    public bool PopupHaut { get; set; }
    public TimeOnly HeureChangementJournee { get; set; } = TimeOnly.MinValue;
    public bool AfficherValeursMonetaires { get; set; }
    public bool AfficherBarre { get; set; }
    public bool UtiliserConsommationsArchivees { get; set; }
    public string TexteBas { get; set; } = "";
    public string TexteMoyen { get; set; } = "";
    public string TexteHaut { get; set; } = "";
}

/// <summary>Filtre sauvegardable de l'écran « État daté des comptes clients ».</summary>
public sealed class EtatComptesFilter
{
    public string Modele { get; set; } = "";
    public bool FiltrerDateDebut { get; set; }
    public DateTime? DateDebut { get; set; }
    public bool FiltrerDateFin { get; set; }
    public DateTime? DateFin { get; set; }
    public bool FiltrerClotureDebut { get; set; }
    public string? IdClotureDebut { get; set; }
    public bool FiltrerClotureFin { get; set; }
    public string? IdClotureFin { get; set; }
}
