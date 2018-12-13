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
    public partial class Menu : Form, View
    {

        private WerewolfController controller;
        private Form _mainform;
        
        public Menu(Form mainfrom)
        {
            InitializeComponent();
            _mainform = mainfrom;
           
           
        }

        public void Notify(Model m)
        {
            throw new NotImplementedException();
        }

        public void setController(Controller c)
        {
            controller = (WerewolfController)c;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            this.Visible = false;
            _mainform.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            HTP htp = new HTP(this);
            this.Visible = false;
            htp.Visible = true;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            WinApI.AnimateWindow(this.Handle, 1500, WinApI.BLEND);
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
    }
   
}