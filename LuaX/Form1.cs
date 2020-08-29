using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Microsoft.Win32;
using System.Diagnostics;
using static LuaX.DiscordRpc;
using System.Reflection;

namespace LuaX
{
    /// <summary>
    /// THIS UI IS NOT TO BE REUSED IN ANY FORM
    /// GIVE FULL CREDITS TO CREATOR
    /// </summary>

    public partial class Form1 : Form
    {
       
        public bool browserdark = true;
        public const string DLL = "/bin/discord-rpc-w32";
        private DiscordRpc.RichPresence presence;
        DiscordRpc.EventHandlers handlers;
        #region Monaco
        private string defPath = Application.StartupPath + "//bin//Monaco//";

        private void addIntel(string label, string kind, string detail, string insertText)
        {
            string text = "\"" + label + "\"";
            string text2 = "\"" + kind + "\"";
            string text3 = "\"" + detail + "\"";
            string text4 = "\"" + insertText + "\"";
            webBrowser1.Document.InvokeScript("AddIntellisense", new object[]
            {
                label,
                kind,
                detail,
                insertText
            });
        }
        private void Initialize(string clientId)
        {
            handlers = new DiscordRpc.EventHandlers();

            DiscordRpc.Initialize(clientId, ref handlers, true, null);

        }



        private void addGlobalF()
        {
            string[] array = File.ReadAllLines(this.defPath + "//globalf.txt");
            foreach (string text in array)
            {
                bool flag = text.Contains(':');
                if (flag)
                {
                    this.addIntel(text, "Function", text, text.Substring(1));
                }
                else
                {
                    this.addIntel(text, "Function", text, text);
                }
            }
        }

        private void addGlobalV()
        {
            foreach (string text in File.ReadLines(this.defPath + "//globalv.txt"))
            {
                this.addIntel(text, "Variable", text, text);
            }
        }

        private void addGlobalNS()
        {
            foreach (string text in File.ReadLines(this.defPath + "//globalns.txt"))
            {
                this.addIntel(text, "Class", text, text);
            }
        }

        private void addMath()
        {
            foreach (string text in File.ReadLines(this.defPath + "//classfunc.txt"))
            {
                this.addIntel(text, "Method", text, text);
            }
        }

        private void addBase()
        {
            foreach (string text in File.ReadLines(this.defPath + "//base.txt"))
            {
                this.addIntel(text, "Keyword", text, text);
            }
        }
#endregion
        //Draggable Control
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public Form1()
        {
            InitializeComponent();

        }
        private long DateTimeToTimestamp(DateTime dt)
        {
            return (dt.Ticks - 621355968000000000) / 10000000;
        }

      

        private async void Form1_Load(object sender, EventArgs e) {
            this.Initialize("");
            presence.details = "";
            presence.state = "";
            string starttime = this.DateTimeToTimestamp(DateTime.UtcNow).ToString();
            if (long.TryParse(starttime, out long startTimestamp))
            {
                presence.startTimestamp = startTimestamp;
            }

            presence.largeImageKey = "";
            presence.largeImageText = "";
            presence.smallImageKey = "";
            presence.smallImageText = "";

            DiscordRpc.UpdatePresence(ref presence);

            this.TopMost = true;
            ToolTip toolTip1 = new ToolTip(); // Tooltips
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(killrblxButton, "Kill Roblox");
            toolTip1.SetToolTip(injectButton, "Attach to Roblox");
            toolTip1.SetToolTip(scriptsButton, "Scripts List");
            toolTip1.SetToolTip(discordButton, "Join Discord");
            toolTip1.SetToolTip(creditsButton, "Show Credits");
            listBox1.Visible = false;
            foreach (string path in Directory.GetFiles(Application.StartupPath + "\\Scripts", "*.txt", SearchOption.AllDirectories))
            {
                this.listBox1.Items.Add(Path.GetFileName(path)); // Repeat cause too lazy to put extensions into array
            }
            foreach (string path in Directory.GetFiles(Application.StartupPath + "\\Scripts", "*.lua", SearchOption.AllDirectories))
            {
                this.listBox1.Items.Add(Path.GetFileName(path));
            }
            WebClient wc = new WebClient();
            wc.Proxy = null;
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                string friendlyName = AppDomain.CurrentDomain.FriendlyName;
                bool flag2 = registryKey.GetValue(friendlyName) == null;
                if (flag2)
                {
                    registryKey.SetValue(friendlyName, 11001, RegistryValueKind.DWord);
                }
                registryKey = null;
                friendlyName = null;
            }
            catch (Exception)
            {
            }
            webBrowser1.Url = new Uri(string.Format("file:///{0}/bin/Monaco/Monaco.html", Directory.GetCurrentDirectory()));
            await Task.Delay(500);
            webBrowser1.Document.InvokeScript("SetTheme", new string[]
            {
                   "Dark" 
            });
            browserdark = true;
            addBase();
            addMath();
            addGlobalNS();
            addGlobalV();
            addGlobalF();
            webBrowser1.Document.InvokeScript("SetText", new object[]
            {
                 "-- Welcome to Artex --"
            });

        } 

        private void closeButton_Click(object sende, EventArgs e)
        {
            DiscordRpc.Shutdown();
            Environment.Exit(0); //Closes application
        }
        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // Panel1 Mousedown Draggable event
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void InjectButton_Click(object sender, EventArgs e)
        {
            Functions.Attach();
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (Functions.openfiledialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    
                    string MainText = File.ReadAllText(Functions.openfiledialog.FileName);
                    if (webBrowser1.Visible == true)
                    {
                        webBrowser1.Document.InvokeScript("SetText", new object[]
                        {
                          MainText
                        });
                    }
                    else
                    {
                        fastColoredTextBox1.Text = MainText;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }


        private void executeButton_Click(object sender, EventArgs e)
        {
            if (webBrowser1.Visible == true)
            {
                HtmlDocument document = webBrowser1.Document;
                string scriptName = "GetText";
                object[] args = new string[0];
                object obj = document.InvokeScript(scriptName, args);
                string script = obj.ToString();
                NamedPipes.Execute(script);
            }
            else
            {
                string script = fastColoredTextBox1.Text;
                NamedPipes.Execute(script);

            }
        }

        private void KillrblxButton_Click(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("RobloxPlayerBeta"))
            {
                process.Kill();

            }
        }


        private void scriptsButton_Click(object sender, EventArgs e)
        {
            if (listBox1.Visible == false)
            {
                this.Size = new System.Drawing.Size(1107, 472);
                listBox1.Visible = true;
            }
            else
            {
                this.Size = new System.Drawing.Size(850, 472);
                listBox1.Visible = false;
            }


        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (this.listBox1.SelectedItem != null)
            {
                string str = this.listBox1.SelectedItem.ToString();
                string path = Application.StartupPath + "\\Scripts\\" + str;
                string MainText = File.ReadAllText(path);
                if (webBrowser1.Visible == true)
                {
                    webBrowser1.Document.InvokeScript("SetText", new object[]
                {
                          MainText
                });
                }
                else
                {
                    fastColoredTextBox1.Text = MainText;
                }
            }
        }

        private void DiscordButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/wuDgeD");
        }

        private void minimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void CreditsButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Credits: \n JaydenV3 - Developer \n Recreation - UI Developer");
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();

            settings.Show(this);

        }

        private void SwitchEditorbtn_Click(object sender, EventArgs e)
        {
            if (webBrowser1.Visible == true)
            {
                webBrowser1.Visible = false;
                fastColoredTextBox1.Visible = true;
                HtmlDocument document = webBrowser1.Document;
                string scriptName = "GetText";
                object[] args = new string[0];
                object obj = document.InvokeScript(scriptName, args);
                string script = obj.ToString();
                fastColoredTextBox1.Text = script;
            }
            else
            {
                webBrowser1.Visible = true;
                fastColoredTextBox1.Visible = false;
                string script = fastColoredTextBox1.Text;
                webBrowser1.Document.InvokeScript("SetText", new object[]
               {
                 script
               });
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            if (webBrowser1.Visible == true)
            {
                webBrowser1.Document.InvokeScript("SetText", new object[]
                {
                 "" //Set text to nothing
                });
            }
            else
            {
                fastColoredTextBox1.Text = "";
            }
        }

        private void hubbtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hub undergoing development.");
           // Hub hub = new Hub();
           //  hub.StartPosition = FormStartPosition.CenterParent;

           //  hub.Show(this);
        }

    }
}



