using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WerewolfClient
{
    
    public partial class HTP : Form
    {

        private Form _menu;
        public HTP(Form menu)
        {
            InitializeComponent();

            _menu = menu;
        }

        private void HTP_Load(object sender, EventArgs e)
        {
            WinApI.AnimateWindow(this.Handle, 1500, WinApI.BLEND);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            _menu.Visible = true;
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity > 0.0)
            {
                this.Opacity -= 0.1;
            }
            else
            {
                timer1.Stop();
                Application.Exit();
            }
        }

        private void Werewolf_btn_Click(object sender, EventArgs e)
        {
            if (button3.Visible == false)
            {
                button3.Visible = true;
            }
            else
            {
                button3.Visible = false;
            }
        }

        private void Vilager_btn_Click(object sender, EventArgs e)
        {
            if (button13.Visible == false)
            {
                button13.Visible = true;
            }
            else
            {
                button13.Visible = false;
            }
        }

        private void AuraSeer_btn_Click(object sender, EventArgs e)
        {
            if (button12.Visible == false)
            {
                button12.Visible = true;
            }
            else
            {
                button12.Visible = false;
            }
        }

        private void BodyGuard_btn_Click(object sender, EventArgs e)
        {
            if (button11.Visible == false)
            {
                button11.Visible = true;
            }
            else
            {
                button11.Visible = false;
            }
        }

        private void Doctor_btn_Click(object sender, EventArgs e)
        {
            if (button10.Visible == false)
            {
                button10.Visible = true;
            }
            else
            {
                button10.Visible = false;
            }
        }

        private void fool_btn_Click(object sender, EventArgs e)
        {
            if (button9.Visible == false)
            {
                button9.Visible = true;
            }
            else
            {
                button9.Visible = false;
            }
        }

        private void Gunner_btn_Click(object sender, EventArgs e)
        {
            if (button8.Visible == false)
            {
                button8.Visible = true;
            }
            else
            {
                button8.Visible = false;
            }
        }

        private void Jailer_btn_Click(object sender, EventArgs e)
        {
            if (button7.Visible == false)
            {
                button7.Visible = true;
            }
            else
            {
                button7.Visible = false;
            }
        }

        private void medium_btn_Click(object sender, EventArgs e)
        {
            if (button6.Visible == false)
            {
                button6.Visible = true;
            }
            else
            {
                button6.Visible = false;
            }
        }

        private void Priest_btn_Click(object sender, EventArgs e)
        {
            if (button5.Visible == false)
            {
                button5.Visible = true;
            }
            else
            {
                button5.Visible = false;
            }
        }

        private void Seer_btn_Click(object sender, EventArgs e)
        {
            if (button4.Visible == false)
            {
                button4.Visible = true;
            }
            else
            {
                button4.Visible = false;
            }
        }

        private void SerailKiller_btn_Click(object sender, EventArgs e)
        {
            if (button16.Visible == false)
            {
                button16.Visible = true;
            }
            else
            {
                button16.Visible = false;
            }
        }

        private void WolfSeer_btn_Click(object sender, EventArgs e)
        {
            if (button15.Visible == false)
            {
                button15.Visible = true;
            }
            else
            {
                button15.Visible = false;
            }
        }

        private void WolfShaman_btn_Click(object sender, EventArgs e)
        {
            if (button14.Visible == false)
            {
                button14.Visible = true;
            }
            else
            {

                button14.Visible = false;
            }
        }

        private void AlphaWolf_btn_Click(object sender, EventArgs e)
        {
            if (button17.Visible == false)
            {
                button17.Visible = true;
            }
            else
            {
                button17.Visible = false;
            }
        }


    }
}