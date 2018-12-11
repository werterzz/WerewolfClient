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
    public partial class Menu : Form,View
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
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _mainform.Visible = true;
            this.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HTP htp = new HTP();
            htp.ShowDialog();
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }
    }
}
