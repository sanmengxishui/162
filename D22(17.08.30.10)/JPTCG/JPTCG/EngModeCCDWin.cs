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
    public partial class EngModeCCDWin : Form
    {
        public double X = 0.0;
        public double Y = 0.0;
        public double Ang = 0.0;
        string myTitle = "CCD";
        public EngModeCCDWin(string Title)
        {
            myTitle = Title;
            InitializeComponent();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            X = double.Parse(XEB.Text);
            Y = double.Parse(YEB.Text);
            Ang = double.Parse(AngEB.Text);
            Close();
        }

        private void EngModeCCDWin_Load(object sender, EventArgs e)
        {
            this.Text = myTitle;
            XEB.Text = X.ToString("F1");
            YEB.Text = Y.ToString("F1");
            AngEB.Text = Ang.ToString("F1");
        }
    }
}
