namespace App_Progetto.Models
{
    public class Sensore
    {
        public Sensore()
        {
            Misuraziones = new List<Misurazione>();
        }
        public int Id { get; set; }

        required public bool StatoSensore { get; set; }

        required public string TipoSensore { get; set; }

        public int TerrenoId { get; set; } // Required foreign key property
        required public Terreno Terreno { get; set; } // Required reference navigation to principal

        public ICollection<Misurazione> Misuraziones { get; set; }

    }
}
