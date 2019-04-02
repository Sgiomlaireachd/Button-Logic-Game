using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic_Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random rand = new Random();
            int N = 8;
            int GAP = 15;
            int BUTTON_SIZE = 65;
            Btn[,] buttons = new Btn[N, N];
            this.Width = BUTTON_SIZE * N + GAP * (N + 1);
            this.Height = BUTTON_SIZE * N + GAP * (N + 1);

            for(int i = 0; i < N; i++)
            {
                int x = GAP * (i + 1) + BUTTON_SIZE * i;
                for(int j = 0; j < N; j++)
                {
                    int y = GAP * (j + 1) + BUTTON_SIZE * j;
                    Button b = new Button();
                    b.Location = new Point(x, y);
                    b.Height = BUTTON_SIZE;
                    b.Width = BUTTON_SIZE;
                    b.KeyDown += Form1_KeyDown;

                    buttons[i, j] = new Btn(b, rand,i,j);
                    this.Controls.Add(b);
                }
            }
            Btn.setButtons(buttons,N);
            Btn.f = this;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }



        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
    public class Btn
    {
        Button btn;
        bool active;
        static Btn[,] buttons;
        static int N;
        public static Form f;

        int i, j;

        public Btn(Button b, Random r, int i, int j)
        {
            btn = b;
            double a = r.Next(0, 2);
            Console.WriteLine(a);
            active = Convert.ToBoolean(a);
            this.i = i;
            this.j = j;

            btn.Click += onClick;
            display();
        }
        public void onClick(object sender, EventArgs e)
        {
            active = !active;
            display();
            
            if(j > 0)
            {
                buttons[i, j - 1].active = !buttons[i, j - 1].active;
                buttons[i, j - 1].display();
            }
            if (j < N-1)
            {
                buttons[i, j + 1].active = !buttons[i, j + 1].active;
                buttons[i, j + 1].display();
            }
            if (i > 0)
            {
                buttons[i - 1, j].active = !buttons[i - 1, j].active;
                buttons[i - 1, j].display();
            }
            if (i < N - 1)
            {
                buttons[i + 1, j].active = !buttons[i + 1, j].active;
                buttons[i + 1, j].display();
            }

            if (allButtonsAreActive())
            {
                MessageBox.Show("YOU WON!","Congratulations");
            }
            
        }

        void display()
        {
            if (active) btn.BackColor = Color.FromArgb(0, 255, 0);
            else btn.BackColor = Color.FromArgb(255,0,0);
        }
        static public void setButtons(Btn[,]b,int n)
        {
            buttons = b;
            N = n;
        }

        bool allButtonsAreActive()
        {
            for(int i = 0; i < N; i++)
            {
                for(int j = 0; j < N; j++)
                {
                    if (!buttons[i, j].active) return false;
                }
            }
            return true;
        }
    }




}
