using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace PersonelYonetimSistemi
{
    public partial class Form1 : Form
    {
        private static readonly Color C_KOYU_MAVI = Color.FromArgb(15, 52, 96);
        private static readonly Color C_MAVI = Color.FromArgb(25, 118, 210);
        private static readonly Color C_YESIL = Color.FromArgb(46, 125, 50);
        private static readonly Color C_MOR = Color.FromArgb(106, 27, 154);
        private static readonly Color C_TEAL = Color.FromArgb(0, 121, 107);
        private static readonly Color C_KIRMIZI = Color.FromArgb(183, 28, 28);
        private static readonly Color C_ARKAPLAN = Color.FromArgb(232, 240, 254);
        private static readonly Color C_BILDIRIM = Color.FromArgb(255, 243, 205);

        private Timer tmrSaat;
        private Label lblSaat;
        private Label lblUyariBant;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormAyarla();
            YildonumuKontrolEt();
            SozlesmeUyariKontrol();
        }

        private void FormAyarla()
        {
            this.Text = "Tekstil A.S. -- Personel Yonetim Sistemi";
            this.BackColor = C_ARKAPLAN;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(780, 620);
            this.Size = new Size(860, 700);
            this.Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;

            var pnlBaslik = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = C_KOYU_MAVI
            };
            this.Controls.Add(pnlBaslik);

            var lblBaslik = new Label
            {
                Text = "TEKSTIL A.S.",
                Font = new Font("Segoe UI", 22f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Bounds = new Rectangle(0, 8, 860, 42),
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlBaslik.Controls.Add(lblBaslik);

            var lblAlt = new Label
            {
                Text = "Personel, Izin ve Kariyer Yonetim Sistemi",
                Font = new Font("Segoe UI", 11f, FontStyle.Italic),
                ForeColor = Color.FromArgb(180, 220, 255),
                AutoSize = false,
                Bounds = new Rectangle(0, 50, 860, 26),
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlBaslik.Controls.Add(lblAlt);

            lblSaat = new Label
            {
                Text = DateTime.Now.ToString("dd.MM.yyyy  HH:mm:ss"),
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(160, 200, 255),
                AutoSize = false,
                Bounds = new Rectangle(0, 78, 850, 20),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            pnlBaslik.Controls.Add(lblSaat);

            tmrSaat = new Timer { Interval = 1000 };
            tmrSaat.Tick += (s, e2) =>
                lblSaat.Text = DateTime.Now.ToString("dd.MM.yyyy  HH:mm:ss");
            tmrSaat.Start();

            lblUyariBant = new Label
            {
                Dock = DockStyle.None,
                Visible = false,
                Height = 34,
                BackColor = C_BILDIRIM,
                ForeColor = Color.FromArgb(130, 80, 0),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(lblUyariBant);

            string[] etiketler =
            {
                "Personel Yonetimi",
                "Izin Yonetimi",
                "Onay Islemleri",
                "Sozlesme Yonetimi",
                "Raporlar"
            };
            string[] aciklamalar =
            {
                "Calisan bilgileri, kariyer gecmisi ve kidem takibi",
                "Izin talepleri, bakiye ve is gunu hesaplama",
                "Amir / IK onay akisi yonetimi",
                "Sozlesme tarihleri ve 60 gun uyari sistemi",
                "PDF rapor olusturma ve disa aktarma"
            };
            Color[] renkler = { C_MAVI, C_YESIL, C_MOR, C_TEAL, C_KIRMIZI };
            string[] ikonlar = { "  ►  ", "  ►  ", "  ►  ", "  ►  ", "  ►  " };

            for (int i = 0; i < 5; i++)
            {
                int idx = i;
                var pnlBtn = new Panel
                {
                    BackColor = Color.White,
                    Cursor = Cursors.Hand,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };
                pnlBtn.Paint += (s, e2) =>
                {
                    var g = e2.Graphics;
                    using (var brush = new SolidBrush(renkler[idx]))
                        g.FillRectangle(brush, 0, 0, 6, pnlBtn.Height);
                };

                var lblBtnBaslik = new Label
                {
                    Text = ikonlar[i] + etiketler[i],
                    Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                    ForeColor = renkler[i],
                    AutoSize = false,
                    Bounds = new Rectangle(18, 10, 700, 26),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };
                var lblBtnAcik = new Label
                {
                    Text = aciklamalar[i],
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = Color.FromArgb(90, 90, 90),
                    AutoSize = false,
                    Bounds = new Rectangle(24, 38, 700, 18),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };

                pnlBtn.Controls.Add(lblBtnBaslik);
                pnlBtn.Controls.Add(lblBtnAcik);

                pnlBtn.MouseEnter += (s, e2) => pnlBtn.BackColor = Color.FromArgb(245, 248, 255);
                pnlBtn.MouseLeave += (s, e2) => pnlBtn.BackColor = Color.White;
                lblBtnBaslik.MouseEnter += (s, e2) => pnlBtn.BackColor = Color.FromArgb(245, 248, 255);
                lblBtnBaslik.MouseLeave += (s, e2) => pnlBtn.BackColor = Color.White;
                lblBtnAcik.MouseEnter += (s, e2) => pnlBtn.BackColor = Color.FromArgb(245, 248, 255);
                lblBtnAcik.MouseLeave += (s, e2) => pnlBtn.BackColor = Color.White;

                pnlBtn.Click += (s, e2) => MenuTikla(idx);
                lblBtnBaslik.Click += (s, e2) => MenuTikla(idx);
                lblBtnAcik.Click += (s, e2) => MenuTikla(idx);

                this.Controls.Add(pnlBtn);
            }

            var pnlAlt = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 28,
                BackColor = C_KOYU_MAVI
            };
            var lblVersiyon = new Label
            {
                Text = "v1.0  |  Tekstil A.S. IK Sistemi  |  Tum haklar saklidir.",
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(140, 180, 230),
                Font = new Font("Segoe UI", 8f),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlAlt.Controls.Add(lblVersiyon);
            this.Controls.Add(pnlAlt);

            this.Resize += Form1_Resize;
            Form1_Resize(null, null);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int w = this.ClientSize.Width;
            int startY = 100;
            if (lblUyariBant != null && lblUyariBant.Visible)
            {
                lblUyariBant.Bounds = new Rectangle(0, startY, w, 34);
                startY += 34;
            }
            int panelH = 68;
            int gap = 6;
            int y = startY + 16;
            int margin = Math.Max(20, (w - 680) / 2);
            int panelW = w - margin * 2;

            int panelIdx = 0;
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Panel pnl && pnl.Dock != DockStyle.Top && pnl.Dock != DockStyle.Bottom)
                {
                    pnl.Bounds = new Rectangle(margin, y, panelW, panelH);
                    y += panelH + gap;
                    panelIdx++;
                    if (panelIdx >= 5) break;
                }
            }
        }

        private void MenuTikla(int idx)
        {
            switch (idx)
            {
                case 0: new frmPersonel().Show(); break;
                case 1: new frmIzin().Show(); break;
                case 2: new frmOnay().Show(); break;
                case 3: new frmSozlesme().Show(); break;
                case 4: new frmRapor().Show(); break;
            }
        }

        private void YildonumuKontrolEt()
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    var da = new SQLiteDataAdapter(
                        "SELECT Id AS PersonelID, IseGirisTarihi FROM Personeller WHERE Durum='Aktif'", conn);
                    var dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        DBHelper.YildonumuHakGuncelle(
                            Convert.ToInt32(row["PersonelID"]),
                            Convert.ToDateTime(row["IseGirisTarihi"]));
                }
            }
            catch { }
        }

        private void SozlesmeUyariKontrol()
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string sql = @"
                        SELECT COUNT(*) FROM Sozlesme s
                        JOIN Personeller p ON s.PersonelID=p.Id
                        WHERE p.Durum='Aktif'
                          AND s.BitisTarihi IS NOT NULL
                          AND CAST(julianday(s.BitisTarihi) - julianday('now') AS INTEGER) BETWEEN 0 AND 60";
                    int sayi = Convert.ToInt32(new SQLiteCommand(sql, conn).ExecuteScalar());
                    if (sayi > 0)
                    {
                        lblUyariBant.Text = string.Format(
                            "  UYARI: {0} sozlesme onumuzdeki 60 gun icinde bitiyor! Sozlesme ekranini kontrol edin.", sayi);
                        lblUyariBant.Visible = true;
                        Form1_Resize(null, null);
                    }
                }
            }
            catch { }
        }
    }
}