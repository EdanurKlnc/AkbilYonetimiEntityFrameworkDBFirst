using AkbilYonetimiVeriKatmani;
using AkbilYonetimiIsKatmani;
using System.Data.SqlClient;
using AkbilYonetimiVeriKatmani.Models;

namespace AkbilYonetimiUI
{

    public partial class FormKayitOl : Form
    {
        AkbildbContext context = new AkbildbContext();
        public FormKayitOl()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void FormKayitOl_Load(object sender, EventArgs e)
        {
            txtSifre.PasswordChar = '*';
            dtpDogumTarihi.MaxDate = new DateTime(2016, 1, 1); //min girilen yaş
            dtpDogumTarihi.Value = new DateTime(2016, 1, 1); //ilk görüntü
            dtpDogumTarihi.Format = DateTimePickerFormat.Short;

        }

        private void btnKayit_Click(object sender, EventArgs e)
        {
            try
            {
                btnKayit.Enabled = false;
                btnGiris.Enabled = false;
                foreach (var item in Controls)
                {
                    if (item is TextBox && string.IsNullOrEmpty(((TextBox)item).Text))
                    {
                        MessageBox.Show("Zorunlu alanları doldurunuz");
                        btnKayit.Enabled = true;
                        btnGiris.Enabled = true;
                        return;
                    }
                }
                if (context.Kullanicilars.FirstOrDefault(x => x.Email == txtEmail.Text.Trim()) != null)
                {
                    MessageBox.Show("Bu emial sistemde mevcut");
                    btnKayit.Enabled = true;
                    btnGiris.Enabled = true;
                    return;
                }
                Kullanicilar yeniKullanici = new Kullanicilar()
                {
                    EklenmeTarihi = DateTime.Now,
                    Ad = txtAd.Text.Trim(),
                    Soyad = txtSoyad.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Parola = GenelIslemler.MD5Encryption(txtSifre.Text.Trim())

                };
                context.Kullanicilars.Add(yeniKullanici);
                if (context.SaveChanges() > 0)
                {
                    MessageBox.Show("Kullanıcı eklendi.");
                    //temizlik
                    foreach (var item in Controls)
                    {
                        if (item is TextBox)
                        {
                            ((TextBox)item).Clear();
                        }
                        if (item is DateTimePicker)
                        {
                            ((DateTimePicker)item).Value = ((DateTimePicker)item).MaxDate;
                        }
                    }//foreach bitti

                    var cevap = MessageBox.Show("Giriş sayfasına gitmek ister misiniz?", "SORU", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (cevap == DialogResult.Yes) //((int)cevap==6)
                    {
                        Form1 frmg = new Form1();
                        frmg.Email = txtEmail.Text.Trim();
                        this.Hide();
                        frmg.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Kullanıcı eklenemedi");
                }
                btnGiris.Enabled = true;
                btnKayit.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Beklenmedik bir hata oluştu.Tekrar deneyiniz");
                btnKayit.Enabled = true;
                btnGiris.Enabled = true;
            }
        }

        private void GirisFormunaGit()
        {
            Form1 formG = new Form1();
            formG.Email = txtEmail.Text.Trim();
            this.Hide();
            formG.Show();
        }

        private void dtpDogumTarihi_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            GirisFormunaGit();
        }

        private void FormKayitOl_FormClosed(object sender, FormClosedEventArgs e)
        {
            GirisFormunaGit();
        }
    }
}
