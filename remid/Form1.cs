namespace remid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string teks = textBox1.Text.Trim();

            int detik = (int)numericUpDown1.Value;
            int durasi = (int)numericUpDown2.Value;

            if (string.IsNullOrWhiteSpace(teks))
            {
                MessageBox.Show("Text cerita tidak boleh kosong!");
                return;
            }

            string items = $"{teks}. muncul pada {detik} selama {durasi}";

            listBox1.Items.Add(items);

            textBox1.Clear();

            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;

            textBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int x = (int)numericUpDown3.Value;
            int y = (int)numericUpDown4.Value;

            int urutan = listBox2.Items.Count + 1;

            if (x == 0 || y == 0)
            {
                MessageBox.Show("X dan Y tidak boleh kosong!");
                return;
            }

            string jenis = "";

            if (radioButton1.Checked)
            {
                jenis = "Standar";
            }
            else if (radioButton2.Checked)
            {
                jenis = "Jatuh";
            }
            else
            {
                MessageBox.Show("Harus pilih bintang dulu!");
                return;
            }

            string value = $"0 -> {x}, {y} : {urutan}";

            listBox2.Items.Add(value);
        }

        private void button4_Click(object sender, EventArgs e)
        {

            List<string> cerita = new List<string>();

            foreach (var item in listBox1.Items)
            {
                cerita.Add(item.ToString());
            }

            List<string> bintang = new List<string>();

            foreach (var item in listBox2.Items)
            {
                bintang.Add(item.ToString());
            }

            Form2 form2 = new Form2(cerita, bintang);
            form2.Show();
        }
    }
}
