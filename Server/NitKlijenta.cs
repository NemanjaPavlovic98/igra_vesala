using Domen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class NitKlijenta
    {
        NetworkStream stream;
        Korisnik k;
        BinaryFormatter formatter;


        public NitKlijenta(NetworkStream stream, Korisnik k)
        {
            this.stream = stream;
            this.k = k;
            formatter = new BinaryFormatter();
            Thread threadKlijent = new Thread(Obradi);
            threadKlijent.IsBackground = true;
            threadKlijent.Start();
        }
        void Obradi()
        {
            try
            {
                int operacija = 0;
                while (operacija != (int)Operacije.Kraj)
                {
                    TransferKlasa transfer = (TransferKlasa)formatter.Deserialize(stream);

                    switch (transfer.Operacija)
                    {
                        case Operacije.Pogadjaj:
                            k.BrojPokusaja++;
                            k = transfer.Korisnik;

                          
                            if (k.Rec.ToUpper().Contains(transfer.Slovo))
                            {
                                for (int i = 0; i < k.Rec.Length; i++)
                                {
                                    if(k.Rec[i].ToString().ToUpper() == transfer.Slovo)
                                    {
                                        k.Maska = k.Maska.Substring(0, i) + transfer.Slovo + k.Maska.Substring(i+1);
                                    }
                                }
                            }
                            if (!k.Maska.Contains('*'))
                            {
                                transfer.Kraj = true;
                                transfer.Poruka = "Pogodili ste rec";
                            }

                            if (!transfer.Kraj && k.BrojPokusaja == k.MaxBrPokusaja)
                            {
                                transfer.Kraj = true;
                                transfer.Poruka = "Nema vise pokusaja!";
                            }

                            transfer.Korisnik = k;
                            formatter.Serialize(stream, transfer);
                            
                            break;
                        case Operacije.Kraj:
                            Server.listaKorisnika.Remove(k);
                            operacija = 1;
                            
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                Server.listaKorisnika.Remove(k);
                return;
            }
        }
    }
}
