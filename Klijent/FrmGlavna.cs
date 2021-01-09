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
    public partial class FrmGlavna : Form
    {
        private Komunikacija k;
        private Korisnik korisnik;

        public FrmGlavna()
        {
        }

        public FrmGlavna(Komunikacija k, Korisnik korisnik)
        {
            InitializeComponent();
            this.k = k;
            this.korisnik = korisnik;
        }

        private void FrmGlavna_Load(object sender, EventArgs e)
        {
            lblMaska.Text = korisnik.Maska;
            lblPokusanaSlova.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TransferKlasa transfer = new TransferKlasa();
            transfer.Korisnik = korisnik;

            try
            {
                Char c = Convert.ToChar(textBox1.Text);
                if (!Char.IsLetter(c))
                {
                    MessageBox.Show("Nije slovo!");
                    return;
                }
                transfer.Slovo = c.ToString().ToUpper();
                lblPokusanaSlova.Text += transfer.Slovo + " ";
                transfer.Operacija = Operacije.Pogadjaj;
                transfer = k.Pogadjaj(transfer);

                if (transfer.Kraj)
                {
                    MessageBox.Show(transfer.Poruka);
                    button1.Enabled = false;
                    this.Close();
                }

                korisnik = transfer.Korisnik;
                lblMaska.Text  = korisnik.Maska;
            }
            catch (Exception)
            {

                MessageBox.Show("Niste izabrali slovo!");
                return;
            }
        }
    }
}
