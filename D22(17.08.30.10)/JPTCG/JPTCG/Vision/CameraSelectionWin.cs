using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPTCG.Vision
{
    public partial class CameraSelectionWin : Form
    {
        HalconVision myHalcon = null;
        public CameraSelectionWin(HalconVision halVis)
        {
            InitializeComponent();
            myHalcon = halVis;
            List<string> camList = myHalcon.GetBaslerCamSerialList(); 
            CamLB.Items.Clear();
            for (int i = 0; i < camList.Count; i++)
                CamLB.Items.Add(camList[i]);
            
            AssignBtn.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AssignBtn_Click(object sender, EventArgs e)
        {
            if (CamLB.SelectedIndex != -1)
                myHalcon.AssignBaslerCamera(CamLB.SelectedItem.ToString());
            Close();
        }

        private void CamLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AssignBtn.Enabled == false)
                AssignBtn.Enabled = true;
        }
    }
}
