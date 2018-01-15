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

namespace JPTCG.Motion
{
    public partial class WinMotionSettings : Form
    {
        DeltaMotionMgr myMgr;
        public WinMotionSettings(DeltaMotionMgr mMgr)
        {
            InitializeComponent();
            myMgr = mMgr;
        }

        private void WinMotionSettings_Load(object sender, EventArgs e)
        {
            AxisLB.Items.Clear();
            for (int i =0;i<myMgr.AxisPara.Count;i++)
                AxisLB.Items.Add(Helper.GetEnumDescription(myMgr.AxisPara[i].AxisIdx));

            AxisLB.SelectedIndex = 0;
            UpdateUI(0);
        }
        private void UpdateUI(int SelectedIdx)
        {
            SettingGB.Text = Helper.GetEnumDescription(myMgr.AxisPara[SelectedIdx].AxisIdx);
            AxisNumEB.Text = ((int)myMgr.AxisPara[SelectedIdx].AxisIdx).ToString();
            HomeSpEB.Text = myMgr.AxisPara[SelectedIdx].HomeSpeed.ToString();
            RunSpEB.Text = myMgr.AxisPara[SelectedIdx].RunSpeed.ToString();
            MScaleEB.Text = myMgr.AxisPara[SelectedIdx].MotorScale.ToString();
            CBIsServo.Checked = myMgr.AxisPara[SelectedIdx].IsServoMotor;
        }

        private void AxisLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI(AxisLB.SelectedIndex);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            int SelectedIdx = AxisLB.SelectedIndex;
            myMgr.AxisPara[SelectedIdx].HomeSpeed = int.Parse(HomeSpEB.Text);
            myMgr.AxisPara[SelectedIdx].RunSpeed = int.Parse(RunSpEB.Text);
            myMgr.AxisPara[SelectedIdx].MotorScale = int.Parse(MScaleEB.Text);
            myMgr.AxisPara[SelectedIdx].IsServoMotor = CBIsServo.Checked;
            myMgr.SaveMotionSettings();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
