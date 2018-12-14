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
    public partial class register : Form, View
    {
        WerewolfController controller;
        private Login _login;
        public register(Login login,string sever)
        {
            InitializeComponent();
            _login = login;
            TBServer.Text = sever;
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
            _login.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == textBox2.Text)
            {
                WerewolfController c = WerewolfController.GetInstance();
                WerewolfCommand wcmd = new WerewolfCommand();
                wcmd.Action = WerewolfCommand.CommandEnum.SignUp;
                wcmd.Payloads = new Dictionary<string, string>() { { "Login", textBox1.Text }, { "Password", textBox2.Text }, { "Server", TBServer.Text } };
                c.ActionPerformed(wcmd);
            }
            else
            {
                MessageBox.Show("Please make sure your passwords match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void register_Load(object sender, EventArgs e)
        {
            WinApI.AnimateWindow(this.Handle, 2000, WinApI.BLEND);
        }

        private void register_Load_1(object sender, EventArgs e)
        {
            WinApI.AnimateWindow(this.Handle, 2000, WinApI.BLEND);
        }
    }
}