using Microsoft.AspNetCore.Identity;

namespace App_Progetto.Models;

public class ApplicationUser : IdentityUser
{
    // Altri campi personalizzati per l'utente, se necessario
    public string Nome { get; set; }
    public string Cognome { get; set; }

    // Proprietà di navigazione per la gestione delle associazioni tra utenti e terreni
    //public ICollection<Gestione>? Associazioni { get; set; }
}