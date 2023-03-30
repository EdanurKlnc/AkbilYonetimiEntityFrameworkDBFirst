

using System.Data;
using System.Data.SqlClient;
using AkbilYonetimiVeriKatmani;
using AkbilYonetimiIsKatmani;
using AkbilYonetimiVeriKatmani.Models;

namespace AkbilYonetimiUI
{
    public partial class FormAkbiller : Form
    {
        AkbildbContext context = new AkbildbContext();
        public FormAkbiller()
        {
            InitializeComponent();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                // kontroller
                if (cmbBoxAkbilTipleri.SelectedIndex < 0)
                {
                    MessageBox.Show("Akbil türü seçiniz");
                    return;
                }
                if (maskedTextBoxAkbilNo.Text.Length < 16)
                {
                    MessageBox.Show("Akbil No 16 haneli olmak zorunda");
                    return;
                }
                Akbiller yeniAkbil = new Akbiller()
                {
                    EklenmeTarihi = DateTime.Now,
                    AkbilNo = maskedTextBoxAkbilNo.Text,
                    AkbilinSahibiId = GenelIslemler.GirisYapanKullaniciID,
                    AkbilTipi = cmbBoxAkbilTipleri.SelectedItem.ToString(),
                    Bakiye = 0,
                    VizelendigiTarih = null
                };
                context.Akbillers.Add(yeniAkbil);
                int sonuc = context.SaveChanges();
                if (sonuc > 0)
                {
                    MessageBox.Show("Yeni akbil eklendi");
                    cmbBoxAkbilTipleri.Text = "Akbil tipi seçiniz...";
                    cmbBoxAkbilTipleri.SelectedIndex = -1;
                    DataGridViewDoldur();
                }
                else
                {
                    MessageBox.Show("Akbil eklenemedi");
                }


            }
            catch (Exception hata)
            {
                MessageBox.Show("Beklenmedik bir hata oluştu" + hata.Message);
            }
        }

        private void FormAkbiller_Load(object sender, EventArgs e)
        {
            cmbBoxAkbilTipleri.Text = "Akbil tipi seçiniz...";
            cmbBoxAkbilTipleri.SelectedIndex = -1;
            DataGridViewDoldur();
        }

        private void DataGridViewDoldur()
        {
            try
            {

                dataGridViewAkbiller.DataSource = context.Akbillers.Where(x => x.AkbilinSahibiId == GenelIslemler.GirisYapanKullaniciID).ToList();
                dataGridViewAkbiller.Columns["AkbilinSahibiId"].Visible = false;
                dataGridViewAkbiller.Columns["VizelendigiTarih"].HeaderText = "Vizelendiği Tarih";

            }
            catch (Exception hata)
            {

                MessageBox.Show("Akbiller listelenemedi" + hata.Message);
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void cikisYapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Güle Güle Çıkış Yapıldı");
            GenelIslemler.GirisYapanKullaniciAdSoyad = string.Empty;
            GenelIslemler.GirisYapanKullaniciID = 0;

            foreach (Form item in Application.OpenForms)
            {

                if (item.Name != "Form1") ;
                {
                    item.Hide();
                }
            }
            Application.OpenForms["Form1"].Show();
        }

        private void anasayfaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAnasayfa formA = new FormAnasayfa();
            this.Hide();
            formA.Show();
        }
    }
}
