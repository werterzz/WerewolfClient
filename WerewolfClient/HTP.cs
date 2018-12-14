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
        //
        private void Werewolf_btn_Hove(object sender, EventArgs e)
        {
            button3.Visible = true;
        }

        private void Werewolf_btn_Leave(object sender, EventArgs e)
        {
            button3.Visible = false;
        }
        //

        //
        private void Vilager_btn_Hove(object sender, EventArgs e)
        {
            button13.Visible = true;
        }

        private void Vilager_btn_Leave(object sender, EventArgs e)
        {
            button13.Visible = false;
        }
        //

        //
        private void AuraSeer_btn_Hove(object sender, EventArgs e)
        {
            button12.Visible = true;
        }

        private void AuraSeer_btn_Leave(object sender, EventArgs e)
        {
            button12.Visible = false;
        }
        //


        //
        private void BodyGuard_btn_Hove(object sender, EventArgs e)
        {
            button11.Visible = true;
        }

        private void BodyGuard_btn_Leave(object sender, EventArgs e)
        {
            button11.Visible = false;
        }
        //

        //
        private void Doctor_btn_Hove(object sender, EventArgs e)
        {
            button10.Visible = true;
        }

        private void Doctor_btn_Leave(object sender, EventArgs e)
        {
            button10.Visible = false;
        }
        //

        //
        private void fool_btn_Hove(object sender, EventArgs e)
        {
            button9.Visible = true;
        }

        private void fool_btn_Leave(object sender, EventArgs e)
        {
            button9.Visible = false;
        }
        //

        //
        private void Gunner_btn_Hove(object sender, EventArgs e)
        {
            button8.Visible = true;
        }

        private void Gunner_btn_Leave(object sender, EventArgs e)
        {
            button8.Visible = false;
        }
        //

        //
        private void Jailer_btn_Hove(object sender, EventArgs e)
        {
            button7.Visible = true;
        }

        private void Jailer_btn_Leave(object sender, EventArgs e)
        {
            button7.Visible = false;
        }
        //

        //
        private void medium_btn_Hove(object sender, EventArgs e)
        {
            button6.Visible = true;
        }

        private void medium_btn_Leave(object sender, EventArgs e)
        {
            button6.Visible = false;
        }
        //

        //
        private void Priest_btn_Hove(object sender, EventArgs e)
        {
            button5.Visible = true;
        }

        private void Priest_btn_Leave(object sender, EventArgs e)
        {
            button5.Visible = false;
        }
        //

        //
        private void Seer_btn_Hove(object sender, EventArgs e)
        {
            button4.Visible = true;
        }

        private void Seer_btn_Leave(object sender, EventArgs e)
        {
            button4.Visible = false;
        }
        //

        //
        private void SerailKiller_btn_Hove(object sender, EventArgs e)
        {
            button16.Visible = true;
        }

        private void SerailKiller_btn_Leave(object sender, EventArgs e)
        {
            button16.Visible = false;
        }
        //

        //
        private void WolfSeer_btn_Hove(object sender, EventArgs e)
        {
            button15.Visible = true;
        }

        private void WolfSeer_btn_Leave(object sender, EventArgs e)
        {
            button15.Visible = false;
        }
        //

        //
        private void WolfShaman_btn_Hove(object sender, EventArgs e)
        {
            button14.Visible = true;
        }

        private void WolfShaman_btn_Leave(object sender, EventArgs e)
        {
            button14.Visible = false;
        }
        //

        //
        private void AlphaWolf_btn_Hove(object sender, EventArgs e)
        {
            button17.Visible = true;
        }

        private void AlphaWolf_btn_Leave(object sender, EventArgs e)
        {
            button17.Visible = false;
        }
        //

    }
}