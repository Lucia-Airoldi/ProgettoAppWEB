namespace App_Progetto.Models
{
    public class Token
    {
        required public string Value { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
