using Domen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Klijent
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = Enum.GetValues(typeof(Oblast));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Komunikacija k = new Komunikacija();
            TransferKlasa transfer = new TransferKlasa();
            Korisnik korisnik = new Korisnik();

            korisnik.Mail = txtMail.Text;
            korisnik.Password = txtPass.Text;
            try
            {
                korisnik.Oblast = (Oblast)Enum.Parse(typeof(Oblast), comboBox1.SelectedItem.ToString());
            }
            catch (Exception)
            {

                MessageBox.Show("NISTE ODABRALI OBLAST");
                return;
            }

            transfer.Korisnik = korisnik;
            transfer = k.PoveziSeNaServer(transfer);

            if (transfer.Ulogovan)
            {
                new FrmGlavna(k, transfer.Korisnik).ShowDialog();
            }
            else
            {
                MessageBox.Show(transfer.Poruka);
            }
        }
    }
}
