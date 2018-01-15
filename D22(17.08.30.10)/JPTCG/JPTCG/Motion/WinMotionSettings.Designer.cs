namespace JPTCG.Motion
{
    partial class WinMotionSettings
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
            this.label1 = new System.Windows.Forms.Label();
            this.AxisLB = new System.Windows.Forms.ListBox();
            this.SettingGB = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.MScaleEB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.RunSpEB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.HomeSpEB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AxisNumEB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.CBIsServo = new System.Windows.Forms.CheckBox();
            this.SettingGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Axis List";
            // 
            // AxisLB
            // 
            this.AxisLB.FormattingEnabled = true;
            this.AxisLB.Location = new System.Drawing.Point(12, 39);
            this.AxisLB.Name = "AxisLB";
            this.AxisLB.Size = new System.Drawing.Size(146, 225);
            this.AxisLB.TabIndex = 1;
            this.AxisLB.SelectedIndexChanged += new System.EventHandler(this.AxisLB_SelectedIndexChanged);
            // 
            // SettingGB
            // 
            this.SettingGB.Controls.Add(this.CBIsServo);
            this.SettingGB.Controls.Add(this.label7);
            this.SettingGB.Controls.Add(this.label6);
            this.SettingGB.Controls.Add(this.label8);
            this.SettingGB.Controls.Add(this.MScaleEB);
            this.SettingGB.Controls.Add(this.label5);
            this.SettingGB.Controls.Add(this.RunSpEB);
            this.SettingGB.Controls.Add(this.label4);
            this.SettingGB.Controls.Add(this.HomeSpEB);
            this.SettingGB.Controls.Add(this.label3);
            this.SettingGB.Controls.Add(this.AxisNumEB);
            this.SettingGB.Controls.Add(this.label2);
            this.SettingGB.Location = new System.Drawing.Point(181, 31);
            this.SettingGB.Name = "SettingGB";
            this.SettingGB.Size = new System.Drawing.Size(258, 229);
            this.SettingGB.TabIndex = 2;
            this.SettingGB.TabStop = false;
            this.SettingGB.Text = "groupBox1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(208, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Pulse";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(208, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Pulse";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(208, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Pulse";
            // 
            // MScaleEB
            // 
            this.MScaleEB.Location = new System.Drawing.Point(109, 153);
            this.MScaleEB.Name = "MScaleEB";
            this.MScaleEB.Size = new System.Drawing.Size(97, 20);
            this.MScaleEB.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Motor Scale";
            // 
            // RunSpEB
            // 
            this.RunSpEB.Location = new System.Drawing.Point(109, 113);
            this.RunSpEB.Name = "RunSpEB";
            this.RunSpEB.Size = new System.Drawing.Size(97, 20);
            this.RunSpEB.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Run Speed";
            // 
            // HomeSpEB
            // 
            this.HomeSpEB.Location = new System.Drawing.Point(109, 70);
            this.HomeSpEB.Name = "HomeSpEB";
            this.HomeSpEB.Size = new System.Drawing.Size(97, 20);
            this.HomeSpEB.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Home Speed";
            // 
            // AxisNumEB
            // 
            this.AxisNumEB.Location = new System.Drawing.Point(109, 28);
            this.AxisNumEB.Name = "AxisNumEB";
            this.AxisNumEB.ReadOnly = true;
            this.AxisNumEB.Size = new System.Drawing.Size(100, 20);
            this.AxisNumEB.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Axis Number";
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(364, 266);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 3;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // CBIsServo
            // 
            this.CBIsServo.AutoSize = true;
            this.CBIsServo.Location = new System.Drawing.Point(23, 193);
            this.CBIsServo.Name = "CBIsServo";
            this.CBIsServo.Size = new System.Drawing.Size(84, 17);
            this.CBIsServo.TabIndex = 13;
            this.CBIsServo.Text = "Servo Motor";
            this.CBIsServo.UseVisualStyleBackColor = true;
            // 
            // WinMotionSettings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(452, 321);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.SettingGB);
            this.Controls.Add(this.AxisLB);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WinMotionSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Delta Motion Settings";
            this.Load += new System.EventHandler(this.WinMotionSettings_Load);
            this.SettingGB.ResumeLayout(false);
            this.SettingGB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox AxisLB;
        private System.Windows.Forms.GroupBox SettingGB;
        private System.Windows.Forms.TextBox AxisNumEB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox MScaleEB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox RunSpEB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox HomeSpEB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox CBIsServo;
    }
}