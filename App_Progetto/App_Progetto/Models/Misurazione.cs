namespace App_Progetto.Models
{
    public class Misurazione
    {
        public int Id { get; set; }
        public DateTime DataOra { get; set; } // Identificata da ora e data
        public int CodiceSensore { get; set; } // Identificata dal codice del sensore

        public Sensore? Sensores { get; set; }
        public float Valore { get; set; }
        required public string TipoMisurazione { get; set; }

    }
}
