namespace App_Progetto.Models
{
    public class Terreno
    {
        public Terreno()
        {
            Attuatores = new List<Attuatore>();
            Sensores = new List<Sensore>();
        }

        public int Id { get; set; }
        required public int Mappale { get; set; }

        required public int Foglio { get; set; }

        public int Ettari { get; set; }

        required public string CittaTerreno { get; set; }

        required public string TipoColtura { get; set; }

        required public string TipoTerreno { get; set; }

        public ICollection<Attuatore> Attuatores { get; set; }

        public ICollection<Sensore> Sensores { get; set; }

        // Proprietà di navigazione per la gestione delle associazioni tra utenti e terreni
        required public ICollection<Gestione> Gestiones { get; set; }
    }
}