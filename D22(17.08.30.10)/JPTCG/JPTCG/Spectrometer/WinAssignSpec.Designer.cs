namespace JPTCG.Spectrometer
{
    partial class WinAssignSpec
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CAS140RB = new System.Windows.Forms.RadioButton();
            this.NotRB = new System.Windows.Forms.RadioButton();
            this.MayaRB = new System.Windows.Forms.RadioButton();
            this.AvantesRB = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.specLB = new System.Windows.Forms.ListBox();
            this.AssignBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ModNameEB = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CAS140RB);
            this.groupBox1.Controls.Add(this.NotRB);
            this.groupBox1.Controls.Add(this.MayaRB);
            this.groupBox1.Controls.Add(this.AvantesRB);
            this.groupBox1.Location = new System.Drawing.Point(12, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(244, 83);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type";
            // 
            // CAS140RB
            // 
            this.CAS140RB.AutoSize = true;
            this.CAS140RB.Location = new System.Drawing.Point(6, 48);
            this.CAS140RB.Name = "CAS140RB";
            this.CAS140RB.Size = new System.Drawing.Size(80, 21);
            this.CAS140RB.TabIndex = 3;
            this.CAS140RB.TabStop = true;
            this.CAS140RB.Text = "CAS140";
            this.CAS140RB.UseVisualStyleBackColor = true;
            this.CAS140RB.CheckedChanged += new System.EventHandler(this.CAS140RB_CheckedChanged);
            // 
            // NotRB
            // 
            this.NotRB.AutoSize = true;
            this.NotRB.Location = new System.Drawing.Point(108, 48);
            this.NotRB.Name = "NotRB";
            this.NotRB.Size = new System.Drawing.Size(113, 21);
            this.NotRB.TabIndex = 2;
            this.NotRB.TabStop = true;
            this.NotRB.Text = "Not Assigned";
            this.NotRB.UseVisualStyleBackColor = true;
            this.NotRB.CheckedChanged += new System.EventHandler(this.NotRB_CheckedChanged);
            // 
            // MayaRB
            // 
            this.MayaRB.AutoSize = true;
            this.MayaRB.Location = new System.Drawing.Point(108, 21);
            this.MayaRB.Name = "MayaRB";
            this.MayaRB.Size = new System.Drawing.Size(63, 21);
            this.MayaRB.TabIndex = 1;
            this.MayaRB.TabStop = true;
            this.MayaRB.Text = "Maya";
            this.MayaRB.UseVisualStyleBackColor = true;
            this.MayaRB.CheckedChanged += new System.EventHandler(this.MayaRB_CheckedChanged);
            // 
            // AvantesRB
            // 
            this.AvantesRB.AutoSize = true;
            this.AvantesRB.Location = new System.Drawing.Point(6, 21);
            this.AvantesRB.Name = "AvantesRB";
            this.AvantesRB.Size = new System.Drawing.Size(80, 21);
            this.AvantesRB.TabIndex = 0;
            this.AvantesRB.TabStop = true;
            this.AvantesRB.Text = "Avantes";
            this.AvantesRB.UseVisualStyleBackColor = true;
            this.AvantesRB.CheckedChanged += new System.EventHandler(this.AvantesRB_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Spectrometer List";
            // 
            // specLB
            // 
            this.specLB.FormattingEnabled = true;
            this.specLB.ItemHeight = 16;
            this.specLB.Location = new System.Drawing.Point(12, 144);
            this.specLB.Name = "specLB";
            this.specLB.Size = new System.Drawing.Size(130, 116);
            this.specLB.TabIndex = 3;
            // 
            // AssignBtn
            // 
            this.AssignBtn.Location = new System.Drawing.Point(148, 228);
            this.AssignBtn.Name = "AssignBtn";
            this.AssignBtn.Size = new System.Drawing.Size(108, 32);
            this.AssignBtn.TabIndex = 6;
            this.AssignBtn.Text = "Assign";
            this.AssignBtn.UseVisualStyleBackColor = true;
            this.AssignBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Name";
            // 
            // ModNameEB
            // 
            this.ModNameEB.Location = new System.Drawing.Point(66, 9);
            this.ModNameEB.Name = "ModNameEB";
            this.ModNameEB.ReadOnly = true;
            this.ModNameEB.Size = new System.Drawing.Size(190, 22);
            this.ModNameEB.TabIndex = 8;
            // 
            // WinAssignSpec
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(271, 268);
            this.Controls.Add(this.ModNameEB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AssignBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.specLB);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WinAssignSpec";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WinAssignSpec";
            this.Load += new System.EventHandler(this.WinAssignSpec_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton NotRB;
        private System.Windows.Forms.RadioButton MayaRB;
        private System.Windows.Forms.RadioButton AvantesRB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox specLB;
        private System.Windows.Forms.Button AssignBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ModNameEB;
        private System.Windows.Forms.RadioButton CAS140RB;
    }
}