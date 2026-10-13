# DGsys – UI Blazor (migration WinDev 2024 → C#)

Front-end de la nouvelle version du logiciel d'encaissement DGsys, en **C# / Blazor (.NET 8)**.
Une seule bibliothèque de composants, deux hôtes :

| Projet | Rôle | Cible |
|---|---|---|
| `src/DGsys.UI` | **Tout l'UI** : layouts, écrans, composants, thème, contrats de services + données factices | `net8.0` (Razor Class Library) |
| `src/DGsys.Web` | Hôte navigateur (Blazor WebAssembly) : dev rapide, démo, Docker | `net8.0` |
| `src/DGsys.Wpf` | Hôte desktop (WPF + BlazorWebView/WebView2) : poste de caisse, accès matériel | `net8.0-windows` |
| `tests/DGsys.UI.Tests` | Tests xUnit + bUnit | `net8.0` |

> Analogie : `DGsys.UI` est le « moteur », les hôtes sont deux « carrosseries ». On écrit l'écran une fois et il tourne dans le navigateur comme dans la fenêtre Windows.

## Lancer

```bash
# Navigateur (Linux/macOS/Windows) → http://localhost:5080
dotnet run --project src/DGsys.Web

# Navigateur avec hot reload (rafraîchit à chaque sauvegarde)
dotnet watch --project src/DGsys.Web run

# Desktop (Windows uniquement, runtime WebView2 requis – présent par défaut sur Windows 10/11)
dotnet run --project src/DGsys.Wpf

# Tests
dotnet test tests/DGsys.UI.Tests

# Docker (front web servi par nginx) → http://localhost:8080
cp .env.example .env
docker compose up --build
```

Variables d'environnement : seulement `WEB_PORT` (port exposé par Docker, `8080` par défaut). Aucun secret n'est nécessaire pour l'instant.

## Travailler dans VS Code

À l'ouverture du dossier, VS Code propose les extensions recommandées (`.vscode/extensions.json`) : accepte, surtout **C# Dev Kit**.

| Action | Comment |
|---|---|
| Lancer + déboguer le web | `F5` → « Web (Blazor WebAssembly) » (points d'arrêt dans `.razor` / `.cs`) |
| Lancer + déboguer le desktop | `F5` → « Desktop (WPF) » |
| Hot reload pendant le design | `Ctrl+Maj+P` → *Tasks: Run Task* → **watch web** |
| Compiler | `Ctrl+Maj+B` |
| Tests | Onglet *Testing* (fiole) ou tâche **test** |
| Docker | Tâche **docker up** |

## Écrans disponibles

| Route | Écran |
|---|---|
| `/caisse/encaissement` (accueil) | Encaissement : barre d'objectif, widgets (ticket, lignes, actions, utilisateurs), sélection d'articles, règlement, volet latéral Client/Zone/Livraison/Attente |
| `/clients` | Gestion Clients (cartes, événements, clients actifs / factures non réglées) |
| `/clients/etat-comptes` | État daté des comptes clients (fenêtre + panneau Filtrage) |
| `/articles` | Gestion Articles (suivi des commandes, état du stock) |
| `/caisse/objectifs` | Objectifs (seuils par jour, pop-up, textes) |
| `/caisse/clotures` | Liste des clôtures à ré-imprimer |
| `/dashboard`, `/caisse`, `/comptabilite`, `/personnel`, `/configuration` | Accueils de module générés depuis le menu |
| toute autre route du menu | Page « Écran à migrer » (sert de check-list de migration) |

## Architecture

```
DGsys.UI/
├── Layout/            MainLayout (barre + menu), NavigationCatalog (source unique du menu)
├── Caisse/            CaisseLayout (rail + volet), écran d'encaissement
│   └── Widgets/       Widgets de la bande haute + registre CaisseWidgets
├── Components/        Icon (Font Awesome), Card, Dialog, TabStrip, Check, Toggle, Money…
│   └── Grid/          DgGrid<T> : tri, filtre contextuel (≈ tables WinDev), pieds Somme/Moyenne
├── Pages/             Écrans de gestion
├── Models/            Records/DTO consommés par l'UI
├── Services/          Interfaces + TicketState / PaymentKeypad (logique pure, testée)
│   └── Mock/          Données factices (remplacées par les vrais services)
└── wwwroot/
    ├── css/dgsys.css  Design system (tokens de couleurs en tête de fichier)
    └── lib/           Font Awesome 6.7.2 Free Solid + Inter (locaux, hors-ligne)
```

### Brancher les vraies données

L'UI ne dépend que d'interfaces (`IClientService`, `ICatalogService`, `IClotureService`…).
`AddDgsysUi()` enregistre les **mocks** avec `TryAdd` : toute implémentation enregistrée **avant** gagne.

```csharp
// Program.cs (Web) ou MainWindow.xaml.cs (WPF)
builder.Services.AddScoped<IClientService, ApiClientService>(); // ta vraie implémentation
builder.Services.AddDgsysUi();                                   // complète avec les mocks restants
```

Exemple d'implémentation qui appelle une API REST :

```csharp
public sealed class ApiClientService(HttpClient http) : IClientService
{
    public async Task<IReadOnlyList<ClientRow>> GetClientsAsync() =>
        await http.GetFromJsonAsync<List<ClientRow>>("api/clients") ?? [];
    // …
}
```

```bash
curl -s http://localhost:5000/api/clients
```

### Écran d'encaissement modulable

La bande haute est pilotée par `ICaisseLayoutService` → `CaisseLayoutConfig(TopRow: [WidgetSlot(Key, Width)])`
et rendue via `DynamicComponent`. Ajouter un widget = créer le composant + l'inscrire dans `CaisseWidgets.Registry`.
C'est la base du futur paramétrage par poste et du glisser-déposer (la poignée ⋮⋮ est déjà en place, inactive).

## Choix techniques

- **Blazor + RCL** : réutilisation des compétences HTML/CSS, un seul code pour navigateur et desktop.
- **WPF BlazorWebView** plutôt que MAUI : plus stable sur Windows, et accès direct aux API .NET pour le matériel de caisse (imprimante ESC/POS, tiroir, TPE).
- **Icônes Font Awesome Free (Solid) et police Inter embarquées** : aucune requête réseau, indispensable pour une caisse hors-ligne.
  `<Icon Name="cash-register" />` accepte n'importe quel nom FA Solid sans le préfixe `fa-`.
- **Culture fr-FR forcée** (montants `1 234,56 €`, dates `JJ/MM/AAAA`).
- **Accessibilité** : vrais `<button>`/`<input>`, rôles ARIA (`dialog`, `tablist`, `switch`, `menu`, `meter`), Échap ferme les fenêtres, focus visible, cibles tactiles ≥ 44 px.

## Licences tierces

- Font Awesome Free 6.7.2 : icônes CC BY 4.0, police SIL OFL 1.1, code MIT (`wwwroot/lib/fontawesome/LICENSE.txt`).
- Inter (Fontsource) : SIL OFL 1.1 (`wwwroot/lib/inter/LICENSE.txt`).
