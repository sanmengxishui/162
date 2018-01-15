using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPTCG.Spectrometer
{
    public partial class WinAssignSpec : Form
    {
        SpectManager myMgr;
        int ModIdx = -1;
        SpectType selType = SpectType.NoSpectrometer;
        public WinAssignSpec(SpectManager specMgr, int selIdx)
        {
            InitializeComponent();
            myMgr = specMgr;
            ModIdx = selIdx;
        }

        private void WinAssignSpec_Load(object sender, EventArgs e)
        {
            ModNameEB.Text = myMgr.SpecList[ModIdx].Name;
            this.Text = myMgr.SpecList[ModIdx].Name;
            switch (myMgr.SpecList[ModIdx].specType)
            {
                case SpectType.NoSpectrometer:
                    NotRB.Checked = true;
                    break;
                case SpectType.Avantes:
                    AvantesRB.Checked = true;
                    break;
                case SpectType.Maya:
                    MayaRB.Checked = true;
                    break;
                case SpectType.CAS140:
                    CAS140RB.Checked = true;
                    break;
            }
            UpdateSpecList(myMgr.SpecList[ModIdx].specType);
        }

        private void UpdateSpecList(SpectType myType)
        {
            specLB.Items.Clear();
            List<string> myList = new List<string>();
            myList = myMgr.GetSpectrometerList(myType);

            for (int i = 0; i < myList.Count; i++)
                specLB.Items.Add(myList[i]);

            selType = myType;
        }

        private void AvantesRB_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSpecList(SpectType.Avantes);
        }

        private void MayaRB_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSpecList(SpectType.Maya);
        }

        private void NotRB_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSpecList(SpectType.NoSpectrometer);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selType == SpectType.NoSpectrometer)
            {
                Close();                
            }
            if (specLB.Items.Count == 0)
                return;
            if (specLB.SelectedIndex == -1)
                return;
            myMgr.SpecList[ModIdx].specType = selType;
            myMgr.SpecList[ModIdx].serial = specLB.SelectedItem.ToString();
            myMgr.SpecList[ModIdx].Idx = specLB.SelectedIndex;
            Close();
        }

        private void CAS140RB_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSpecList(SpectType.CAS140);
        }
    }
}
