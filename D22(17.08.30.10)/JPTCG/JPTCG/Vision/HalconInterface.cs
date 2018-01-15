using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using ViewROI;

namespace JPTCG.Vision
{
    public partial class HalconInterface : UserControl
    {
        HalconVision myHalcon = null;
        private HWndCtrl hWndCtrl;
        private Object m_lockShowpicture;

        public HalconInterface(String name, Panel parentPnl,HalconVision halVis, HWindowControl myHalconWin)
        {
            InitializeComponent();
            ModuleNameLbl.Text = name;
            this.Parent = parentPnl;
            myHalcon = halVis;
            m_lockShowpicture = new object();
            HalconWin = myHalconWin;
            hWndCtrl = new HWndCtrl(HalconWin);
            hWndCtrl.setViewState(HWndCtrl.MODE_VIEW_NONE);
            hWndCtrl.repaint();
            myHalcon.OnImageReadyFunction += OnImgReady;

            CaliXLbl.Text = myHalcon.CaliValue.X.ToString("F3");
            CaliYLbl.Text = myHalcon.CaliValue.Y.ToString("F3");            
        }

        

        private void AssignCamBtn_Click(object sender, EventArgs e)
        {
            CameraSelectionWin camSelWin = new CameraSelectionWin(myHalcon);
            camSelWin.ShowDialog();
            camIDLbl.Text = myHalcon.cameraID;
        }

        private void OpenImageFile()
        {
            string strHeadImagePath;
            HImage image;

            OpenImageDialog.Title = "Open Image file";
            OpenImageDialog.ShowHelp = true;
            OpenImageDialog.Filter = "(*.gif)|*.gif|(*.jpg)|*.jpg|(*.JPEG)|*.JPEG|(*.bmp)|*.bmp|(*.png)|*.png|All files (*.*)|*.*";
            DialogResult result = OpenImageDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    strHeadImagePath = OpenImageDialog.FileName;
                    image = new HImage(strHeadImagePath);
                    myHalcon.myImage = image;
                    hWndCtrl.ClearResult();
                    hWndCtrl.addIconicVar(image);
                    hWndCtrl.repaint();
                }
                catch
                {
                    MessageBox.Show("format not correct");
                }
            }
        }

        private void LoadImgBtn_Click(object sender, EventArgs e)
        {            
            OpenImageFile();            
        }

        private void TBCrossHair_Click(object sender, EventArgs e)
        {
            lock (m_lockShowpicture)
            {
                hWndCtrl.bShowCrossHair = !hWndCtrl.bShowCrossHair;
                hWndCtrl.repaint();
            }
        }

        private void HalconWin_HMouseMove(object sender, HMouseEventArgs e)
        {
            HTuple Window = new HTuple();
            Window = HalconWin.HalconWindow;
            HTuple Row1, Col1, Button;
            try
            {
                HOperatorSet.GetMposition(Window, out Row1, out Col1, out Button);
                StatusLblX.Text = Col1.ToString();
                StatusLblY.Text = Row1.ToString();
            }
            catch
            {
            }
        }

        private void TBFitToScreen_Click(object sender, EventArgs e)
        {
            try
            {
                lock (m_lockShowpicture)
                {
                    hWndCtrl.resetWindow();
                    hWndCtrl.repaint();
                }
            }
            catch
            {
            }
        }

        private void TBZoomIn_Click(object sender, EventArgs e)
        {
            lock (m_lockShowpicture)
            {
                hWndCtrl.zoomImage(0.9);
            }
        }

        private void TBZoomOut_Click(object sender, EventArgs e)
        {
            lock (m_lockShowpicture)
            {
                hWndCtrl.zoomImage(1.1);
            }
        }

        private void TBMove_Click(object sender, EventArgs e)
        {
            if (hWndCtrl.GetViewState() == HWndCtrl.MODE_VIEW_MOVE)
                hWndCtrl.setViewState(HWndCtrl.MODE_VIEW_NONE);
            else
                hWndCtrl.setViewState(HWndCtrl.MODE_VIEW_MOVE);
        }

        private void TBLiveCam_Click(object sender, EventArgs e)
        {
            if (myHalcon.IsCameraOpen())
            {
                hWndCtrl.setViewState(HWndCtrl.MODE_VIEW_NONE);
                hWndCtrl.resetWindow();
                hWndCtrl.repaint();
                myHalcon.LiveCamera();
                EnbToolBtn(false);
                camStatusLbl.Text = "Live...";
                camStatusLbl.BackColor = Color.Lime;
            }
            
        }
        private void EnbToolBtn(bool Val)
        {
            TBFitToScreen.Enabled = Val;
            TBZoomIn.Enabled = Val;
            TBZoomOut.Enabled = Val;
            TBMove.Enabled = Val;
            TBLiveCam.Enabled = Val;
            AssignCamBtn.Enabled = Val;
            LoadImgBtn.Enabled = Val;
            InspectBtn.Enabled = Val;
        }

        private void TBStopCam_Click(object sender, EventArgs e)
        {
            if (myHalcon.IsCameraOpen())
            {           
                myHalcon.StopCamera();
                EnbToolBtn(true);
                camStatusLbl.Text = "Idle...";
                camStatusLbl.BackColor = Color.Orange;                
            }
            
        }

        private void InspectBtn_Click(object sender, EventArgs e)
        {
            hWndCtrl.ClearResult();
            JPTCG.Vision.HalconInspection.RectData myRes =  myHalcon.Inspect();            
            hWndCtrl.DrawRect(myRes);

        }

        private void HalconInterface_Load(object sender, EventArgs e)
        {
            camIDLbl.Text = myHalcon.cameraID;
        }
    }
}
