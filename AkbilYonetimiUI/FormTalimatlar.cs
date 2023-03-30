
using System.Collections;
using AkbilYonetimiIsKatmani;
using AkbilYonetimiVeriKatmani;
using AkbilYonetimiVeriKatmani.Models;

namespace AkbilYonetimiUI
{
    public partial class FormTalimatlar : Form
    {

        AkbildbContext context = new AkbildbContext();
        public FormTalimatlar()
        {
            InitializeComponent();
        }
        private void FormTalimatlar_Load(object sender, EventArgs e)
        {
            //Comboxa akbilleri getir
            ComboBoxaKullanicininAkbilleriniGetir();
            cmbBoxAkbiller.SelectedIndex = -1;
            cmbBoxAkbiller.Text = "Akbil seçiniz...";
            // cmbBoxAkbiller.DropDownStyle = ComboBoxStyle.DropDownList;
            groupBoxYukleme.Enabled = false;

            dataGridViewTalimatlar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            TalimatlariDataGrideGetir();
            dataGridViewTalimatlar.ContextMenuStrip = contextMenuStrip1;

            checkBoxTumunuGoster.Checked = false;
            BekleyenTalimatSayisiniGetir();
            timerBekleyenTalimat.Interval = 1000;
            timerBekleyenTalimat.Enabled = true;
        }

        private void BekleyenTalimatSayisiniGetir()
        {
            try
            {
                //burada bekleyen talimatları bulur
                var bekleyen = context.KullanicininTalimatlaris.Where(x => x.KullaniciId == GenelIslemler.GirisYapanKullaniciID && !x.YuklendiMi);

                if (cmbBoxAkbiller.SelectedIndex >= 0)
                {
                    //burada ise bekleyen talimatlar içinden sadece combo seçili olanın sayısını alıyoruz
                    lblBekleyenTalimat.Text = bekleyen.Count(x => x.Akbil.Substring(0, 16) == cmbBoxAkbiller.SelectedValue.ToString()).ToString();
                }
                else
                {
                    //bekleyen talimatı yukarıda almıştık. Aldığımız sonucu count ile saydık
                    lblBekleyenTalimat.Text = bekleyen.Count().ToString();

                }


            }
            catch (Exception hata)
            {

                MessageBox.Show("Beklenmedik bir hata oluştu" + hata.Message);

            }
        }

        private void TalimatlariDataGrideGetir(bool tumunuGoster = false)
        {
            try
            {
                var talimatlar = context.KullanicininTalimatlaris.Where(x => x.KullaniciId == GenelIslemler.GirisYapanKullaniciID);

                if (!tumunuGoster)
                {
                    talimatlar = talimatlar.Where(x => !x.YuklendiMi);

                }
                if (cmbBoxAkbiller.SelectedIndex >= 0)
                {
                    talimatlar = talimatlar.Where(x => x.Akbil.Substring(0, 16) == cmbBoxAkbiller.SelectedValue.ToString());
                }
                dataGridViewTalimatlar.DataSource = talimatlar.ToList();

                dataGridViewTalimatlar.Columns["Id"].Visible = false;
                dataGridViewTalimatlar.Columns["Akbil"].Width = 200;
                dataGridViewTalimatlar.Columns["YuklendiMi"].Width = 150;
                dataGridViewTalimatlar.Columns["YuklendiMi"].HeaderText = "Talimat Yüklendi mi ?";
            }
            catch (Exception hata)
            {
                MessageBox.Show("Talimatlar getirilirken hata oluştu! " + hata.Message);
            }
        }

        private void ComboBoxaKullanicininAkbilleriniGetir()
        {
            try
            {
                cmbBoxAkbiller.DataSource = context.Akbillers.Where(x => x.AkbilinSahibiId == GenelIslemler.GirisYapanKullaniciID).ToList();
                cmbBoxAkbiller.DisplayMember = "AkbilNo";
                cmbBoxAkbiller.ValueMember = "AkbilNo";

            }
            catch (Exception hata)
            {
                MessageBox.Show("Beklenmedik bir hata oluştu! " + hata.Message);
            }

        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {

            try
            {
                if (cmbBoxAkbiller.SelectedIndex < 0)
                {
                    MessageBox.Show("Akbil seçmeden talimat kaydedilemez! ");
                    return;
                }
                if (string.IsNullOrEmpty(txtYuklenecekTutar.Text))
                {
                    MessageBox.Show("Yükleme miktarı girişi zorunludur! ");
                    return;
                }
                if (!decimal.TryParse(txtYuklenecekTutar.Text.Trim(), out decimal tutar))
                {
                    MessageBox.Show("Yükleme miktarı girişi uygun formatta olmalıdır! ");
                    return;
                }
                Talimatlar yeniTalimat = new Talimatlar()
                {
                    EklenmeTarihi = DateTime.Now,
                    AkbilId = cmbBoxAkbiller.SelectedValue.ToString(),
                    YuklendiMi = false,
                    YuklenecekTutar = Convert.ToDecimal(txtYuklenecekTutar.Text),
                    YuklenmeTarihi = null
                };
                context.Talimatlars.Add(yeniTalimat);
                if (context.SaveChanges() > 0)
                {
                    MessageBox.Show("Yeni talimat eklendi");
                    txtYuklenecekTutar.Clear();
                    cmbBoxAkbiller.SelectedIndex = -1;
                    cmbBoxAkbiller.Text = "Akbil Seçiniz...";
                    groupBoxYukleme.Enabled = false;
                    TalimatlariDataGrideGetir(checkBoxTumunuGoster.Checked);
                    BekleyenTalimatSayisiniGetir();
                }
                else
                {
                    MessageBox.Show("Yeni talimat eklenemedi");

                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Talimat kaydedilemedi! " + hata.Message);
            }
        }
        private void checkBoxTumunuGoster_CheckedChanged(object sender, EventArgs e)
        {
            TalimatlariDataGrideGetir(checkBoxTumunuGoster.Checked);

        }

        private void cmbBoxAkbiller_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbBoxAkbiller.SelectedIndex >= 0)
            {
                txtYuklenecekTutar.Clear();
                groupBoxYukleme.Enabled = true;
               
            }
            else
            {
                txtYuklenecekTutar.Clear();
                groupBoxYukleme.Enabled = false;
                TalimatlariDataGrideGetir(checkBoxTumunuGoster.Checked);

            }
            BekleyenTalimatSayisiniGetir();

        }

        private void anaMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAnasayfa formaA = new FormAnasayfa();
            this.Hide();
            formaA.Show();
        }

        private void cikisYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Güle güle çıkış yapıldı");
            // GenelIslemler.GirisYapanKullaniciAdSoyad = string.Empty;
            // GenelIslemler.GirisYapanKullaniciID = 0;

            foreach (Form item in Application.OpenForms)
            {
                if (item.Name != "Form1")
                {
                    item.Hide();
                }
            }
            Application.OpenForms["Form1"].Show();
        }

        private void lblBekleyenTalimat_Click(object sender, EventArgs e)
        {

        }

        private void timerBekleyenTalimat_Tick(object sender, EventArgs e)
        {
            if (lblBekleyenTalimat.Text != "0")
            {
                if (DateTime.Now.Second % 2 == 0)
                {
                    lblBekleyenTalimat.Font = new Font("Segoe UI Black", 40);
                    lblBekleyenTalimat.ForeColor = Color.Red;
                }
                else
                {
                    lblBekleyenTalimat.Font = new Font("Segoe UI Black", 25);
                    lblBekleyenTalimat.ForeColor = Color.Black;
                }
            }
        }

        private void talimatıiptalEtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int sayac = 0;
            foreach (DataGridViewRow item in dataGridViewTalimatlar.SelectedRows)
            {
                if ((bool)item.Cells["YuklendiMi"].Value)
                {
                    MessageBox.Show($"Dikkat {item.Cells["Akbil"].Value} {item.Cells["YuklenecekTutar"].Value} liralık yüklemesi yapılmıştır. Yüklenen talimat iptal edilemez/silinemez. \nİşlemlerinize devam etmek için tamama basınız.");
                    continue;
                }
                var secilenTalimat = context.Talimatlars.FirstOrDefault(x => x.Id == (int)item.Cells["Id"].Value);
                if (secilenTalimat != null)
                {
                    context.Talimatlars.Remove(secilenTalimat); //silme sorgusu
                    sayac += context.SaveChanges();
                }

            }
            MessageBox.Show($"{sayac} adet talimat iptal edilmiştir");
            TalimatlariDataGrideGetir();
            BekleyenTalimatSayisiniGetir();
        }
        private void talimatiYukleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int sayac = 0;
                foreach (DataGridViewRow item in dataGridViewTalimatlar.SelectedRows)
                {
                    if ((bool)item.Cells["YuklendiMi"].Value)
                    {
                        continue;
                    }
                    //önce talimatı yüklendi mi = 1 yapıcaz
                    var secilenTalimat = context.Talimatlars.FirstOrDefault(x => x.Id == (int)item.Cells["Id"].Value);
                    if (secilenTalimat != null)
                    {
                        secilenTalimat.YuklendiMi = true;
                        secilenTalimat.YuklenmeTarihi = DateTime.Now;
                        context.Talimatlars.Update(secilenTalimat);
                        sayac += context.SaveChanges();
                    }

                    //sonra akbilin bakiyesine ekleme yapıcaz
                    secilenTalimat.Akbil.Bakiye += Convert.ToDecimal(item.Cells["YuklenecekTutar"].Value);
                    context.Akbillers.Update(secilenTalimat.Akbil);
                    context.SaveChanges();
                }
                MessageBox.Show($"{sayac} adet talimat akbile yüklendi");
                TalimatlariDataGrideGetir();
                BekleyenTalimatSayisiniGetir();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Hata" + hata.Message);
            }
        }
    }
}
