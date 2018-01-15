namespace JPTCG.Motion
{
    partial class JogPad
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
            this.components = new System.ComponentModel.Container();
            this.JPGB = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.PelLbl = new System.Windows.Forms.Label();
            this.NelLbl = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LeftBtn = new System.Windows.Forms.Button();
            this.DownBtn = new System.Windows.Forms.Button();
            this.RightBtn = new System.Windows.Forms.Button();
            this.UpBtn = new System.Windows.Forms.Button();
            this.stepEB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.GoToBtn = new System.Windows.Forms.Button();
            this.tchEB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.curPosEB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.JPTimer = new System.Windows.Forms.Timer(this.components);
            this.JPGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // JPGB
            // 
            this.JPGB.Controls.Add(this.button1);
            this.JPGB.Controls.Add(this.PelLbl);
            this.JPGB.Controls.Add(this.NelLbl);
            this.JPGB.Controls.Add(this.label5);
            this.JPGB.Controls.Add(this.label4);
            this.JPGB.Controls.Add(this.LeftBtn);
            this.JPGB.Controls.Add(this.DownBtn);
            this.JPGB.Controls.Add(this.RightBtn);
            this.JPGB.Controls.Add(this.UpBtn);
            this.JPGB.Controls.Add(this.stepEB);
            this.JPGB.Controls.Add(this.label3);
            this.JPGB.Controls.Add(this.ApplyBtn);
            this.JPGB.Controls.Add(this.GoToBtn);
            this.JPGB.Controls.Add(this.tchEB);
            this.JPGB.Controls.Add(this.label2);
            this.JPGB.Controls.Add(this.curPosEB);
            this.JPGB.Controls.Add(this.label1);
            this.JPGB.Location = new System.Drawing.Point(3, 3);
            this.JPGB.Name = "JPGB";
            this.JPGB.Size = new System.Drawing.Size(194, 244);
            this.JPGB.TabIndex = 0;
            this.JPGB.TabStop = false;
            this.JPGB.Text = "Not Assigned";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(145, 90);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "Home";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PelLbl
            // 
            this.PelLbl.BackColor = System.Drawing.Color.Black;
            this.PelLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PelLbl.Location = new System.Drawing.Point(165, 211);
            this.PelLbl.Name = "PelLbl";
            this.PelLbl.Size = new System.Drawing.Size(15, 15);
            this.PelLbl.TabIndex = 15;
            // 
            // NelLbl
            // 
            this.NelLbl.BackColor = System.Drawing.Color.Black;
            this.NelLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NelLbl.Location = new System.Drawing.Point(164, 151);
            this.NelLbl.Name = "NelLbl";
            this.NelLbl.Size = new System.Drawing.Size(15, 15);
            this.NelLbl.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(158, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "PEL";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(157, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "NEL";
            // 
            // LeftBtn
            // 
            this.LeftBtn.Image = global::JPTCG.Properties.Resources.LeftArrow;
            this.LeftBtn.Location = new System.Drawing.Point(28, 160);
            this.LeftBtn.Name = "LeftBtn";
            this.LeftBtn.Size = new System.Drawing.Size(40, 40);
            this.LeftBtn.TabIndex = 11;
            this.LeftBtn.UseVisualStyleBackColor = true;
            this.LeftBtn.Click += new System.EventHandler(this.LeftBtn_Click);
            // 
            // DownBtn
            // 
            this.DownBtn.Image = global::JPTCG.Properties.Resources.DownArrow;
            this.DownBtn.Location = new System.Drawing.Point(68, 200);
            this.DownBtn.Name = "DownBtn";
            this.DownBtn.Size = new System.Drawing.Size(40, 40);
            this.DownBtn.TabIndex = 10;
            this.DownBtn.UseVisualStyleBackColor = true;
            this.DownBtn.Click += new System.EventHandler(this.DownBtn_Click);
            // 
            // RightBtn
            // 
            this.RightBtn.Image = global::JPTCG.Properties.Resources.rightArrow;
            this.RightBtn.Location = new System.Drawing.Point(108, 160);
            this.RightBtn.Name = "RightBtn";
            this.RightBtn.Size = new System.Drawing.Size(40, 40);
            this.RightBtn.TabIndex = 9;
            this.RightBtn.UseVisualStyleBackColor = true;
            this.RightBtn.Click += new System.EventHandler(this.RightBtn_Click);
            // 
            // UpBtn
            // 
            this.UpBtn.Image = global::JPTCG.Properties.Resources.UpArrow;
            this.UpBtn.Location = new System.Drawing.Point(68, 120);
            this.UpBtn.Name = "UpBtn";
            this.UpBtn.Size = new System.Drawing.Size(40, 40);
            this.UpBtn.TabIndex = 8;
            this.UpBtn.UseVisualStyleBackColor = true;
            this.UpBtn.Click += new System.EventHandler(this.UpBtn_Click);
            // 
            // stepEB
            // 
            this.stepEB.Location = new System.Drawing.Point(75, 92);
            this.stepEB.Name = "stepEB";
            this.stepEB.Size = new System.Drawing.Size(64, 21);
            this.stepEB.TabIndex = 7;
            this.stepEB.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(21, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Step(mm)";
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplyBtn.Location = new System.Drawing.Point(145, 27);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(45, 23);
            this.ApplyBtn.TabIndex = 5;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // GoToBtn
            // 
            this.GoToBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GoToBtn.Location = new System.Drawing.Point(145, 63);
            this.GoToBtn.Name = "GoToBtn";
            this.GoToBtn.Size = new System.Drawing.Size(45, 23);
            this.GoToBtn.TabIndex = 4;
            this.GoToBtn.Text = "GoTo";
            this.GoToBtn.UseVisualStyleBackColor = true;
            this.GoToBtn.Click += new System.EventHandler(this.GoToBtn_Click);
            // 
            // tchEB
            // 
            this.tchEB.Location = new System.Drawing.Point(75, 60);
            this.tchEB.Name = "tchEB";
            this.tchEB.ReadOnly = true;
            this.tchEB.Size = new System.Drawing.Size(64, 21);
            this.tchEB.TabIndex = 3;
            this.tchEB.Text = "0.000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Teached(mm)";
            // 
            // curPosEB
            // 
            this.curPosEB.Location = new System.Drawing.Point(75, 28);
            this.curPosEB.Name = "curPosEB";
            this.curPosEB.ReadOnly = true;
            this.curPosEB.Size = new System.Drawing.Size(64, 21);
            this.curPosEB.TabIndex = 1;
            this.curPosEB.Text = "0.000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current(mm)";
            // 
            // JPTimer
            // 
            this.JPTimer.Tick += new System.EventHandler(this.JPTimer_Tick);
            // 
            // JogPad
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.JPGB);
            this.Name = "JogPad";
            this.Size = new System.Drawing.Size(200, 250);
            this.JPGB.ResumeLayout(false);
            this.JPGB.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox JPGB;
        private System.Windows.Forms.TextBox curPosEB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tchEB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button GoToBtn;
        private System.Windows.Forms.Button ApplyBtn;
        private System.Windows.Forms.TextBox stepEB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button UpBtn;
        private System.Windows.Forms.Button LeftBtn;
        private System.Windows.Forms.Button DownBtn;
        private System.Windows.Forms.Button RightBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label PelLbl;
        private System.Windows.Forms.Label NelLbl;
        private System.Windows.Forms.Timer JPTimer;
        private System.Windows.Forms.Button button1;
    }
}
