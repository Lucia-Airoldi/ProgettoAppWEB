namespace App_Progetto.Models
{
    public class Attuatore
    {
        public int Id { get; set; }

        required public string TipoAttuatore { get; set; }

        public bool Standby { get; set; }

        public bool Attivazione { get; set; }
        public int TerrenoId { get; set; } // Required foreign key property
        required public Terreno Terreno { get; set; } // Required reference navigation to principal

        required public Piano Pianos { get; set; }
    }
}
