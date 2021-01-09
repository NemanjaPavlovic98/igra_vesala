using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
    public enum Oblast { Timovi, Gradovi, Predmet}
    [Serializable]
    public class Korisnik
    {
        public string Mail { get; set; }
        public string Password { get; set; }
        public Oblast Oblast { get; set; }
        public int MaxBrPokusaja { get; set; }
        public int BrojPokusaja { get; set; }
        public string  Rec { get; set; }
        public string  Maska { get; set; }

        public override bool Equals(object obj)
        {
            var x = obj as Korisnik;
            return (x != null && x.Mail == this.Mail); 
        }
    }
}
