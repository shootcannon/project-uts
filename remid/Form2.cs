using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace remid
{
    public partial class Form2 : Form
    {
        private List<string> cerita;
        private int detikSekarang = 0;
        private List<string> dataBintang;
        private List<Point> posisiBintang = new List<Point>();
        private List<string> jenisBintang = new List<string>();
        private int jamSekarang = 0;
        private bool isSiang = false;
        private int tinggiTanah = 80;
        private List<int> posisiBunga = new List<int>();

        private Random acak = new Random();

        private int playerX = 100;
        private int playerY = 0;

        private int playerWidth = 35;
        private int playerHeight = 55;

        private int speed = 5;
        private int playerVelY = 0;
        private int gravity = 1;
        private int jumpForce = -15;

        private bool isGrounded = false;

        private bool keyLeft = false;
        private bool keyRight = false;

        public Form2(List<string> dataCerita, List<string> dataBintangForm1)
        {
            InitializeComponent();
            cerita = dataCerita;

            dataBintang = dataBintangForm1;

            label1.Location = new Point(10, 10);
            label1.ForeColor = Color.White;
            label1.BackColor = Color.Black;
            label1.AutoSize = true;
            label1.Visible = false;

            this.BackColor = Color.White;
            this.DoubleBuffered = true;

            this.Paint += Form2_Paint;
            this.KeyPreview = true;
            this.KeyDown += Form2_KeyDown;
            this.KeyUp += Form2_KeyUp;

            foreach (string item in dataBintang)
            {
                string[] bagian = item.Split(' ');

                if (bagian.Length >= 5)
                {
                    string jenis = bagian[0];

                    int x = Convert.ToInt32(bagian[2].Replace(",", ""));
                    int y = Convert.ToInt32(bagian[3]);

                    jenisBintang.Add(jenis);
                }
            }

            timer1.Interval = 1000;
            timer1.Tick -= timer1_Tick;
            timer1.Tick += timer1_Tick;

            timer2.Interval = 20;
            timer2.Tick -= timer2_Tick;
            timer2.Tick += timer2_Tick;

            TampilkanCerita();

            timer1.Start();
            timer2.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            detikSekarang++;

            int jamSebelumnya = jamSekarang;

            jamSekarang = detikSekarang % 24;

            if (jamSekarang >= 6 && jamSekarang < 18)
            {
                isSiang = true;
            }
            else
            {
                isSiang = false;
            }

            if (jamSebelumnya == 5 && jamSekarang == 6)
            {
                BuatBunga();
            }

            TampilkanCerita();

            int tanahY = this.ClientSize.Height - tinggiTanah;

            for (int i = 0; i < posisiBintang.Count; i++)
            {
                if (jenisBintang[i] == "Jatuh")
                {
                    Point posisi = posisiBintang[i];

                    posisi.Y = posisi.Y + 5;

                    if (posisi.Y > tanahY - 15)
                    {
                        posisi.Y = 0;
                    }
                    posisiBintang[i] = posisi;
                }
            }

            this.Invalidate();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int tanahY = this.ClientSize.Height - tinggiTanah;

            if (keyLeft)
            {
                playerX = playerX - speed;
            }
            if (keyRight)
            {
                playerX = playerX + speed;
            }

            if (playerX < 0)
            {
                playerX = 0;
            }
            if (playerX > this.ClientSize.Width - playerWidth)
            {
                playerX = this.ClientSize.Width - playerWidth;
            }

            playerY = tanahY + playerVelY;
            playerVelY = playerVelY + gravity;

            if (playerY + playerHeight >= tanahY)
            {
                playerY = tanahY - playerHeight;
                playerVelY = 0;
                isGrounded = true;
            }

            this.Invalidate();
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                keyLeft = true;
            }
            if (e.KeyCode == Keys.D)
            {
                keyRight = true;
            }
            if (e.KeyCode == Keys.W && isGrounded)
            {
                playerVelY = jumpForce;
                isGrounded = false;
            }

            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                keyLeft = false;
            }
            if (e.KeyCode == Keys.D)
            {
                keyRight = false;
            }
        }

        private void BuatBunga()
        {
            posisiBunga.Clear();

            for (int i = 0; i < 8; i++)
            {
                int x = acak.Next(20, this.ClientSize.Width - 20);
                posisiBunga.Add(x);
            }
        }

        private void TampilkanCerita()
        {
            label1.Visible = false;

            foreach (string item in cerita)
            {
                int posisi = item.IndexOf(". muncul pada ");

                if (posisi != -1)
                {
                    string teks = item.Substring(0, posisi);
                    string waktu = item.Substring(posisi + 14);

                    int posisiDurasi = waktu.IndexOf(" selama ");

                    if (posisiDurasi != -1)
                    {
                        string waktuMulai = waktu.Substring(0, posisiDurasi);
                        int detik = Convert.ToInt32(waktuMulai);

                        string lamaCerita = waktu.Substring(posisiDurasi + 8);
                        int durasi = Convert.ToInt32(lamaCerita);

                        if (detikSekarang >= detik && detikSekarang < detik + durasi)
                        {
                            label1.Text = teks;
                            label1.Visible = true;

                            return;
                        }
                    }
                }
            }
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int tanahY = this.ClientSize.Height - tinggiTanah;

            if (isSiang)
            {
                e.Graphics.Clear(Color.LightSkyBlue);
            }
            else
            {
                e.Graphics.Clear(Color.MidnightBlue);
            }

            if (isSiang)
            {
                e.Graphics.FillEllipse(Brushes.Yellow, this.ClientSize.Width - 120, 40, 70, 70);
            }
            else
            {
                e.Graphics.FillEllipse(Brushes.WhiteSmoke, this.ClientSize.Width - 120, 40, 50, 50);
            }

            e.Graphics.FillRectangle(Brushes.ForestGreen, 0, tanahY, this.ClientSize.Width, tinggiTanah);

            if (!isSiang)
            {
                for (int i = 0; i < posisiBintang.Count; i++)
                {
                    Point posisi = posisiBintang[i];

                    if (posisi.Y > tanahY - 15)
                    {
                        posisi.Y = tanahY - 15;
                    }

                    PointF[] titik = new PointF[10];

                    for (int j = 0; j < 10; j++)
                    {
                        double sudut = j * Math.PI / 5 - Math.PI / 2;
                        int radius = 15;

                        if (j % 2 == 1)
                        {
                            radius = 7;
                        }

                        titik[j] = new PointF(posisi.X + (float)Math.Cos(sudut) * radius,
                            posisi.Y + (float)Math.Sin(sudut) * radius
                            );
                    }

                    if (jenisBintang[i] == "Standar")
                    {
                        e.Graphics.FillPolygon(Brushes.Gold, titik);
                    }
                    else if (jenisBintang[i] == "Jatuh")
                    {
                        e.Graphics.FillPolygon(Brushes.OrangeRed, titik);
                    }

                    e.Graphics.DrawPolygon(Pens.Black, titik);
                }
            }

            foreach (int x in posisiBunga)
            {
                e.Graphics.FillRectangle(Brushes.DarkGreen, x - 2, tanahY - 35, 4, 35);

                if (isSiang)
                {
                    e.Graphics.FillEllipse(Brushes.HotPink, x - 15, tanahY - 50, 14, 14);
                    e.Graphics.FillEllipse(Brushes.HotPink, x + 1, tanahY - 50, 14, 14);
                    e.Graphics.FillEllipse(Brushes.HotPink, x - 7, tanahY - 58, 14, 14);
                    e.Graphics.FillEllipse(Brushes.HotPink, x - 7, tanahY - 42, 14, 14);

                    e.Graphics.FillEllipse(Brushes.Yellow, x - 7, tanahY - 50, 14, 14);

                }
            }

            GambarKarakter(e.Graphics, playerX, playerY);
        }

        private void GambarKarakter(Graphics g, int x, int y)
        {
            g.FillEllipse(Brushes.NavajoWhite, x + 5, y, 25, 25);

            g.FillEllipse(Brushes.RoyalBlue, x + 5, y + 23,25, 22);

            g.FillEllipse(Brushes.DarkSlateGray, x + 6, y + 45, 9, 10);
            g.FillEllipse(Brushes.DarkSlateGray, x + 20, y + 45, 9, 10);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            TampilkanCerita();

            this.Invalidate();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {

            timer1.Stop();
            timer2.Stop();

            base.OnFormClosed(e);
        }
    }
}
