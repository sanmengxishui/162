using JPTCG.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPTCG
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (label2.Text.Length > 30)
                label2.Text = "Initializing";
            else
                label2.Text = label2.Text + ".";
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            label2.Text = "Initializing";
            VersionLbl.Text = Para.SWVersion;
            timer1.Start();
        }

        private void SplashScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
