using Domen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        Socket listener;
        public static List<Korisnik> listaKorisnika = new List<Korisnik>();

        List<String> listaGradova;
        List<String> listaTimova;
        List<String> listaPredmeta;

        void PopuniListe()
        {
            listaGradova = new List<string>();
            listaGradova.Add("Novi Sad");
            listaGradova.Add("Beograd");
            listaGradova.Add("Krusevac");
            listaGradova.Add("Nis");
            listaGradova.Add("Banjaluka");

            listaTimova = new List<string>();
            listaTimova.Add("Totenham");
            listaTimova.Add("Chelsea");
            listaTimova.Add("Liverpool");
            listaTimova.Add("Leicester");
            listaTimova.Add("Roma");

            listaPredmeta = new List<string>();
            listaPredmeta.Add("Olovka");
            listaPredmeta.Add("Tastatura");
            listaPredmeta.Add("Mis");
            listaPredmeta.Add("Solja");
            listaPredmeta.Add("Mobilni telefon");
        }

        public Server()
        {
            PopuniListe();
        }

        void OdaberiRec(Korisnik k)
        {
            Random r = new Random();
            int i = 0;
            switch (k.Oblast)
            {
                case Oblast.Timovi:
                    i = r.Next(0, listaTimova.Count);
                    k.Rec = listaTimova[i];
                    break;
                case Oblast.Gradovi:
                    i = r.Next(0, listaGradova.Count);
                    k.Rec = listaGradova[i];
                    break;
                case Oblast.Predmet:
                    i = r.Next(0, listaPredmeta.Count);
                    k.Rec = listaPredmeta[i];
                    break;
                default:
                    break;
            }
        }

        void NapraviMasku(Korisnik k)
        {
            k.Maska = "";
            foreach (Char c in k.Rec)
            {
                if (c == ' ') k.Maska += " ";
                k.Maska += "*";
            }
        }

        public bool PokreniServer()
        {
            try
            {
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, 9999);
                listener.Bind(ep);

                Thread threadKlijent = new Thread(Osluskuj);
                threadKlijent.IsBackground = true;
                threadKlijent.Start();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        void Osluskuj()
        {
            try
            {
                while (true)
                {
                    listener.Listen(5);
                    Socket klijent = listener.Accept();
                    NetworkStream stream = new NetworkStream(klijent);
                    BinaryFormatter formatter = new BinaryFormatter();

                    TransferKlasa transfer = (TransferKlasa)formatter.Deserialize(stream);
                     
                    Korisnik k = transfer.Korisnik;
                    transfer.Ulogovan = true;

                    if (listaKorisnika.Contains(k))
                    {
                        transfer.Ulogovan = false;
                        transfer.Poruka = "Vec si ulogovan";
                    }

                    if(transfer.Ulogovan && !k.Mail.Contains('@'))
                    {
                        transfer.Ulogovan = false;
                        transfer.Poruka = "Fali @";
                    }

                    if (transfer.Ulogovan && k.Password=="")
                    {
                        transfer.Ulogovan = false;
                        transfer.Poruka = "Niste uneli sifru";
                    }

                    if (transfer.Ulogovan && !Char.IsLetter(k.Password[0]))
                    {
                        transfer.Ulogovan = false;
                        transfer.Poruka = "Lozinka mora poceti slovom";
                    }

                    if (transfer.Ulogovan)
                    {
                        bool postoji = false;
                        foreach(Char c in k.Password)
                        {
                            if (Char.IsDigit(c))
                            {
                                postoji = true;
                                break; 
                            }
                        }
                        if (!postoji)
                        {
                            transfer.Ulogovan = false;
                            transfer.Poruka = "Nema broja u sifri";
                        }
                    }

                    if (transfer.Ulogovan)
                    {
                        OdaberiRec(k);
                        k.MaxBrPokusaja = k.Rec.Length + 8;
                        NapraviMasku(k); 
                        new NitKlijenta(stream, k);
                        listaKorisnika.Add(k);
                    }
                    transfer.Korisnik = k;  
                    formatter.Serialize(stream, transfer);

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Greska: " + ex.Message); ;
            }
        }

        public bool ZaustaviServer()
        {
            try
            {
                listener.Close();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
