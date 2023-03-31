using AkbilYonetimiIsKatmani;
using AkbilYonetimiVeriKatmani;
using AkbilYonetimiVeriKatmani.Models;
using System.Data.SqlClient;

namespace AkbilYonetimiUI
{
    public partial class FormAyarlar : Form
    {
        AkbildbContext contex = new AkbildbContext();
        public FormAyarlar()
        {
            InitializeComponent();
        }

        private void FormAyarlar_Load(object sender, EventArgs e)
        {
            txtYeniSifre.PasswordChar = '*';
            dtpDogumTarihi.MaxDate = new DateTime(2016, 1, 1); //min girilen yaş
            dtpDogumTarihi.Value = new DateTime(2016, 1, 1); //ilk görüntü
            dtpDogumTarihi.Format = DateTimePickerFormat.Short;

            KullanicininBilgileriniGetir();
        }

        private void KullanicininBilgileriniGetir()
        {
            try
            {
                var kullanici = contex.Kullanicilars.FirstOrDefault(x => x.Id == GenelIslemler.GirisYapanKullaniciID);
                if (kullanici != null)
                {
                    txtAd.Text = kullanici.Ad;
                    txtSoyad.Text = kullanici.Soyad;
                    txtEmail.Text = kullanici.Email;
                    txtEmail.Enabled = false;
                    dtpDogumTarihi.Value = kullanici.DogumTarihi;
                }
                else
                {
                    MessageBox.Show("Kullanıcı bilgileri getilemedi.");
                }
            }
            catch (Exception hata)
            {

                MessageBox.Show("Beklenmedik bir hata oluştu" + hata.Message);
            }
        }

        private void btnGucelle_Click(object sender, EventArgs e)
        {
            try
            {
                var kullanici = contex.Kullanicilars.FirstOrDefault(x => x.Id == GenelIslemler.GirisYapanKullaniciID);
                if (kullanici != null) //kullanıcının girdiği önceki ile aynıysa güncellemeyi engelleme
                {
                    kullanici.Ad = txtAd.Text.Trim();
                    kullanici.Soyad = txtSoyad.Text.Trim();
                    kullanici.DogumTarihi = dtpDogumTarihi.Value;

                    if (!string.IsNullOrEmpty(txtYeniSifre.Text.Trim()))
                    {
                        if (!string.IsNullOrEmpty(txtYeniSifre.Text.Trim()) && kullanici.Parola != GenelIslemler.MD5Encryption(txtYeniSifre.Text.Trim() ))
                        {
                            kullanici.Parola = GenelIslemler.MD5Encryption(txtYeniSifre.Text.Trim());
                            MessageBox.Show("Yeni şifre girdiniz");
                        }
                    }
                    contex.Kullanicilars.Update(kullanici);
                    if (contex.SaveChanges() > 0)
                    {
                        MessageBox.Show("Bilgileriniz güncellendi");
                        FormAnasayfa formA = new FormAnasayfa();
                        this.Hide();
                        formA.Show();

                    }
                }
            }
            catch (Exception )
            {
                MessageBox.Show("Hata" );
            }
        }
    }
}
