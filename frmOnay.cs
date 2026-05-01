using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace PersonelYonetimSistemi
{
    public partial class frmOnay : Form
    {
        private ComboBox cmbRol;
        private DataGridView dgv;
        private Button btnOnayla, btnReddet, btnYenile;
        private Label lblBaslikBant, lblAciklama, lblBildirim, lblSayac;

        private static readonly Color C_BASLIK = Color.FromArgb(74, 20, 140);
        private static readonly Color C_ARKA = Color.FromArgb(243, 240, 255);

        public frmOnay()
        {
            this.Text = "Onay Islemleri";
            this.BackColor = C_ARKA;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(980, 580);
            this.Size = new Size(1100, 680);
            this.Font = new Font("Segoe UI", 9f);
            this.Load += FrmOnay_Load;
            FormOlustur();
        }

        private void FormOlustur()
        {
            var pnlBaslik = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = C_BASLIK };
            pnlBaslik.Controls.Add(new Label
            {
                Text = "ONAY ISLEMLERI",
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            });
            this.Controls.Add(pnlBaslik);

            var pnlFiltre = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.White,
                Padding = new Padding(10, 8, 10, 8)
            };

            var lblRol = new Label
            {
                Text = "Oturum Acik Kullanici :",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = C_BASLIK,
                AutoSize = true,
                Location = new Point(10, 14)
            };
            pnlFiltre.Controls.Add(lblRol);

            cmbRol = new ComboBox
            {
                Font = new Font("Segoe UI", 10f),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(220, 12),
                Width = 240,
                Height = 28
            };
            cmbRol.Items.AddRange(new object[]
            {
                "Suzan Ozer (Genel Amir)",
                "Cennet Firdevs Uzan (IK Yoneticisi)"
            });
            cmbRol.SelectedIndex = 0;
            cmbRol.SelectedIndexChanged += (s, e) => BekleyenleriYukle();
            pnlFiltre.Controls.Add(cmbRol);

            btnYenile = new Button
            {
                Text = "Yenile",
                Size = new Size(90, 30),
                Location = new Point(475, 11),
                BackColor = Color.FromArgb(74, 20, 140),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnYenile.FlatAppearance.BorderSize = 0;
            btnYenile.Click += (s, e) => BekleyenleriYukle();
            pnlFiltre.Controls.Add(btnYenile);

            lblSayac = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 20, 140),
                AutoSize = true,
                Location = new Point(580, 15)
            };
            pnlFiltre.Controls.Add(lblSayac);
            this.Controls.Add(pnlFiltre);

            lblBaslikBant = new Label
            {
                Dock = DockStyle.Top,
                Height = 38,
                BackColor = Color.FromArgb(237, 231, 246),
                ForeColor = Color.FromArgb(74, 20, 140),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Onay Akisi:  Calisan  -->  Suzan Ozer (Veya sadece Suzan izindeyken IK)"
            };
            this.Controls.Add(lblBaslikBant);

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
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9f),
                ColumnHeadersHeight = 36,
                RowTemplate = { Height = 30 },
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(220, 210, 240),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = C_BASLIK;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(209, 196, 233);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 245, 255);
            this.Controls.Add(dgv);

            var pnlAlt = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 90,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            lblAciklama = new Label
            {
                Text = "Listeden bir talep secin, sonra Onayla veya Reddet butonuna basin.",
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 100, 100),
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(4, 0, 0, 0)
            };
            pnlAlt.Controls.Add(lblAciklama);

            var pnlBtnSatir = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 4, 0, 0)
            };

            btnOnayla = new Button
            {
                Text = "ONAYLA",
                Size = new Size(160, 40),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnOnayla.FlatAppearance.BorderSize = 0;
            btnOnayla.Click += BtnOnayla_Click;

            btnReddet = new Button
            {
                Text = "REDDET",
                Size = new Size(160, 40),
                BackColor = Color.FromArgb(183, 28, 28),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnReddet.FlatAppearance.BorderSize = 0;
            btnReddet.Click += BtnReddet_Click;

            lblBildirim = new Label
            {
                Text = "",
                AutoSize = false,
                Width = 500,
                Height = 40,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };

            pnlBtnSatir.Controls.AddRange(new Control[] { btnOnayla, btnReddet, lblBildirim });
            pnlAlt.Controls.Add(pnlBtnSatir);
            this.Controls.Add(pnlAlt);
        }

        private void FrmOnay_Load(object sender, EventArgs e)
        {
            BekleyenleriYukle();
        }

        private void BekleyenleriYukle()
        {
            bool suzanSecili = cmbRol.SelectedIndex == 0;
            string rolAciklama = suzanSecili
                ? "Suzan Ozer onayini bekleyen talepler listeleniyor."
                : "IK (Cennet Firdevs Uzan) onayini bekleyen talepler listeleniyor. (Sadece Amir izindeyken gelenler ve IK personeli)";

            lblAciklama.Text = rolAciklama;

            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string sql = @"
                        SELECT
                            t.TalepID,
                            t.PersonelID,
                            p.Ad || ' ' || p.Soyad                 AS Personel,
                            p.Departman,
                            p.Pozisyon,
                            t.IzinTipi,
                            strftime('%d.%m.%Y', t.BaslangicTarihi) AS Baslangic,
                            strftime('%d.%m.%Y', t.BitisTarihi)     AS Bitis,
                            CAST(julianday(t.BitisTarihi) - julianday(t.BaslangicTarihi) AS INTEGER) + 1 
                                                                    AS ToplamGun,
                            t.Durum,
                            strftime('%d.%m.%Y', t.TalepTarihi)     AS TalepTarihi,
                            substr(IFNULL(t.Aciklama,''), 1, 80)    AS Aciklama,
                            0                                       AS AmirizindeMi
                        FROM IzinTalebi t
                        JOIN Personeller p ON t.PersonelID = p.Id
                        WHERE ";

                    if (suzanSecili)
                    {
                        sql += " t.Durum = 'Bekliyor' AND p.Departman != 'Insan Kaynaklari' ";
                    }
                    else
                    {
                        sql += " t.Durum = 'AmirOnayladi' OR (t.Durum = 'Bekliyor' AND p.Departman = 'Insan Kaynaklari') ";
                    }

                    sql += " ORDER BY t.TalepTarihi";

                    var da = new SQLiteDataAdapter(sql, conn);
                    var dt = new DataTable();
                    da.Fill(dt);
                    dgv.DataSource = dt;

                    foreach (string gizli in new[] { "TalepID", "PersonelID", "AmirizindeMi" })
                        if (dgv.Columns[gizli] != null) dgv.Columns[gizli].Visible = false;

                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        bool amirizinde = Convert.ToBoolean(
                            dgv.Columns["AmirizindeMi"] != null ? row.Cells["AmirizindeMi"].Value : false);
                        if (amirizinde)
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 205);
                    }

                    lblSayac.Text = string.Format("Bekleyen talep: {0}", dt.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yukleme hatasi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOnayla_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null || dgv.SelectedRows.Count == 0)
            { Bildirim("Listeden bir talep secin.", false); return; }

            int talepID = Convert.ToInt32(dgv.CurrentRow.Cells["TalepID"].Value);
            int personelID = Convert.ToInt32(dgv.CurrentRow.Cells["PersonelID"].Value);
            bool suzanSecili = cmbRol.SelectedIndex == 0;
            string onaylayan = suzanSecili ? "Suzan Ozer (Genel Amir)" : "Cennet Firdevs Uzan (IK)";

            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    var cmd = new SQLiteCommand(@"
                        UPDATE IzinTalebi
                        SET Durum    = 'Onaylandi',
                            Aciklama = IFNULL(Aciklama,'') || @acik
                        WHERE TalepID = @id", conn);
                    cmd.Parameters.AddWithValue("@acik", " [" + onaylayan + " onayladi]");
                    cmd.Parameters.AddWithValue("@id", talepID);
                    cmd.ExecuteNonQuery();

                    BakiyeDus(conn, talepID);
                    Bildirim("Talep " + onaylayan + " tarafindan kesin onaylandi. Bakiye guncellendi.", true);
                }
                BekleyenleriYukle();
            }
            catch (Exception ex) { Bildirim("Hata: " + ex.Message, false); }
        }

        private void BtnReddet_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null || dgv.SelectedRows.Count == 0)
            { Bildirim("Listeden bir talep secin.", false); return; }

            int talepID = Convert.ToInt32(dgv.CurrentRow.Cells["TalepID"].Value);
            string personel = dgv.CurrentRow.Cells["Personel"].Value?.ToString() ?? "";
            bool suzanSecili = cmbRol.SelectedIndex == 0;
            string reddeden = suzanSecili ? "Suzan Ozer" : "Cennet Firdevs Uzan";

            if (MessageBox.Show(
                string.Format("{0} kisisinin talebini reddetmek istiyor musunuz?", personel),
                "Reddetme Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    var cmd = new SQLiteCommand(@"
                        UPDATE IzinTalebi
                        SET Durum    = 'Reddedildi',
                            Aciklama = IFNULL(Aciklama,'') || @acik
                        WHERE TalepID = @id", conn);
                    cmd.Parameters.AddWithValue("@acik",
                        string.Format(" [{0} tarafindan reddedildi]", reddeden));
                    cmd.Parameters.AddWithValue("@id", talepID);
                    cmd.ExecuteNonQuery();
                }
                Bildirim("Talep reddedildi.", false);
                BekleyenleriYukle();
            }
            catch (Exception ex) { Bildirim("Hata: " + ex.Message, false); }
        }

        private void BakiyeDus(SQLiteConnection conn, int talepID)
        {
            var cmd = new SQLiteCommand(@"
                SELECT PersonelID, BaslangicTarihi, BitisTarihi, IzinTipi
                FROM IzinTalebi WHERE TalepID = @id", conn);
            cmd.Parameters.AddWithValue("@id", talepID);

            int personelID = 0;
            DateTime bas = DateTime.Today, bit = DateTime.Today;
            string tip = "";

            using (var r = cmd.ExecuteReader())
            {
                if (r.Read())
                {
                    personelID = Convert.ToInt32(r["PersonelID"]);
                    bas = Convert.ToDateTime(r["BaslangicTarihi"]);
                    bit = Convert.ToDateTime(r["BitisTarihi"]);
                    tip = r["IzinTipi"].ToString();
                }
            }

            if (tip != "Yillik Izin") return;

            int isGunu = 0;
            DateTime cur = bas.Date;
            while (cur <= bit.Date)
            {
                if (cur.DayOfWeek != DayOfWeek.Saturday &&
                    cur.DayOfWeek != DayOfWeek.Sunday) isGunu++;
                cur = cur.AddDays(1);
            }
            if (isGunu <= 0) return;

            var cmdB = new SQLiteCommand(@"
                UPDATE IzinBakiye
                SET KullanilanGun = KullanilanGun + @gun,
                    KalanGun      = KalanGun      - @gun
                WHERE PersonelID = @pid AND Yil = @yil", conn);
            cmdB.Parameters.AddWithValue("@gun", isGunu);
            cmdB.Parameters.AddWithValue("@pid", personelID);
            cmdB.Parameters.AddWithValue("@yil", DateTime.Today.Year);
            cmdB.ExecuteNonQuery();
        }

        private void Bildirim(string mesaj, bool basari)
        {
            lblBildirim.Text = mesaj;
            lblBildirim.BackColor = basari ? Color.FromArgb(200, 230, 201) : Color.FromArgb(255, 205, 210);
            lblBildirim.ForeColor = basari ? Color.FromArgb(27, 94, 32) : Color.FromArgb(183, 28, 28);
        }
    }
}