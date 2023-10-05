using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Black_Jack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Random rand = new Random();

        PictureBox[] cards = new PictureBox[52];
        PictureBox back = new PictureBox();

        int count = 0;
        int croupier = 0;
        int player = 0;
        int sum_c = 0;
        int sum_p = 0;
        int _sum_c = 0;
        int _sum_p = 0;
        int A_c = 0;
        int A_p = 0;
        int chip = 50000;
        int bet = 0;

        private void Initialize()
        {
            for (int i = 0; i < 52; i++)
                cards[i].Visible = false;
            back.Visible = false;
            
            Shuffle(cards);
            Image_Set(cards);

            count = 0;
            croupier = 0;
            player = 0;
            sum_c = 0;
            sum_p = 0;
            _sum_c = 0;
            _sum_p = 0;
            A_c = 0;
            A_p = 0;
        }

        private void Ready()
        {
            Croupier_Draw_Card(cards);
            back.Location = new Point(12 +  croupier * 30, 24);
            back.Visible = true;
            back.BringToFront();

            Player_Draw_Card(cards);
            Player_Draw_Card(cards);

            button1.Enabled = true;
            button2.Enabled = true;
        }
    
        private void Shuffle(PictureBox[] n)
        {
            int[] t = new int[52];
            for (int i = 0; i < t.Length; i++)
                t[i] = i;

            for (int i = 0; i < t.Length; i++)
            {
                int random = rand.Next(0, 52);
                int temp = t[i];
                t[i] = t[random];
                t[random] = temp;
            }

            int k = 0;
            for (int i = 0; i < 52; i++)
            {
                n[i].Tag = t[k];
                k++;
            }
        }

        private void Image_Set(PictureBox[] n)
        {
            string[] suits = new string[4] { "s", "h", "d", "c" };

            for (int i = 0; i < n.Length; i++)
            { 
                int tag = (int)n[i].Tag;
                int no = tag % 13 + 1;
                n[i].Image = (Image)Properties.Resources.ResourceManager.GetObject(suits[tag / 13] + no.ToString("00"));
            }
        }

        private void Wait()
        {
            button4.Enabled = true;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = true;
            numericUpDown1.Enabled = true;
        }

        

        private void A_Sum(ref int s,int _s,int A)
        {
            int j = 0;
            for (int i = A; i >= 0; i--)
            {
                s = _s + 11 * i + j++;
                if (s <= 21)
                    break;
                else
                    s = _s + j - 1;
            }
        }

        private void Croupier_Draw_Card(PictureBox[] n)
        {
            n[count].Location = new Point(12 + croupier++ * 30, 24);
            n[count].Visible = true;
            n[count].BringToFront();
           
            if ((int)n[count].Tag % 13 + 1 >= 11)
                _sum_c += 10;
            else if ((int)n[count].Tag % 13 + 1 != 1)
                _sum_c += (int)n[count].Tag % 13 + 1;
            else
                A_c++;

            A_Sum(ref sum_c, _sum_c, A_c);

            if (sum_c > 21)
            {
                label3.Text = sum_c.ToString();
                MessageBox.Show("WIN");
                chip += 2 * bet;
                bet = 0;
                textBox1.Text = chip.ToString();
                textBox2.Text = "";
                label3.Text = "";
                label4.Text = "";
            }
            else
                label3.Text = sum_c.ToString();

            count++;
        }

        private void Player_Draw_Card(PictureBox[] n)
        {
            n[count].Location = new Point(12 + player++ * 30, 240);
            n[count].Visible = true;
            n[count].BringToFront();

            if ((int)n[count].Tag % 13 + 1 >= 11)
                _sum_p += 10;
            else if ((int)n[count].Tag % 13 + 1 != 1)
                _sum_p += (int)n[count].Tag % 13 + 1;
            else
                A_p++;

            A_Sum(ref sum_p, _sum_p, A_p);

            if (sum_p > 21)
            {
                back.Visible = false;
                n[++count].Visible = true;
                n[count].Location = new Point(42, 24);
                n[count].BringToFront();

                if ((int)n[count].Tag % 13 + 1 >= 11)
                    _sum_c += 10;
                else if ((int)n[count].Tag % 13 + 1 != 1)
                    _sum_c += (int)n[count].Tag % 13 + 1;
                else
                    A_c++;

                A_Sum(ref sum_c, _sum_c, A_c);

                label3.Text = sum_c.ToString();
                label4.Text = sum_p.ToString();
                MessageBox.Show("LOSE");
                bet = 0;
                textBox2.Text = "";
                label3.Text = "";
                label4.Text = "";
                Initialize();
                Wait();
            }
            else
                label4.Text = sum_p.ToString();

            count++;
        }

        private void Compare(int x,int y)
        {
            if (x == y)
            {
                MessageBox.Show("DRAW");
                chip += bet;
                bet = 0;
                textBox1.Text = chip.ToString();
                textBox2.Text = "";
            }
            else if (x > y)
            {
                MessageBox.Show("WIN");
                chip += 2 * bet;
                bet = 0;
                textBox1.Text = chip.ToString();
                textBox2.Text = "";
            }
            else
            {
                MessageBox.Show("LOSE");
                bet = 0;
                textBox2.Text = "";
            }

            label3.Text = "";
            label4.Text = "";
            Initialize();
            Wait();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label8.Text = Properties.Settings.Default.high_score.ToString();
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = new PictureBox();
                cards[i].Size = new Size(70, 105);
                cards[i].SizeMode = PictureBoxSizeMode.Zoom;
                Controls.Add(cards[i]);
            }

            back.Size = new Size(70, 105);
            back.SizeMode = PictureBoxSizeMode.Zoom;
            back.Image = Properties.Resources.z01;
            Controls.Add(back);

            textBox1.Text = chip.ToString();
            Wait();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Player_Draw_Card(cards);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            back.Visible = false;

            while (true)
            {
                Croupier_Draw_Card(cards);
                if (sum_c > 21)
                {
                    Initialize();
                    Wait();
                    break;
                }
                else if (sum_c >= 17)
                {
                    Compare(sum_p, sum_c);
                    break;
                }
            }  
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(numericUpDown1.Value == 0)
            {
                numericUpDown1.Value = 0;
            }
            else if (chip - (int)numericUpDown1.Value >= 0)
            {
                chip -= (int)numericUpDown1.Value;
                bet = (int)numericUpDown1.Value;
                textBox1.Text = chip.ToString();
                textBox2.Text = bet.ToString();
                button3.Enabled = false;
                button4.Enabled = false;
                button1.Enabled = true;
                button2.Enabled = true;
                numericUpDown1.Enabled = false;
                Initialize();
                Ready();
            }
            else
            {
                numericUpDown1.Value = 0;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (chip > Properties.Settings.Default.high_score)
            {
                Properties.Settings.Default.high_score = chip;
                Properties.Settings.Default.Save();
                label8.Text = Properties.Settings.Default.high_score.ToString();
                chip = 50000;
                textBox1.Text = chip.ToString();
                Initialize();
                Wait();
            }
            else
            {
                chip = 50000;
                textBox1.Text = chip.ToString();
                Initialize();
                Wait();
            }
                
        }
    }
}
