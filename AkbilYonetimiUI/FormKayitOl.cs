﻿
using System.Data.SqlClient;

namespace AkbilYonetimiUI
{
    public partial class FormKayitOl : Form
    {
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
                foreach (var item in Controls)
                {
                    if (item is TextBox && string.IsNullOrEmpty(((TextBox)item).Text))
                    {
                        MessageBox.Show("Zorunlu alanları doldurunuz");
                        return;
                    }
                }
                Dictionary<string, object> kolonlar = new Dictionary<string, object>();
                kolonlar.Add("Ad", $"'{txtAd.Text.Trim()}'");
                kolonlar.Add("Soyad", $"'{txtSoyad.Text.Trim()}'");
                kolonlar.Add("Email", $"'{txtEmail.Text.Trim()}'");
                kolonlar.Add("DogumTarihi", $"'{dtpDogumTarihi.Value.ToString("yyyMMdd")}'");
                kolonlar.Add("EklenmeTarihi", $"'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'");
              //  kolonlar.Add("Parola", $"'{GenelIslemler.MD5Encryption(txtSifre.Text.Trim())}'");

              
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Beklenmedik bir hata oluştu.Tekrar deneyiniz");
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
