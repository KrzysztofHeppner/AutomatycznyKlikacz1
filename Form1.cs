using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace AutomatycznyKlikacz1
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        public Form1()
        {
            InitializeComponent();
            timer1.Start();
            timer2.Start();
            Thread mainThread = Thread.CurrentThread;
            mainThread.Name = "Main Thread";
        }

        int mnoznik = 1;
        bool numPad0 = false; bool czekanieNumPad0 = false;
        bool numPad1 = false; bool czekanieNumPad1 = false;
        bool numPad2 = false; bool czekanieNumPad2 = false;
        bool numPad3 = false; bool czekanieNumPad3 = false;
        string znak = " ";


        public void Klikanie(string a)
        {
            for (int i = 0; i < mnoznik; i++)
            {
                switch (a)
                {
                    case "NumPad0":
                        uint XL = (uint)System.Windows.Forms.Cursor.Position.X;
                        uint YL = (uint)System.Windows.Forms.Cursor.Position.Y;
                        mouse_event(0x02 | 0x04, XL, YL, 0, 0);
                        break;
                    case "NumPad1":
                        uint XR = (uint)System.Windows.Forms.Cursor.Position.X;
                        uint YR = (uint)System.Windows.Forms.Cursor.Position.Y;
                        mouse_event(0x06 | 0x08, XR, YR, 0, 0);
                        break;
                    case "NumPad2":
                        SendKeys.Send("{ENTER}");
                        break;
                    case "NumPad3":
                        try
                        {
                            SendKeys.Send(znak);
                        }
                        catch
                        {
                            znak = " ";
                            MessageBox.Show("Podany znak jest błędny, zmieniono na SPACE", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public void Timer1_Tick(object sender, EventArgs e)
        {
            if (numPad0)
            {
                Klikanie("NumPad0");
            }
            if (numPad1)
            {
                Klikanie("NumPad1");
            }
            if (numPad2)
            {
                Klikanie("NumPad2");
            }
            if (numPad3)
            {
                Klikanie("NumPad3");
            }
        }

        public void Timer2_Tick(object sender, EventArgs e)
        {
            //Numpad 0
            if (((Keyboard.GetKeyStates(Key.NumPad0) & KeyStates.Down) > 0) && (czekanieNumPad0 == false))
            {
                numPad0 = !numPad0;
                czekanieNumPad0 = true;
                //if((textBox1.Text == "6") && (thread1.IsAlive == false))
                //    thread1.Start();
            }
            else if ((Keyboard.GetKeyStates(Key.NumPad0) & KeyStates.Down) == 0)
            {
                czekanieNumPad0 = false;
                //if ((textBox1.Text == "6") && (thread1.IsAlive == true))
                //    thread1.Interrupt();
            }

            //Numpad 1
            if (((Keyboard.GetKeyStates(Key.NumPad1) & KeyStates.Down) > 0) && (czekanieNumPad1 == false))
            {
                numPad1 = !numPad1;
                czekanieNumPad1 = true;
            }
            else if ((Keyboard.GetKeyStates(Key.NumPad1) & KeyStates.Down) == 0)
            {
                czekanieNumPad1 = false;
            }

            //Numpad 2
            if (((Keyboard.GetKeyStates(Key.NumPad2) & KeyStates.Down) > 0) && (czekanieNumPad2 == false))
            {
                numPad2 = !numPad2;
                czekanieNumPad2 = true;
            }
            else if ((Keyboard.GetKeyStates(Key.NumPad2) & KeyStates.Down) == 0)
            {
                czekanieNumPad2 = false;
            }

            //Numpad 3
            if (((Keyboard.GetKeyStates(Key.NumPad3) & KeyStates.Down) > 0) && (czekanieNumPad3 == false))
            {
                numPad3 = !numPad3;
                czekanieNumPad3 = true;
            }
            else if ((Keyboard.GetKeyStates(Key.NumPad3) & KeyStates.Down) == 0)
            {
                czekanieNumPad3 = false;
            }

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                int.TryParse(textBox1.Text, out int otrzymanyMnoznik);
                if ((otrzymanyMnoznik > 0) && otrzymanyMnoznik < 16)
                {
                    mnoznik = otrzymanyMnoznik;
                    label8.Text = "razy około 63 = " + mnoznik * 63;
                    SendKeys.Send("{TAB}");
                }
                else
                {
                    MessageBox.Show("za duża lub za mała liczba", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mnoznik = 1;
                    textBox1.Text = "1";
                }
            }
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys?view=windowsdesktop-5.0#remarks");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text[0].ToString() != "{")
                {
                    comboBox1.Text = "{" + comboBox1.Text;
                }
                if (comboBox1.Text[comboBox1.Text.Length - 1].ToString() != "}")
                {
                    comboBox1.Text = comboBox1.Text + "}";
                }
                znak = comboBox1.Text;
            }
            catch { }
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            comboBox1_SelectedIndexChanged(null, null);
        }

        private void buttonOnOff_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == false)
            {
                timer1.Start();
                timer2.Start();
                buttonOnOff.Text = "Wyłącz";
            }
            else
            {
                timer1.Stop();
                timer2.Stop();
                buttonOnOff.Text = "Włącz";
            }
        }
    }
}
