using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuaX
{
    public partial class Settings : Form
    {

        //Draggable Control
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public Settings()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Settings_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void BunifuCheckbox1_OnChange(object sender, EventArgs e)
        {
            if (bunifuCheckbox1.Checked == true)
            {
                ((Form1)Owner).TopMost = true;

            }
            else
            {
                ((Form1)Owner).TopMost = false;
            }
        }

        private void BunifuCheckbox2_OnChange(object sender, EventArgs e)
        {
            if (((Form1)Owner).webBrowser1.Visible == true)
            {
                if (bunifuCheckbox2.Checked == true)
                {
                    ((Form1)Owner).webBrowser1.Document.InvokeScript("SetTheme", new string[]
                         {

                         "Dark"

                     });
                    ((Form1)Owner).browserdark = true;
                }
                else
                {
                    ((Form1)Owner).webBrowser1.Document.InvokeScript("SetTheme", new string[]
                         {

                         "Light"
                     });
                    ((Form1)Owner).browserdark = false;
                }
            }
            else
            {
                if (((Form1)Owner).fastColoredTextBox1.BackColor == Color.FromArgb(30,30,30)) {

                    ((Form1)Owner).fastColoredTextBox1.BackColor = Color.White;
                     ((Form1)Owner).fastColoredTextBox1.CaretColor = Color.Black;
                    ((Form1)Owner).fastColoredTextBox1.ForeColor = Color.Black;
                    ((Form1)Owner).fastColoredTextBox1.LineNumberColor = Color.Black;
                    ((Form1)Owner).fastColoredTextBox1.IndentBackColor = Color.White;
                    ((Form1)Owner).fastColoredTextBox1.ServiceLinesColor = Color.LightGray;


                }
                else
                {
                    ((Form1)Owner).fastColoredTextBox1.BackColor = Color.FromArgb(30, 30, 30);
                    ((Form1)Owner).fastColoredTextBox1.CaretColor = Color.White;
                    ((Form1)Owner).fastColoredTextBox1.ForeColor = Color.DarkGray;
                    ((Form1)Owner).fastColoredTextBox1.LineNumberColor = Color.DarkGray;
                    ((Form1)Owner).fastColoredTextBox1.IndentBackColor = Color.FromArgb(30, 30, 30);
                    ((Form1)Owner).fastColoredTextBox1.ServiceLinesColor = Color.Transparent;
                }
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            if (((Form1)Owner).webBrowser1.Visible == true)
            {
                if (((Form1)Owner).browserdark == true)
                {
                    bunifuCheckbox2.Checked = true;
                }
                else
                {
                    bunifuCheckbox2.Checked = false;
                }
            }
            else
            {
                if (((Form1)Owner).fastColoredTextBox1.BackColor == Color.FromArgb(30, 30, 30))
                {
                    bunifuCheckbox2.Checked = true;
                }
                else
                {
                    bunifuCheckbox2.Checked = false;
                }
            }
            if (((Form1)Owner).TopMost == true)
            {
                bunifuCheckbox1.Checked = true;
            }
            else
            {
                bunifuCheckbox1.Checked = false;
            }

        }
    }
}


