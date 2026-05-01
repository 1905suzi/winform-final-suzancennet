using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace PersonelYonetimSistemi
{
    public partial class frmPersonel : Form
    {
        private int secilenPersonelID = 0;

        // Nesne Tanımlamaları
        private DataGridView dgv;
        private TextBox txtAd, txtSoyad, txtTC, txtEmail, txtTelefon;
        private ComboBox cmbDepartman, cmbPozisyon, cmbDurum;
        private DateTimePicker dtpIseGiris;
        private Label lblKidem;
        private Button btnKaydet, btnGuncelle, btnSil, btnTemizle;
        private TextBox txtAra;

        // --- YENİ TOZ PEMBESİ TEMASI 🌸 ---
        private static readonly Color C_TOZ_PEMBE = Color.FromArgb(230, 160, 180);
        private static readonly Color C_KREM_ARKA = Color.FromArgb(250, 245, 240);
        private static readonly Color C_KOYU_GRI = Color.FromArgb(60, 60, 60);

        public frmPersonel()
        {
            this.Text = "İlmek Tekstil - Personel Yönetim Paneli";
            this.BackColor = C_KREM_ARKA;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(1100, 750);
            this.Font = new Font("Segoe UI Semibold", 10f);

            this.Load += FrmPersonel_Load;
            FormOlustur();
        }

        private void FormOlustur()
        {
            // 1. ÜST BAŞLIK (Sığma sorunu için yüksekliği 70 yaptık)
            var pnlBaslik = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = C_TOZ_PEMBE };
            var lblBaslik = new Label
            {
                Text = "İLMEK TEKSTİL PERSONEL YÖNETİM SİSTEMİ",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlBaslik.Controls.Add(lblBaslik);
            this.Controls.Add(pnlBaslik);

            // 2. ANA TAŞIYICI
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 420,
                BackColor = Color.Transparent
            };
            this.Controls.Add(split);

            // 3. SOL PANEL: GİRİŞ FORMU
            var grp = new GroupBox
            {
                Text = " Personel Kartı Bilgileri ",
                ForeColor = C_KOYU_GRI,
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            split.Panel1.Controls.Add(grp);

            var tbl = new TableLayoutPanel { Dock = DockStyle.Top, Height = 460, ColumnCount = 2, Padding = new Padding(5) };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            grp.Controls.Add(tbl);

            Action<string, Control> satirEkle = (etiket, ctrl) =>
            {
                var lbl = new Label { Text = etiket, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.FromArgb(80, 80, 80) };
                ctrl.Dock = DockStyle.Fill;
                tbl.Controls.Add(lbl); tbl.Controls.Add(ctrl);
            };

            // ALANLAR (AD KISMI EN BAŞTA)
            txtAd = YeniTextBox(); satirEkle("Adı :", txtAd);
            txtSoyad = YeniTextBox(); satirEkle("Soyadı :", txtSoyad);
            txtTC = YeniTextBox(); satirEkle("TC Kimlik :", txtTC);
            txtEmail = YeniTextBox(); satirEkle("E-posta :", txtEmail);
            txtTelefon = YeniTextBox(); satirEkle("Telefon :", txtTelefon);
            cmbDepartman = YeniCombo(new[] { "Üretim", "Lojistik", "Muhasebe", "İnsan Kaynakları", "Yönetim" });
            satirEkle("Departman :", cmbDepartman);
            cmbPozisyon = YeniCombo(new[] { "Makinacı", "Ütücü", "Kalite Kontrol", "Depo Sorumlusu", "Şoför", "Muhasebe Elemanı", "İK Uzmanı", "İK Yöneticisi" });
            satirEkle("Pozisyon :", cmbPozisyon);
            cmbDurum = YeniCombo(new[] { "Aktif", "Pasif" });
            satirEkle("Durum :", cmbDurum);
            dtpIseGiris = new DateTimePicker { Format = DateTimePickerFormat.Short, Font = new Font("Segoe UI", 10f) };
            satirEkle("İşe Giriş :", dtpIseGiris);

            lblKidem = new Label { Text = "Personel seçin...", Font = new Font("Segoe UI", 9f, FontStyle.Italic), ForeColor = Color.DimGray, BackColor = Color.FromArgb(250, 240, 245), TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Top, Height = 35 };
            grp.Controls.Add(lblKidem);

            // BUTONLAR
            var pnlButonlar = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 60, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(5) };
            btnKaydet = YeniButon("Kaydet", Color.FromArgb(70, 150, 70));
            btnGuncelle = YeniButon("Güncelle", C_KOYU_GRI);
            btnSil = YeniButon("Sil", Color.FromArgb(180, 50, 50));
            btnTemizle = YeniButon("Temizle", Color.DarkGray);
            btnKaydet.Click += BtnKaydet_Click; btnGuncelle.Click += BtnGuncelle_Click; btnSil.Click += BtnSil_Click; btnTemizle.Click += (s, e) => Temizle();
            pnlButonlar.Controls.AddRange(new Control[] { btnKaydet, btnGuncelle, btnSil, btnTemizle });
            grp.Controls.Add(pnlButonlar);

            // 4. SAĞ PANEL: LİSTE
            var pnlSag = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            split.Panel2.Controls.Add(pnlSag);

            txtAra = new TextBox { Dock = DockStyle.Top, Height = 35, Font = new Font("Segoe UI", 11f), BorderStyle = BorderStyle.FixedSingle };
            txtAra.TextChanged += (s, e) => PersonelleriYukle(txtAra.Text.Trim());
            pnlSag.Controls.Add(new Label { Text = "Hızlı Arama (Ad/Departman):", Dock = DockStyle.Top, Height = 25 });
            pnlSag.Controls.Add(txtAra);

            // --- DATA GRID VIEW (STİL HATALARI DÜZELTİLDİ) ---
            dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.EnableHeadersVisualStyles = false; // Başlık renkleri için bu ŞART
            dgv.GridColor = Color.FromArgb(235, 225, 230);

            // Stil Ayarları (Hata vermemesi için doğru özellik isimleri kullanıldı)
            dgv.ColumnHeadersDefaultCellStyle.BackColor = C_TOZ_PEMBE;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;

            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 210, 220);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);

            dgv.CellClick += Dgv_CellClick;
            pnlSag.Controls.Add(dgv);
        }

        private TextBox YeniTextBox() => new TextBox { Font = new Font("Segoe UI", 11f), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.FromArgb(254, 252, 253) };
        private ComboBox YeniCombo(string[] items) { var c = new ComboBox { Font = new Font("Segoe UI", 11f), DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat }; c.Items.AddRange(items); return c; }
        private Button YeniButon(string text, Color renk) => new Button { Text = text, BackColor = renk, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Size = new Size(92, 44), Cursor = Cursors.Hand };

        private void FrmPersonel_Load(object sender, EventArgs e) => PersonelleriYukle(null);

        private void PersonelleriYukle(string ara)
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT Id, Ad, Soyad, TcKimlik, Departman, Pozisyon, IseGirisTarihi, Durum FROM Personeller WHERE 1=1";
                    if (!string.IsNullOrWhiteSpace(ara)) sql += " AND (Ad LIKE @a OR Soyad LIKE @a OR Departman LIKE @a)";

                    var da = new SQLiteDataAdapter(sql, conn);
                    da.SelectCommand.Parameters.AddWithValue("@a", "%" + ara + "%");
                    DataTable dt = new DataTable(); da.Fill(dt); dgv.DataSource = dt;

                    // Başlıkları Burada Ayarlıyoruz (Hata almamak için Sütun Varlığını Kontrol Ederek)
                    if (dgv.Columns.Contains("TcKimlik")) dgv.Columns["TcKimlik"].HeaderText = "TC Kimlik";
                    if (dgv.Columns.Contains("IseGirisTarihi")) dgv.Columns["IseGirisTarihi"].HeaderText = "Giriş Tarihi";
                    if (dgv.Columns.Contains("Id")) dgv.Columns["Id"].Visible = false;
                }
            }
            catch (Exception ex) { MessageBox.Show("Veri Yükleme Hatası: " + ex.Message); }
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgv.Rows[e.RowIndex];
            try
            {
                secilenPersonelID = Convert.ToInt32(row.Cells["Id"].Value);
                txtAd.Text = row.Cells["Ad"].Value?.ToString();
                txtSoyad.Text = row.Cells["Soyad"].Value?.ToString();
                txtTC.Text = row.Cells["TcKimlik"].Value?.ToString();
                cmbDepartman.Text = row.Cells["Departman"].Value?.ToString();
                cmbPozisyon.Text = row.Cells["Pozisyon"].Value?.ToString();
                cmbDurum.Text = row.Cells["Durum"].Value?.ToString();

                if (DateTime.TryParse(row.Cells["IseGirisTarihi"].Value?.ToString(), out DateTime tarih))
                {
                    dtpIseGiris.Value = tarih;
                    lblKidem.Text = $"Seçili: {txtAd.Text} {txtSoyad.Text} | Giriş: {tarih.ToShortDateString()}";
                }
            }
            catch { }
        }

        // --- BUTON İŞLEMLERİ (KAYDET, GÜNCELLE, SİL) ---
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text)) return;
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    var cmd = new SQLiteCommand("INSERT INTO Personeller(Ad,Soyad,TcKimlik,Departman,Pozisyon,IseGirisTarihi,Durum,GenelAmir) VALUES(@ad,@soyad,@tc,@dep,@poz,@ise,@dur,'Suzan Özer')", conn);
                    cmd.Parameters.AddWithValue("@ad", txtAd.Text); cmd.Parameters.AddWithValue("@soyad", txtSoyad.Text);
                    cmd.Parameters.AddWithValue("@tc", txtTC.Text); cmd.Parameters.AddWithValue("@dep", cmbDepartman.Text);
                    cmd.Parameters.AddWithValue("@poz", cmbPozisyon.Text); cmd.Parameters.AddWithValue("@ise", dtpIseGiris.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@dur", cmbDurum.Text); cmd.ExecuteNonQuery();
                }
                PersonelleriYukle(null); Temizle();
            }
            catch (Exception ex) { MessageBox.Show("Kayıt Hatası: " + ex.Message); }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (secilenPersonelID == 0) return;
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    var cmd = new SQLiteCommand("UPDATE Personeller SET Ad=@ad, Soyad=@soyad, TcKimlik=@tc, Departman=@dep, Pozisyon=@poz, IseGirisTarihi=@ise, Durum=@dur WHERE Id=@id", conn);
                    cmd.Parameters.AddWithValue("@ad", txtAd.Text); cmd.Parameters.AddWithValue("@id", secilenPersonelID);
                    cmd.Parameters.AddWithValue("@soyad", txtSoyad.Text); cmd.Parameters.AddWithValue("@tc", txtTC.Text);
                    cmd.Parameters.AddWithValue("@dep", cmbDepartman.Text); cmd.Parameters.AddWithValue("@poz", cmbPozisyon.Text);
                    cmd.Parameters.AddWithValue("@ise", dtpIseGiris.Value.ToString("yyyy-MM-dd")); cmd.Parameters.AddWithValue("@dur", cmbDurum.Text);
                    cmd.ExecuteNonQuery();
                }
                PersonelleriYukle(null);
            }
            catch (Exception ex) { MessageBox.Show("Güncelleme Hatası: " + ex.Message); }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (secilenPersonelID == 0) return;
            if (MessageBox.Show("Siliyoruz, emin misin?", "İşlem Onayı", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    var cmd = new SQLiteCommand("DELETE FROM Personeller WHERE Id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", secilenPersonelID); cmd.ExecuteNonQuery();
                }
                PersonelleriYukle(null); Temizle();
            }
        }

        private void Temizle()
        {
            secilenPersonelID = 0; txtAd.Clear(); txtSoyad.Clear(); txtTC.Clear(); txtEmail.Clear(); txtTelefon.Clear();
            cmbDepartman.SelectedIndex = -1; cmbPozisyon.SelectedIndex = -1;
        }
    }
}