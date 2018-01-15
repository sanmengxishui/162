using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JPTCG.Common;
using System.Threading;

namespace JPTCG.Motion
{
    public partial class JogPad : UserControl
    {

        public delegate void OnApplyClicked(double myPos);
        public event OnApplyClicked OnApplyClick;
        DeltaMotionMgr myMgr;
        Axislist myAxis;
        double myTchPos = 0.0;
        bool revJP = false;

        public JogPad(DeltaMotionMgr motionMgr, Panel parentPnl)
        {
            InitializeComponent();
            this.Parent = parentPnl;
            JPGB.Enabled = false;
            myMgr = motionMgr;
        }

        ~JogPad()
        {
            JPTimer.Enabled = false;
        }

        public void Assign(Axislist jpAxis, double TchPos, JogDir myDir, bool MyrevJP)
        {
            JPGB.Enabled = true;
            myAxis = jpAxis;
            JPGB.Text = Helper.GetEnumDescription(myAxis);
            //JPGB.Text = myAxis.ToString();
            revJP = MyrevJP;
           

            if (myDir == JogDir.LeftRight)
            {
                UpBtn.Visible = false;
                DownBtn.Visible = false;
                LeftBtn.Visible = true;
                RightBtn.Visible = true;
            }
            else
            {
                UpBtn.Visible = true;
                DownBtn.Visible = true;
                LeftBtn.Visible = false;
                RightBtn.Visible = false;
            }
            myTchPos = TchPos;
            tchEB.Text = TchPos.ToString("0.000");

            if (!JPTimer.Enabled)
                JPTimer.Enabled = true;
        }
        public void DisableTimer()
        {
            JPTimer.Enabled = false;
        }

        private void JPTimer_Tick(object sender, EventArgs e)
        {
            curPosEB.Text = myMgr.GetPos((ushort)myAxis).ToString("0.000");

            //PEL
            if (!myMgr.PEL((ushort)myAxis))
            {
                if (PelLbl.BackColor != Color.Black)
                    PelLbl.BackColor = Color.Black;
            }
            else
            {
                if (PelLbl.BackColor != Color.Red)
                    PelLbl.BackColor = Color.Red;
            }
            //NEL
            if (!myMgr.NEL((ushort)myAxis))
            {
                if (NelLbl.BackColor != Color.Black)
                    NelLbl.BackColor = Color.Black;
            }
            else
            {
                if (NelLbl.BackColor != Color.Red)
                    NelLbl.BackColor = Color.Red;
            }

        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            tchEB.Text = curPosEB.Text;
            if (OnApplyClick != null)
                OnApplyClick(double.Parse(curPosEB.Text));
            myTchPos = double.Parse(curPosEB.Text); 
        }

        private void GoToBtn_Click(object sender, EventArgs e)
        {
            myMgr.MoveTo((ushort)myAxis, myTchPos);
            myMgr.WaitAxisStop((ushort)myAxis);
        }

        private void UpBtn_Click(object sender, EventArgs e)
        {
            double steps = double.Parse(stepEB.Text);
            if (revJP)
                myMgr.StepMove((ushort)myAxis, steps);
            else
                myMgr.StepMove((ushort)myAxis, -steps);
            myMgr.WaitAxisStop((ushort)myAxis);
        }

        private void DownBtn_Click(object sender, EventArgs e)
        {
            double steps = double.Parse(stepEB.Text);
            if (revJP)
                myMgr.StepMove((ushort)myAxis, -steps);
            else
                myMgr.StepMove((ushort)myAxis, steps);
            myMgr.WaitAxisStop((ushort)myAxis);
        }

        private void LeftBtn_Click(object sender, EventArgs e)
        {
            double steps = double.Parse(stepEB.Text);
            if (revJP)
                myMgr.StepMove((ushort)myAxis, steps);
            else
                myMgr.StepMove((ushort)myAxis, -steps);
            myMgr.WaitAxisStop((ushort)myAxis);
        }

        private void RightBtn_Click(object sender, EventArgs e)
        {
            double steps = double.Parse(stepEB.Text);
            if (revJP)
                myMgr.StepMove((ushort)myAxis, -steps);
            else
                myMgr.StepMove((ushort)myAxis, steps);
            myMgr.WaitAxisStop((ushort)myAxis);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //myMgr.Homing((ushort)myAxis, 1);
            myMgr.MoveTo((ushort)myAxis, 0,0);
            myMgr.WaitAxisStop((ushort)myAxis);
            Thread.Sleep(100);
            if (myMgr.NEL((ushort)myAxis))
            {
                myMgr.Homing((ushort)myAxis, 1);
                myMgr.WaitHomeDone((ushort)myAxis);
            }

            myMgr.SetPosition((ushort)myAxis, 0);
        }
    }
}
