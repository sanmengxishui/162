namespace JPTCG.BarcodeScanner
{
    partial class WinBarcode
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
            this.ModLB = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ip1TB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdTB = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.msgLB = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ModLB
            // 
            this.ModLB.FormattingEnabled = true;
            this.ModLB.Location = new System.Drawing.Point(12, 28);
            this.ModLB.Name = "ModLB";
            this.ModLB.Size = new System.Drawing.Size(144, 173);
            this.ModLB.TabIndex = 3;
            this.ModLB.SelectedIndexChanged += new System.EventHandler(this.ModLB_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Module List";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(193, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "IP Address";
            // 
            // ip1TB
            // 
            this.ip1TB.Location = new System.Drawing.Point(275, 26);
            this.ip1TB.Name = "ip1TB";
            this.ip1TB.Size = new System.Drawing.Size(125, 20);
            this.ip1TB.TabIndex = 50;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(235, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 54;
            this.label3.Text = "Port";
            // 
            // cmdTB
            // 
            this.cmdTB.Location = new System.Drawing.Point(275, 62);
            this.cmdTB.Name = "cmdTB";
            this.cmdTB.Size = new System.Drawing.Size(100, 20);
            this.cmdTB.TabIndex = 55;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(437, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 25);
            this.button1.TabIndex = 58;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(555, 54);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 25);
            this.button2.TabIndex = 59;
            this.button2.Text = "Read";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // msgLB
            // 
            this.msgLB.FormattingEnabled = true;
            this.msgLB.Location = new System.Drawing.Point(162, 90);
            this.msgLB.Name = "msgLB";
            this.msgLB.Size = new System.Drawing.Size(481, 108);
            this.msgLB.TabIndex = 60;
            this.msgLB.SelectedIndexChanged += new System.EventHandler(this.msgLB_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(437, 54);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 25);
            this.button3.TabIndex = 61;
            this.button3.Text = "Disconnect";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(162, 204);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(88, 25);
            this.button4.TabIndex = 62;
            this.button4.Text = "LOFF";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // WinBarcode
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(675, 235);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.msgLB);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmdTB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ip1TB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ModLB);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WinBarcode";
            this.Text = "Barcode Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WinBarcode_FormClosing);
            this.Load += new System.EventHandler(this.WinBarcode_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ModLB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ip1TB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox cmdTB;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox msgLB;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}