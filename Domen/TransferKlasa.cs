using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
    public enum Operacije { Kraj = 1,
        Pogadjaj = 2
    }
    [Serializable]
    public class TransferKlasa
    {
        public Operacije Operacija { get; set; }
        public Korisnik Korisnik { get; set; }
        public string Maska { get; set; }
        public bool Ulogovan { get; set; }
        public string Poruka  { get; set; }
        public bool Kraj { get; set; }
        public string Slovo { get; set; }
    }
}
