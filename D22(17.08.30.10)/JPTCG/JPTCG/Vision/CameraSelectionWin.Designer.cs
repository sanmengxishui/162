namespace JPTCG.Vision
{
    partial class CameraSelectionWin
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CamLB = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AssignBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CamLB
            // 
            this.CamLB.FormattingEnabled = true;
            this.CamLB.ItemHeight = 16;
            this.CamLB.Location = new System.Drawing.Point(12, 35);
            this.CamLB.Name = "CamLB";
            this.CamLB.Size = new System.Drawing.Size(263, 164);
            this.CamLB.TabIndex = 0;
            this.CamLB.SelectedIndexChanged += new System.EventHandler(this.CamLB_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Camera List";
            // 
            // AssignBtn
            // 
            this.AssignBtn.Location = new System.Drawing.Point(15, 205);
            this.AssignBtn.Name = "AssignBtn";
            this.AssignBtn.Size = new System.Drawing.Size(121, 31);
            this.AssignBtn.TabIndex = 2;
            this.AssignBtn.Text = "Assign";
            this.AssignBtn.UseVisualStyleBackColor = true;
            this.AssignBtn.Click += new System.EventHandler(this.AssignBtn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(154, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 31);
            this.button1.TabIndex = 3;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CameraSelectionWin
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(287, 248);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.AssignBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CamLB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CameraSelectionWin";
            this.Text = "Camera Selection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox CamLB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AssignBtn;
        private System.Windows.Forms.Button button1;
    }
}