
using System.Data.SqlClient;
using System.Text;
using AkbilYonetimiVeriKatmani;
using AkbilYonetimiIsKatmani;
using AkbilYonetimiVeriKatmani.Models;

namespace AkbilYonetimiUI
{
    public partial class Form1 : Form
    {
        public string Email { get; set; } //kaýt ol formuna týklandýðýnda kayýt olan kullanýcýnýn emaili buraya gelsin

        AkbildbContext context = new AkbildbContext();
        public Form1()
        {
            InitializeComponent();
        }


        private void btnKayit_Click(object sender, EventArgs e)
        {
            //Bu formu gizleyeceðiz
            //Kayýt ol formunu acacaðýz
            this.Hide();
            FormKayitOl frm = new FormKayitOl();
            frm.Show();

        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            GirisYap();
        }
        private void GirisYap()
        {
                try
                {
                    //1)Email ve sifre textboxlarý dolu mu?
                    if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtSifre.Text))
                    {
                        MessageBox.Show("Bilgilerinizi eksiksiz giriniz", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    //2)Girdiði email ve þifre veritabanýnda mevcut mu?
                    var kullanici = context.Kullanicilars.FirstOrDefault(x => x.Email == txtEmail.Text && x.Parola == GenelIslemler.MD5Encryption(txtSifre.Text));

                    if (kullanici == null)
                    {
                        MessageBox.Show("Email ya da þifrenizi yanlýþ girdiniz ! ");
                        return;
                    }
                    else
                    {
                        MessageBox.Show($"Hoþgeldiniz...{kullanici.Ad} {kullanici.Soyad}");
                        GenelIslemler.GirisYapanKullaniciID = kullanici.Id;
                        GenelIslemler.GirisYapanKullaniciEmail = kullanici.Email;
                        GenelIslemler.GirisYapanKullaniciAdSoyad = $"{kullanici.Ad} {kullanici.Soyad}";
                        //Beni Hatirlayi settings ile yazalim.

                        //temizlik
                        txtEmail.Clear(); txtSifre.Clear();
                        FormAnasayfa frmAnasayfa = new FormAnasayfa();
                        this.Hide();
                        frmAnasayfa.Show();

                    }
                }
                catch (Exception hata)
                {
                    //Dipnot: Exceptionlar asla kullanýcýya gösterilemez.Exceptionlar loglanýr, yazýlýmcýya iletilir.Biz öðrenmek için mbox ýn içine yazdýk.

                    MessageBox.Show("Beklenmedik bir sorun oluþtu!" + hata.Message);
                }
            }
        

        private void checkBoxHatirla_CheckedChanged(object sender, EventArgs e)
        {
            BeniHatirla();
        }

        private void BeniHatirla()
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Email != null)

            {
                txtEmail.Text = Email;
            }
            txtEmail.TabIndex = 1;
            txtSifre.TabIndex = 2;
            checkBoxHatirla.TabIndex = 3;
            btnGiris.TabIndex = 4;
            btnKayit.TabIndex = 5;

        }

        private void txtSifre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter)) //basýlan tuþ enter ise giriþ yap
            {
                GirisYap();
            }
        }
    }
}