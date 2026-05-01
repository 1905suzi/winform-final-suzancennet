using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace PersonelYonetimSistemi
{
    public partial class frmIzin : Form
    {
        private ComboBox cmbPersonel, cmbIzinTipi;
        private DateTimePicker dtpBaslangic, dtpBitis;
        private Label lblIsGunu, lblBakiye, lblBildirim;
        private TextBox txtAciklama;
        private Button btnTalepGonder, btnIptal;
        private DataGridView dgv;

        private static readonly Color C_BASLIK = Color.FromArgb(92, 52, 10);
        private static readonly Color C_ARKA = Color.FromArgb(255, 248, 235);

        public frmIzin()
        {
            this.Text = "Izin Yonetimi";
            this.BackColor = C_ARKA;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 640);
            this.Size = new Size(1100, 720);
            this.Font = new Font("Segoe UI", 9f);
            this.Load += FrmIzin_Load;
            FormOlustur();
        }

        private void FormOlustur()
        {
            var pnlBaslik = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = C_BASLIK };
            pnlBaslik.Controls.Add(new Label
            {
                Text = "IZIN YONETIMI",
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            });
            this.Controls.Add(pnlBaslik);

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                BackColor = C_ARKA
            };
            this.Controls.Add(split);
            split.SplitterDistance = 340;
            split.Panel1MinSize = 300;
            split.Panel2MinSize = 400;

            var grp = new GroupBox
            {
                Text = "Yeni Izin Talebi",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = C_BASLIK,
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            split.Panel1.Controls.Add(grp);

            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Padding = new Padding(4)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            grp.Controls.Add(tbl);

            Action<string, Control> satirEkle = (lbl, ctrl) =>
            {
                var l = new Label
                {
                    Text = lbl,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight,
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = Color.FromArgb(60, 60, 60)
                };
                ctrl.Dock = DockStyle.Fill;
                tbl.Controls.Add(l);
                tbl.Controls.Add(ctrl);
            };

            cmbPersonel = new ComboBox
            {
                Font = new Font("Segoe UI", 10f),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            cmbPersonel.SelectedIndexChanged += CmbPersonel_Changed;
            satirEkle("Personel :", cmbPersonel);

            cmbIzinTipi = new ComboBox
            {
                Font = new Font("Segoe UI", 10f),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            cmbIzinTipi.Items.AddRange(new[] { "Yillik Izin", "Ucretsiz Izin" });
            cmbIzinTipi.SelectedIndex = 0;
            satirEkle("Izin Tipi :", cmbIzinTipi);

            dtpBaslangic = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today,
                Font = new Font("Segoe UI", 10f)
            };
            dtpBaslangic.ValueChanged += (s, e) => IsGunuGuncelle();
            satirEkle("Baslangic :", dtpBaslangic);

            dtpBitis = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddDays(4),
                Font = new Font("Segoe UI", 10f)
            };
            dtpBitis.ValueChanged += (s, e) => IsGunuGuncelle();
            satirEkle("Bitis :", dtpBitis);

            lblIsGunu = new Label
            {
                Text = "Tarih seciniz...",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(92, 52, 10),
                BackColor = Color.FromArgb(255, 236, 179),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Height = 30
            };
            tbl.SetColumnSpan(lblIsGunu, 2);
            tbl.Controls.Add(lblIsGunu);
            tbl.Controls.Add(new Label());

            lblBakiye = new Label
            {
                Text = "Personel seciniz...",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(27, 94, 32),
                BackColor = Color.FromArgb(200, 230, 201),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Height = 30
            };
            tbl.SetColumnSpan(lblBakiye, 2);
            tbl.Controls.Add(lblBakiye);
            tbl.Controls.Add(new Label());

            var lblAcik = new Label
            {
                Text = "Aciklama :",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopRight,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(60, 60, 60),
                Height = 60
            };
            txtAciklama = new TextBox
            {
                Multiline = true,
                Height = 60,
                Font = new Font("Segoe UI", 9f),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 252, 250),
                Dock = DockStyle.Fill
            };
            tbl.Controls.Add(lblAcik);
            tbl.Controls.Add(txtAciklama);

            lblBildirim = new Label
            {
                Text = "",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                Height = 30,
                Visible = false
            };
            tbl.SetColumnSpan(lblBildirim, 2);
            tbl.Controls.Add(lblBildirim);
            tbl.Controls.Add(new Label());

            var pnlBtns = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 48,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(4),
                BackColor = Color.White
            };
            btnTalepGonder = new Button
            {
                Text = "Talep Gonder",
                Size = new Size(140, 36),
                BackColor = Color.FromArgb(230, 81, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnTalepGonder.FlatAppearance.BorderSize = 0;
            btnTalepGonder.Click += BtnTalepGonder_Click;

            btnIptal = new Button
            {
                Text = "Temizle",
                Size = new Size(90, 36),
                BackColor = Color.FromArgb(97, 97, 97),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnIptal.FlatAppearance.BorderSize = 0;
            btnIptal.Click += (s, e) => { txtAciklama.Text = ""; lblBildirim.Visible = false; };
            pnlBtns.Controls.AddRange(new Control[] { btnTalepGonder, btnIptal });
            grp.Controls.Add(pnlBtns);

            var pnlSag = new Panel { Dock = DockStyle.Fill, BackColor = C_ARKA };
            split.Panel2.Controls.Add(pnlSag);

            var lblTablo = new Label
            {
                Text = "Tum Izin Talepleri",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = C_BASLIK,
                Dock = DockStyle.Top,
                Height = 32,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(6, 0, 0, 0)
            };
            pnlSag.Controls.Add(lblTablo);

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9f),
                ColumnHeadersHeight = 34,
                RowTemplate = { Height = 28 },
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(255, 224, 178),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = C_BASLIK;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 224, 178);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            pnlSag.Controls.Add(dgv);
        }

        private void FrmIzin_Load(object sender, EventArgs e)
        {
            PersonelleriYukle();
            TalepleriYukle();
        }

        private void PersonelleriYukle()
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                var da = new SQLiteDataAdapter(
                    "SELECT Id AS PersonelID, Ad || ' ' || Soyad || ' (' || Departman || ')' AS AdSoyad FROM Personeller WHERE Durum='Aktif' ORDER BY Soyad", conn);
                var dt = new DataTable();
                da.Fill(dt);
                cmbPersonel.DataSource = dt;
                cmbPersonel.DisplayMember = "AdSoyad";
                cmbPersonel.ValueMember = "PersonelID";
                cmbPersonel.SelectedIndex = -1;
            }
        }

        private void TalepleriYukle()
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT 
                        t.TalepID,
                        p.Ad || ' ' || p.Soyad AS Personel,
                        p.Departman,
                        strftime('%d.%m.%Y', t.BaslangicTarihi) AS Baslangic,
                        strftime('%d.%m.%Y', t.BitisTarihi) AS Bitis,
                        t.IzinTipi, t.Durum,
                        strftime('%d.%m.%Y', t.TalepTarihi) AS TalepTarihi,
                        substr(IFNULL(t.Aciklama,''), 1, 60) AS Aciklama
                    FROM IzinTalebi t
                    JOIN Personeller p ON t.PersonelID=p.Id
                    ORDER BY t.TalepTarihi DESC";
                var da = new SQLiteDataAdapter(sql, conn);
                var dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    string durum = row.Cells["Durum"].Value?.ToString() ?? "";
                    switch (durum)
                    {
                        case "Onaylandi":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(200, 230, 201); break;
                        case "Reddedildi":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 210); break;
                        case "Bekliyor":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 236, 179); break;
                        case "AmirOnayladi":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(207, 226, 255); break;
                    }
                }
            }
        }

        private void CmbPersonel_Changed(object sender, EventArgs e)
        {
            if (cmbPersonel.SelectedValue != null)
            {
                int pid = 0;

                // Eğer SelectedValue doğrudan bir sayıya çevrilebiliyorsa
                if (int.TryParse(cmbPersonel.SelectedValue.ToString(), out pid))
                {
                    if (pid > 0) BakiyeGoster(pid);
                }
                // Eğer DataRowView nesnesi olarak geldiyse (Formun ilk açılış durumu)
                else if (cmbPersonel.SelectedItem is System.Data.DataRowView row)
                {
                    pid = Convert.ToInt32(row["PersonelID"]);
                    if (pid > 0) BakiyeGoster(pid);
                }
            }
        }

        private void BakiyeGoster(int pid)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SQLiteCommand(@"
                    SELECT ToplamHak, KullanilanGun, KalanGun FROM IzinBakiye 
                    WHERE PersonelID=@pid AND Yil=@yil", conn);
                cmd.Parameters.AddWithValue("@pid", pid);
                cmd.Parameters.AddWithValue("@yil", DateTime.Today.Year);
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        int kalan = Convert.ToInt32(r["KalanGun"]);
                        lblBakiye.Text = string.Format(
                            "Bakiye: {0} gun  |  Toplam: {1}  |  Kullanilan: {2}",
                            kalan, r["ToplamHak"], r["KullanilanGun"]);
                        lblBakiye.BackColor = kalan > 5
                            ? Color.FromArgb(200, 230, 201) : Color.FromArgb(255, 205, 210);
                        lblBakiye.ForeColor = kalan > 5
                            ? Color.FromArgb(27, 94, 32) : Color.FromArgb(183, 28, 28);
                    }
                    else
                        lblBakiye.Text = "Bu yil icin bakiye kaydi yok.";
                }
            }
        }

        private int IsGunuHesapla(DateTime bas, DateTime bit)
        {
            int gun = 0;
            DateTime cur = bas.Date;
            while (cur <= bit.Date)
            {
                if (cur.DayOfWeek != DayOfWeek.Saturday &&
                    cur.DayOfWeek != DayOfWeek.Sunday) gun++;
                cur = cur.AddDays(1);
            }
            return gun;
        }

        private void IsGunuGuncelle()
        {
            if (dtpBitis.Value.Date < dtpBaslangic.Value.Date)
            {
                lblIsGunu.Text = "HATA: Bitis tarihi baslangictan once olamaz!";
                lblIsGunu.BackColor = Color.FromArgb(255, 205, 210);
                return;
            }
            int gun = IsGunuHesapla(dtpBaslangic.Value, dtpBitis.Value);
            int takvim = (dtpBitis.Value.Date - dtpBaslangic.Value.Date).Days + 1;
            lblIsGunu.Text = string.Format("Is Gunu: {0}  |  Takvim: {1} gun", gun, takvim);
            lblIsGunu.BackColor = Color.FromArgb(255, 236, 179);
            if (cmbPersonel.SelectedValue != null && Convert.ToInt32(cmbPersonel.SelectedValue) > 0)
                BakiyeGoster(Convert.ToInt32(cmbPersonel.SelectedValue));
        }

        private void BtnTalepGonder_Click(object sender, EventArgs e)
        {
            if (cmbPersonel.SelectedValue == null || Convert.ToInt32(cmbPersonel.SelectedValue) <= 0)
            { Bildirim("Personel seciniz.", false); return; }

            int pid = Convert.ToInt32(cmbPersonel.SelectedValue);
            int isGunu = IsGunuHesapla(dtpBaslangic.Value, dtpBitis.Value);

            if (dtpBitis.Value.Date < dtpBaslangic.Value.Date || isGunu <= 0)
            { Bildirim("Gecerli tarih araligi giriniz.", false); return; }

            if (AcikTalepVarMi(pid))
            {
                MessageBox.Show(
                    "Bu personelin bekleyen bir talebi var.\nMevcut talep kapanmadan yeni talep acilmaz.",
                    "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbIzinTipi.Text == "Yillik Izin")
            {
                int kalan = KalanGunGetir(pid);
                if (isGunu > kalan)
                {
                    int fazla = isGunu - kalan;
                    var sec = MessageBox.Show(
                        string.Format(
                            "BAKiYE YETERSiZ!\n\nTalep edilen : {0} is gunu\nKalan bakiye : {1} gun\nFazla        : {2} gun\n\n" +
                            "Fazla {2} gun UCRETSIZ iZiN olarak kaydedilsin mi?\n(Hayir deyin talep olusturulmaz.)",
                            isGunu, kalan, fazla),
                        "Bakiye Yetersiz", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (sec == DialogResult.No) return;
                    TalepBolKaydet(pid, kalan, fazla);
                    return;
                }
            }

            TekTalepKaydet(pid, dtpBaslangic.Value.Date, dtpBitis.Value.Date, cmbIzinTipi.Text, txtAciklama.Text.Trim());
            Bildirim("Talep basariyla gonderildi! Amir onayi bekleniyor.", true);
            txtAciklama.Text = "";
            TalepleriYukle();
        }

        private void TekTalepKaydet(int pid, DateTime bas, DateTime bit, string tip, string acik)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SQLiteCommand(@"
                    INSERT INTO IzinTalebi(PersonelID,BaslangicTarihi,BitisTarihi,
                        IzinTipi,Durum,Aciklama,TalepTarihi)
                    VALUES(@pid,@bas,@bit,@tip,'Bekliyor',@acik,datetime('now', 'localtime'))", conn);
                cmd.Parameters.AddWithValue("@pid", pid);
                cmd.Parameters.AddWithValue("@bas", bas.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@bit", bit.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@tip", tip);
                cmd.Parameters.AddWithValue("@acik", acik);
                cmd.ExecuteNonQuery();
            }
        }

        private void TalepBolKaydet(int pid, int kalanGun, int fazlaGun)
        {
            if (kalanGun > 0)
            {
                DateTime yBit = IsGununeBitisHesapla(dtpBaslangic.Value.Date, kalanGun);
                TekTalepKaydet(pid, dtpBaslangic.Value.Date, yBit,
                    "Yillik Izin", string.Format("[Bolundu: {0}g yillik]", kalanGun));

                DateTime uBas = yBit.AddDays(1);
                while (uBas.DayOfWeek == DayOfWeek.Saturday || uBas.DayOfWeek == DayOfWeek.Sunday)
                    uBas = uBas.AddDays(1);
                TekTalepKaydet(pid, uBas, dtpBitis.Value.Date,
                    "Ucretsiz Izin", string.Format("[Bolundu: {0}g ucretsiz - bakiye asimi]", fazlaGun));

                MessageBox.Show(
                    string.Format("Talep ikiye bolundu:\n1) {0} gun Yillik Izin\n2) {1} gun Ucretsiz Izin\nHer ikisi amir onayina gonderildi.", kalanGun, fazlaGun),
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                TekTalepKaydet(pid, dtpBaslangic.Value.Date, dtpBitis.Value.Date,
                    "Ucretsiz Izin", "[Bakiye sifir - tamami ucretsiz izin]");
                MessageBox.Show("Bakiye sifir. Tamami ucretsiz izin olarak gonderildi.",
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            txtAciklama.Text = "";
            TalepleriYukle();
        }

        private DateTime IsGununeBitisHesapla(DateTime bas, int isGunuSayisi)
        {
            int say = 0; DateTime cur = bas;
            while (say < isGunuSayisi)
            {
                if (cur.DayOfWeek != DayOfWeek.Saturday && cur.DayOfWeek != DayOfWeek.Sunday) say++;
                if (say < isGunuSayisi) cur = cur.AddDays(1);
            }
            return cur;
        }

        private bool AcikTalepVarMi(int pid)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SQLiteCommand(
                    "SELECT COUNT(*) FROM IzinTalebi WHERE PersonelID=@pid AND Durum='Bekliyor'", conn);
                cmd.Parameters.AddWithValue("@pid", pid);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private int KalanGunGetir(int pid)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SQLiteCommand(
                    "SELECT IFNULL(KalanGun,0) FROM IzinBakiye WHERE PersonelID=@pid AND Yil=@yil", conn);
                cmd.Parameters.AddWithValue("@pid", pid);
                cmd.Parameters.AddWithValue("@yil", DateTime.Today.Year);
                var s = cmd.ExecuteScalar();
                return (s == null || s == DBNull.Value) ? 0 : Convert.ToInt32(s);
            }
        }

        private void Bildirim(string mesaj, bool basari)
        {
            lblBildirim.Text = mesaj;
            lblBildirim.BackColor = basari ? Color.FromArgb(200, 230, 201) : Color.FromArgb(255, 205, 210);
            lblBildirim.ForeColor = basari ? Color.FromArgb(27, 94, 32) : Color.FromArgb(183, 28, 28);
            lblBildirim.Visible = true;
        }
    }
}