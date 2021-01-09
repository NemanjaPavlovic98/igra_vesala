using Domen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Klijent
{
    public  class Komunikacija
    {
        Socket klijent;
        NetworkStream stream;
        BinaryFormatter formatter;
        public TransferKlasa PoveziSeNaServer(TransferKlasa transfer)
        {
            try
            {
                klijent = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                klijent.Connect("localhost", 9999);
                stream = new NetworkStream(klijent);
                formatter = new BinaryFormatter();

                formatter.Serialize(stream, transfer);
                return (TransferKlasa)formatter.Deserialize(stream);
            }
            catch (Exception)
            {
                transfer.Ulogovan = false;
                transfer.Poruka = "kORISNIK NIJE ULOGOVAN";
                return transfer;
            }
        }

        internal TransferKlasa Pogadjaj(TransferKlasa transfer)
        {
            formatter.Serialize(stream, transfer);

            return (TransferKlasa)formatter.Deserialize(stream);
        }
    }
}
