namespace JPTCG.Vision
{
    partial class HalconInterface
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLblX = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLblY = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.camStatusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TBFitToScreen = new System.Windows.Forms.ToolStripButton();
            this.TBZoomIn = new System.Windows.Forms.ToolStripButton();
            this.TBZoomOut = new System.Windows.Forms.ToolStripButton();
            this.TBMove = new System.Windows.Forms.ToolStripButton();
            this.TBCrossHair = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.TBLiveCam = new System.Windows.Forms.ToolStripButton();
            this.TBStopCam = new System.Windows.Forms.ToolStripButton();
            this.camIDLbl = new System.Windows.Forms.Label();
            this.AssignCamBtn = new System.Windows.Forms.Button();
            this.InspectBtn = new System.Windows.Forms.Button();
            this.ModuleNameLbl = new System.Windows.Forms.Label();
            this.LoadImgBtn = new System.Windows.Forms.Button();
            this.OpenImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CaliXLbl = new System.Windows.Forms.Label();
            this.CaliYLbl = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.StatusLblX,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel2,
            this.StatusLblY,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.camStatusLbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 279);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(472, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(23, 17);
            this.toolStripStatusLabel1.Text = "X : ";
            // 
            // StatusLblX
            // 
            this.StatusLblX.Name = "StatusLblX";
            this.StatusLblX.Size = new System.Drawing.Size(22, 17);
            this.StatusLblX.Text = "0.0";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel3.Text = "   ";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(20, 17);
            this.toolStripStatusLabel2.Text = "Y :";
            // 
            // StatusLblY
            // 
            this.StatusLblY.Name = "StatusLblY";
            this.StatusLblY.Size = new System.Drawing.Size(22, 17);
            this.StatusLblY.Text = "0.0";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel4.Text = "   ";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(45, 17);
            this.toolStripStatusLabel5.Text = "Status :";
            // 
            // camStatusLbl
            // 
            this.camStatusLbl.Name = "camStatusLbl";
            this.camStatusLbl.Size = new System.Drawing.Size(41, 17);
            this.camStatusLbl.Text = "offline";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TBFitToScreen,
            this.TBZoomIn,
            this.TBZoomOut,
            this.TBMove,
            this.TBCrossHair,
            this.toolStripLabel1,
            this.TBLiveCam,
            this.TBStopCam});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(472, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // TBFitToScreen
            // 
            this.TBFitToScreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TBFitToScreen.Image = global::JPTCG.Properties.Resources.FitToScreen;
            this.TBFitToScreen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TBFitToScreen.Name = "TBFitToScreen";
            this.TBFitToScreen.Size = new System.Drawing.Size(23, 22);
            this.TBFitToScreen.Text = "Fit To Screen";
            this.TBFitToScreen.Click += new System.EventHandler(this.TBFitToScreen_Click);
            // 
            // TBZoomIn
            // 
            this.TBZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TBZoomIn.Image = global::JPTCG.Properties.Resources.zoom_in_icon;
            this.TBZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TBZoomIn.Name = "TBZoomIn";
            this.TBZoomIn.Size = new System.Drawing.Size(23, 22);
            this.TBZoomIn.Text = "Zoom In";
            this.TBZoomIn.Click += new System.EventHandler(this.TBZoomIn_Click);
            // 
            // TBZoomOut
            // 
            this.TBZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TBZoomOut.Image = global::JPTCG.Properties.Resources.zoom_out;
            this.TBZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TBZoomOut.Name = "TBZoomOut";
            this.TBZoomOut.Size = new System.Drawing.Size(23, 22);
            this.TBZoomOut.Text = "Zoom Out";
            this.TBZoomOut.Click += new System.EventHandler(this.TBZoomOut_Click);
            // 
            // TBMove
            // 
            this.TBMove.CheckOnClick = true;
            this.TBMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TBMove.Image = global::JPTCG.Properties.Resources.Move_icon;
            this.TBMove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TBMove.Name = "TBMove";
            this.TBMove.Size = new System.Drawing.Size(23, 22);
            this.TBMove.Text = "Move Image";
            this.TBMove.Click += new System.EventHandler(this.TBMove_Click);
            // 
            // TBCrossHair
            // 
            this.TBCrossHair.Checked = true;
            this.TBCrossHair.CheckOnClick = true;
            this.TBCrossHair.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TBCrossHair.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TBCrossHair.Image = global::JPTCG.Properties.Resources.CrossHair;
            this.TBCrossHair.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TBCrossHair.Name = "TBCrossHair";
            this.TBCrossHair.Size = new System.Drawing.Size(23, 22);
            this.TBCrossHair.Text = "Show/Hide Cross Hair";
            this.TBCrossHair.Click += new System.EventHandler(this.TBCrossHair_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(16, 22);
            this.toolStripLabel1.Text = "   ";
            // 
            // TBLiveCam
            // 
            this.TBLiveCam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TBLiveCam.Image = global::JPTCG.Properties.Resources.icon_Live_camera;
            this.TBLiveCam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TBLiveCam.Name = "TBLiveCam";
            this.TBLiveCam.Size = new System.Drawing.Size(23, 22);
            this.TBLiveCam.Text = "Live Camera";
            this.TBLiveCam.Click += new System.EventHandler(this.TBLiveCam_Click);
            // 
            // TBStopCam
            // 
            this.TBStopCam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TBStopCam.Image = global::JPTCG.Properties.Resources.icon_Stop_camera;
            this.TBStopCam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TBStopCam.Name = "TBStopCam";
            this.TBStopCam.Size = new System.Drawing.Size(23, 22);
            this.TBStopCam.Text = "Stop Camera";
            this.TBStopCam.Click += new System.EventHandler(this.TBStopCam_Click);
            // 
            // camIDLbl
            // 
            this.camIDLbl.AutoSize = true;
            this.camIDLbl.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.camIDLbl.Location = new System.Drawing.Point(329, 68);
            this.camIDLbl.Name = "camIDLbl";
            this.camIDLbl.Size = new System.Drawing.Size(60, 13);
            this.camIDLbl.TabIndex = 7;
            this.camIDLbl.Text = "No Camera";
            // 
            // AssignCamBtn
            // 
            this.AssignCamBtn.Location = new System.Drawing.Point(329, 88);
            this.AssignCamBtn.Name = "AssignCamBtn";
            this.AssignCamBtn.Size = new System.Drawing.Size(127, 25);
            this.AssignCamBtn.TabIndex = 8;
            this.AssignCamBtn.Text = "Assign Camera";
            this.AssignCamBtn.UseVisualStyleBackColor = true;
            this.AssignCamBtn.Click += new System.EventHandler(this.AssignCamBtn_Click);
            // 
            // InspectBtn
            // 
            this.InspectBtn.Location = new System.Drawing.Point(329, 150);
            this.InspectBtn.Name = "InspectBtn";
            this.InspectBtn.Size = new System.Drawing.Size(127, 25);
            this.InspectBtn.TabIndex = 9;
            this.InspectBtn.Text = "Inspect";
            this.InspectBtn.UseVisualStyleBackColor = true;
            this.InspectBtn.Click += new System.EventHandler(this.InspectBtn_Click);
            // 
            // ModuleNameLbl
            // 
            this.ModuleNameLbl.AutoSize = true;
            this.ModuleNameLbl.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ModuleNameLbl.Location = new System.Drawing.Point(329, 33);
            this.ModuleNameLbl.Name = "ModuleNameLbl";
            this.ModuleNameLbl.Size = new System.Drawing.Size(35, 13);
            this.ModuleNameLbl.TabIndex = 10;
            this.ModuleNameLbl.Text = "label2";
            // 
            // LoadImgBtn
            // 
            this.LoadImgBtn.Location = new System.Drawing.Point(329, 119);
            this.LoadImgBtn.Name = "LoadImgBtn";
            this.LoadImgBtn.Size = new System.Drawing.Size(127, 25);
            this.LoadImgBtn.TabIndex = 11;
            this.LoadImgBtn.Text = "Load Image";
            this.LoadImgBtn.UseVisualStyleBackColor = true;
            this.LoadImgBtn.Click += new System.EventHandler(this.LoadImgBtn_Click);
            // 
            // OpenImageDialog
            // 
            this.OpenImageDialog.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(326, 235);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "X (mm/pixel) :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(326, 257);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Y (mm/pixel) :";
            // 
            // CaliXLbl
            // 
            this.CaliXLbl.AutoSize = true;
            this.CaliXLbl.Location = new System.Drawing.Point(408, 237);
            this.CaliXLbl.Name = "CaliXLbl";
            this.CaliXLbl.Size = new System.Drawing.Size(35, 13);
            this.CaliXLbl.TabIndex = 14;
            this.CaliXLbl.Text = "label3";
            // 
            // CaliYLbl
            // 
            this.CaliYLbl.AutoSize = true;
            this.CaliYLbl.Location = new System.Drawing.Point(408, 257);
            this.CaliYLbl.Name = "CaliYLbl";
            this.CaliYLbl.Size = new System.Drawing.Size(35, 13);
            this.CaliYLbl.TabIndex = 15;
            this.CaliYLbl.Text = "label4";
            // 
            // HalconInterface
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.CaliYLbl);
            this.Controls.Add(this.CaliXLbl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LoadImgBtn);
            this.Controls.Add(this.ModuleNameLbl);
            this.Controls.Add(this.InspectBtn);
            this.Controls.Add(this.AssignCamBtn);
            this.Controls.Add(this.camIDLbl);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "HalconInterface";
            this.Size = new System.Drawing.Size(472, 301);
            this.Load += new System.EventHandler(this.HalconInterface_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLblX;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private HalconDotNet.HWindowControl HalconWin;
        private System.Windows.Forms.ToolStripButton TBFitToScreen;
        private System.Windows.Forms.ToolStripButton TBZoomIn;
        private System.Windows.Forms.ToolStripButton TBZoomOut;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel StatusLblY;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel camStatusLbl;
        private System.Windows.Forms.ToolStripButton TBCrossHair;
        private System.Windows.Forms.ToolStripButton TBMove;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton TBLiveCam;
        private System.Windows.Forms.ToolStripButton TBStopCam;
        private System.Windows.Forms.Label camIDLbl;
        private System.Windows.Forms.Button AssignCamBtn;
        private System.Windows.Forms.Button InspectBtn;
        private System.Windows.Forms.Label ModuleNameLbl;
        private System.Windows.Forms.Button LoadImgBtn;
        private System.Windows.Forms.OpenFileDialog OpenImageDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label CaliXLbl;
        private System.Windows.Forms.Label CaliYLbl;
    }
}
