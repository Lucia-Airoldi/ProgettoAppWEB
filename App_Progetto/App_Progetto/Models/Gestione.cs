namespace App_Progetto.Models
{
    public class Gestione
    {
        public int Id { get; set; }
        required public string UserId { get; set; } // Chiave esterna per l'utente dalla tabella AspNetUsers
        //public ApplicationUser? User { get; set; } // Riferimento all'utente

        required public string Ruolo { get; set; }

        public int TerrenoId { get; set; } // Chiave esterna per il terreno
        public Terreno Terreno { get; set; } // Riferimento al terreno

    }
}
