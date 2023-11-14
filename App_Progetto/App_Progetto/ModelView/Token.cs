namespace App_Progetto.ModelView
{
    public record Token
    {
        required public string Value { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
