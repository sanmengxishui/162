using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JPTCG;

namespace JPTCG.Motion
{
    public partial class IOInterface : UserControl
    {
        public DeltaMotionMgr myMgr;
        public IOInterface(DeltaMotionMgr myMotion, Panel parentPnl)
        {
            InitializeComponent();
            myMgr = myMotion;
            
            this.Parent = parentPnl;
            //IOTimer.Enabled = true;
            //InitUI();
        }
        ~IOInterface()
        {
            //IOTimer.Enabled = false;
        }
        private void InitUI()        
        {             
            //OutputGB
            int outputCnt = Enum.GetNames(typeof(OutputIOlist)).Length;
            for (int i = 0; i < outputCnt; i++)
            {
                Label myLbl = new Label();
                myLbl.Parent = OutputGB;
                myLbl.Left = 10;
                myLbl.Top = 15 + (i * 20);
                myLbl.Height = 15;
                myLbl.Width = 15;
                myLbl.Image = JPTCG.Properties.Resources._1_HomeEnable;
            }

        }
        public void SetIOTimer(bool val)
        {
            IOTimer.Enabled = val;
        }
        private void IOTimer_Tick(object sender, EventArgs e)
        {
            //Output
            if (myMgr.ReadIOOut((ushort)OutputIOlist.LampGreen))
            {
                if (GreenLampLbl.BackColor != Color.Lime)
                    GreenLampLbl.BackColor = Color.Lime;
            }
            else
            {
                if (GreenLampLbl.BackColor != Color.Black)
                    GreenLampLbl.BackColor = Color.Black;
            }

            if (myMgr.ReadIOOut((ushort)OutputIOlist.LampAmber))
            {
                if (AmberLampLbl.BackColor != Color.Lime)
                    AmberLampLbl.BackColor = Color.Lime;
            }
            else
            {
                if (AmberLampLbl.BackColor != Color.Black)
                    AmberLampLbl.BackColor = Color.Black;
            }

            if (myMgr.ReadIOOut((ushort)OutputIOlist.LampRed))
            {
                if (RedLampLbl.BackColor != Color.Lime)
                    RedLampLbl.BackColor = Color.Lime;
            }
            else
            {
                if (RedLampLbl.BackColor != Color.Black)
                    RedLampLbl.BackColor = Color.Black;
            }

            //if (myMgr.ReadIOOut((ushort)OutputIOlist.Buzzer))
            //{
            //    if (BuzzerLbl.BackColor != Color.Lime)
            //        BuzzerLbl.BackColor = Color.Lime;
            //}
            //else
            //{
            //    if (BuzzerLbl.BackColor != Color.Black)
            //        BuzzerLbl.BackColor = Color.Black;
            //}

            if (myMgr.ReadIOOut((ushort)OutputIOlist.RI1Vac1))
            {
                if (RI1Vac1Lbl.BackColor != Color.Lime)
                    RI1Vac1Lbl.BackColor = Color.Lime;
            }
            else
            {
                if (RI1Vac1Lbl.BackColor != Color.Black)
                    RI1Vac1Lbl.BackColor = Color.Black;
            }

            if (myMgr.ReadIOOut((ushort)OutputIOlist.RI1Vac2))
            {
                if (RI1Vac2Lbl.BackColor != Color.Lime)
                    RI1Vac2Lbl.BackColor = Color.Lime;
            }
            else
            {
                if (RI1Vac2Lbl.BackColor != Color.Black)
                    RI1Vac2Lbl.BackColor = Color.Black;
            }

            if (myMgr.ReadIOOut((ushort)OutputIOlist.RI2Vac1))
            {
                if (RI2Vac1Lbl.BackColor != Color.Lime)
                    RI2Vac1Lbl.BackColor = Color.Lime;
            }
            else
            {
                if (RI2Vac1Lbl.BackColor != Color.Black)
                    RI2Vac1Lbl.BackColor = Color.Black;
            }

            if (myMgr.ReadIOOut((ushort)OutputIOlist.RI2Vac2))
            {
                if (RI2Vac2Lbl.BackColor != Color.Lime)
                    RI2Vac2Lbl.BackColor = Color.Lime;
            }
            else
            {
                if (RI2Vac2Lbl.BackColor != Color.Black)
                    RI2Vac2Lbl.BackColor = Color.Black;
            }

            if (myMgr.ReadIOOut((ushort)OutputIOlist.RI3Vac1))
            {
                if (RI3Vac1Lbl.BackColor != Color.Lime)
                    RI3Vac1Lbl.BackColor = Color.Lime;
            }
            else
            {
                if (RI3Vac1Lbl.BackColor != Color.Black)
                    RI3Vac1Lbl.BackColor = Color.Black;
            }

            if (myMgr.ReadIOOut((ushort)OutputIOlist.RI3Vac2))
            {
                if (RI3Vac2Lbl.BackColor != Color.Lime)
                    RI3Vac2Lbl.BackColor = Color.Lime;
            }
            else
            {
                if (RI3Vac2Lbl.BackColor != Color.Black)
                    RI3Vac2Lbl.BackColor = Color.Black;
            }

            if (myMgr.ReadIOOut((ushort)OutputIOlist.RI4Vac1))
            {
                if (RI4Vac1Lbl.BackColor != Color.Lime)
                    RI4Vac1Lbl.BackColor = Color.Lime;
            }
            else
            {
                if (RI4Vac1Lbl.BackColor != Color.Black)
                    RI4Vac1Lbl.BackColor = Color.Black;
            }

            if (myMgr.ReadIOOut((ushort)OutputIOlist.RI4Vac2))
            {
                if (RI4Vac2Lbl.BackColor != Color.Lime)
                    RI4Vac2Lbl.BackColor = Color.Lime;
            }
            else
            {
                if (RI4Vac2Lbl.BackColor != Color.Black)
                    RI4Vac2Lbl.BackColor = Color.Black;
            }

            if (myMgr.ReadIOOut((ushort)OutputIOlist.Cam1Light))
            {
                if (CamLight1Lbl.BackColor != Color.Lime)
                    CamLight1Lbl.BackColor = Color.Lime;
            }
            else
            {
                if (CamLight1Lbl.BackColor != Color.Black)
                    CamLight1Lbl.BackColor = Color.Black;
            }

            if (myMgr.ReadIOOut((ushort)OutputIOlist.Cam2Light))
            {
                if (CamLight2Lbl.BackColor != Color.Lime)
                    CamLight2Lbl.BackColor = Color.Lime;
            }
            else
            {
                if (CamLight2Lbl.BackColor != Color.Black)
                    CamLight2Lbl.BackColor = Color.Black;
            }

            if (myMgr.ReadIOOut((ushort)OutputIOlist.SpectrumLS))
            {
                if (SpectrumLSLbl.BackColor != Color.Lime)
                    SpectrumLSLbl.BackColor = Color.Lime;
            }
            else
            {
                if (SpectrumLSLbl.BackColor != Color.Black)
                    SpectrumLSLbl.BackColor = Color.Black;
            }

            if (myMgr.ReadIOOut((ushort)OutputIOlist.MchLight))
            {
                if (mchLightLbl.BackColor != Color.Lime)
                    mchLightLbl.BackColor = Color.Lime;
            }
            else
            {
                if (mchLightLbl.BackColor != Color.Black)
                    mchLightLbl.BackColor = Color.Black;
            }
            

            //Input
            if (myMgr.ReadIOIn((ushort)InputIOlist.BtnLeft))
            {
                if (LeftBtnLbl.BackColor != Color.Lime)
                    LeftBtnLbl.BackColor = Color.Lime;
            }
            else
            {
                if (LeftBtnLbl.BackColor != Color.Black)
                    LeftBtnLbl.BackColor = Color.Black;
            }
            if (myMgr.ReadIOIn((ushort)InputIOlist.BtnRight))
            {
                if (RightBtnLbl.BackColor != Color.Lime)
                    RightBtnLbl.BackColor = Color.Lime;
            }
            else
            {
                if (RightBtnLbl.BackColor != Color.Black)
                    RightBtnLbl.BackColor = Color.Black;
            }
            if (myMgr.ReadIOIn((ushort)InputIOlist.BtnStop))
            {
                if (StopBtnLbl.BackColor != Color.Lime)
                    StopBtnLbl.BackColor = Color.Lime;
            }
            else
            {
                if (StopBtnLbl.BackColor != Color.Black)
                    StopBtnLbl.BackColor = Color.Black;
            }
            if (myMgr.ReadIOIn((ushort)InputIOlist.BtnHome))
            {
                if (HomeBtnLbl.BackColor != Color.Lime)
                    HomeBtnLbl.BackColor = Color.Lime;
            }
            else
            {
                if (HomeBtnLbl.BackColor != Color.Black)
                    HomeBtnLbl.BackColor = Color.Black;
            }
            if (myMgr.ReadIOIn((ushort)InputIOlist.BtnEMO))
            { 
                if (EmoLbl.BackColor != Color.Lime)
                    EmoLbl.BackColor = Color.Lime;
            }
            else
            {
                if (EmoLbl.BackColor != Color.Black)
                    EmoLbl.BackColor = Color.Black;
            }
            if (myMgr.ReadIOIn((ushort)InputIOlist.SafetySensor))
            {
                if (SafetyLbl.BackColor != Color.Lime)
                    SafetyLbl.BackColor = Color.Lime;
            }
            else
            {
                if (SafetyLbl.BackColor != Color.Black)
                    SafetyLbl.BackColor = Color.Black;
            }
            if (myMgr.ReadIOIn((ushort)InputIOlist.DoorSensor))
            {
                if (doorLbl.BackColor != Color.Lime)
                    doorLbl.BackColor = Color.Lime;
            }
            else
            {
                if (doorLbl.BackColor != Color.Black)
                    doorLbl.BackColor = Color.Black;
            }
            if (myMgr.ReadIOIn((ushort)InputIOlist.AirPressure))
            {
                if (AirLbl.BackColor != Color.Lime)
                    AirLbl.BackColor = Color.Lime;
            }
            else
            {
                if (AirLbl.BackColor != Color.Black)
                    AirLbl.BackColor = Color.Black;
            }
            //if (myMgr.ReadIOIn((ushort)InputIOlist.RI1Vac1))
            //{
            //    if (RI1Vac1InputLbl.BackColor != Color.Lime)
            //        RI1Vac1InputLbl.BackColor = Color.Lime;
            //}
            //else
            //{
            //    if (RI1Vac1InputLbl.BackColor != Color.Black)
            //        RI1Vac1InputLbl.BackColor = Color.Black;
            //}

            //if (myMgr.ReadIOIn((ushort)InputIOlist.RI1Vac2))
            //{
            //    if (RI1Vac2InputLbl.BackColor != Color.Lime)
            //        RI1Vac2InputLbl.BackColor = Color.Lime;
            //}
            //else
            //{
            //    if (RI1Vac2InputLbl.BackColor != Color.Black)
            //        RI1Vac2InputLbl.BackColor = Color.Black;
            //}

            //if (myMgr.ReadIOIn((ushort)InputIOlist.RI2Vac1))
            //{
            //    if (RI2Vac1InputLbl.BackColor != Color.Lime)
            //        RI2Vac1InputLbl.BackColor = Color.Lime;
            //}
            //else
            //{
            //    if (RI2Vac1InputLbl.BackColor != Color.Black)
            //        RI2Vac1InputLbl.BackColor = Color.Black;
            //}

            //if (myMgr.ReadIOIn((ushort)InputIOlist.RI2Vac2))
            //{
            //    if (RI2Vac2InputLbl.BackColor != Color.Lime)
            //        RI2Vac2InputLbl.BackColor = Color.Lime;
            //}
            //else
            //{
            //    if (RI2Vac2InputLbl.BackColor != Color.Black)
            //        RI2Vac2InputLbl.BackColor = Color.Black;
            //}

            //if (myMgr.ReadIOIn((ushort)InputIOlist.RI3Vac1))
            //{
            //    if (RI3Vac1InputLbl.BackColor != Color.Lime)
            //        RI3Vac1InputLbl.BackColor = Color.Lime;
            //}
            //else
            //{
            //    if (RI3Vac1InputLbl.BackColor != Color.Black)
            //        RI3Vac1InputLbl.BackColor = Color.Black;
            //}

            //if (myMgr.ReadIOIn((ushort)InputIOlist.RI3Vac2))
            //{
            //    if (RI3Vac2InputLbl.BackColor != Color.Lime)
            //        RI3Vac2InputLbl.BackColor = Color.Lime;
            //}
            //else
            //{
            //    if (RI3Vac2InputLbl.BackColor != Color.Black)
            //        RI3Vac2InputLbl.BackColor = Color.Black;
            //}

            //if (myMgr.ReadIOIn((ushort)InputIOlist.RI4Vac1))
            //{
            //    if (RI4Vac1InputLbl.BackColor != Color.Lime)
            //        RI4Vac1InputLbl.BackColor = Color.Lime;
            //}
            //else
            //{
            //    if (RI4Vac1InputLbl.BackColor != Color.Black)
            //        RI4Vac1InputLbl.BackColor = Color.Black;
            //}

            //if (myMgr.ReadIOIn((ushort)InputIOlist.RI4Vac2))
            //{
            //    if (RI4Vac2InputLbl.BackColor != Color.Lime)
            //        RI4Vac2InputLbl.BackColor = Color.Lime;
            //}
            //else
            //{
            //    if (RI4Vac2InputLbl.BackColor != Color.Black)
            //        RI4Vac2InputLbl.BackColor = Color.Black;
            //}
        }

        private void GreenLampBtn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.LampGreen);
            myMgr.WriteIOOut((ushort)OutputIOlist.LampGreen, !val);
        }

        private void AmberLampBtn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.LampAmber);
            myMgr.WriteIOOut((ushort)OutputIOlist.LampAmber, !val);
        }

        private void RedLampBtn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.LampRed);
            myMgr.WriteIOOut((ushort)OutputIOlist.LampRed, !val);
        }

        private void buzzerBtn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.Buzzer);
            myMgr.WriteIOOut((ushort)OutputIOlist.Buzzer, !val);
        }

        private void RI1Vac1Btn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.RI1Vac1);
            myMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, !val);
        }

        private void RI1Vac2Btn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.RI1Vac2);
            myMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, !val);
        }

        private void RI2Vac1Btn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.RI2Vac1);
            myMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, !val);
        }

        private void RI2Vac2Btn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.RI2Vac2);
            myMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, !val);
        }

        private void RI3Vac1Btn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.RI3Vac1);
            myMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, !val);
        }

        private void RI3Vac2Btn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.RI3Vac2);
            myMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, !val);
        }

        private void RI4Vac1Btn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.RI4Vac1);
            myMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, !val);
        }

        private void RI4Vac2Btn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.RI4Vac2);
            myMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, !val);
        }

        private void RI2Vac1Lbl_Click(object sender, EventArgs e)
        {

        }

        private void CamL1Btn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.Cam1Light);
            myMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, !val);
        }

        private void CamL2Btn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.Cam2Light);
            myMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, !val);
        }

        private void SpectLSBtn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.SpectrumLS);
            myMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, !val);
        }

        private void machLightBtn_Click(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.MchLight);
            myMgr.WriteIOOut((ushort)OutputIOlist.MchLight, !val);  
        }

        private void InputGB_Enter(object sender, EventArgs e)
        {

        }

        private void RI1Vac2Btn_Click_1(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.RI1Vac2);
            myMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, !val);
        }

        private void RI2Vac2Btn_Click_1(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.RI2Vac2);
            myMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, !val);
        }

        private void RI3Vac2Btn_Click_1(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.RI3Vac2);
            myMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, !val);
        }

        private void RI4Vac2Btn_Click_1(object sender, EventArgs e)
        {
            bool val = myMgr.ReadIOOut((ushort)OutputIOlist.RI4Vac2);
            myMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, !val);
        }
    }
}
