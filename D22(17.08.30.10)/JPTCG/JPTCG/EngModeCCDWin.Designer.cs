namespace JPTCG
{
    partial class EngModeCCDWin
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.XEB = new System.Windows.Forms.TextBox();
            this.YEB = new System.Windows.Forms.TextBox();
            this.AngEB = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Angle";
            // 
            // XEB
            // 
            this.XEB.Location = new System.Drawing.Point(52, 15);
            this.XEB.Name = "XEB";
            this.XEB.Size = new System.Drawing.Size(95, 20);
            this.XEB.TabIndex = 3;
            // 
            // YEB
            // 
            this.YEB.Location = new System.Drawing.Point(52, 43);
            this.YEB.Name = "YEB";
            this.YEB.Size = new System.Drawing.Size(95, 20);
            this.YEB.TabIndex = 4;
            // 
            // AngEB
            // 
            this.AngEB.Location = new System.Drawing.Point(52, 76);
            this.AngEB.Name = "AngEB";
            this.AngEB.Size = new System.Drawing.Size(95, 20);
            this.AngEB.TabIndex = 5;
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(95, 105);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(52, 21);
            this.OKBtn.TabIndex = 6;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // EngModeCCDWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(162, 138);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.AngEB);
            this.Controls.Add(this.YEB);
            this.Controls.Add(this.XEB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "EngModeCCDWin";
            this.Text = "CCD";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.EngModeCCDWin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox XEB;
        private System.Windows.Forms.TextBox YEB;
        private System.Windows.Forms.TextBox AngEB;
        private System.Windows.Forms.Button OKBtn;
    }
}