using System.ComponentModel.DataAnnotations;

namespace App_Progetto.Models
{
    public class Piano
    {
        [Key]
        public int CodicePiano { get; set; }

        required public TimeOnly OrarioAttivazione { get; set; }
        required public TimeOnly OrarioDisattivazione { get; set; }

        required public TimeOnly OrarioAttDefault { get; set; }

        required public TimeOnly OrarioDisattDefault { get; set; }

        required public string CondAttivazione { get; set; }

        required public string CondDisattivazione { get; set; }

        required public int CodAtt { get; set; }

        required public Attuatore Attuatores { get; set; }
    }
}
