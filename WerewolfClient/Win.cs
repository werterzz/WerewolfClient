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
    public partial class Win : Form
    {
        private string w;
        public Win(string outcome)
        {
            InitializeComponent();
            w = outcome;
            this.label1.Text = w;
            switch (w)
            {
                case "Werewolfwin":
                    pictureBox1.Image = Properties.Resources.Icon_werewolf;
                    break;
            }
        }
    }
}
