namespace JPTCG.Spectrometer
{
    partial class WinSpectrometer
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.label1 = new System.Windows.Forms.Label();
            this.ModLB = new System.Windows.Forms.ListBox();
            this.SpecChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.SpecSettingsGB = new System.Windows.Forms.GroupBox();
            this.usedTimeEB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.setIOCB = new System.Windows.Forms.CheckBox();
            this.SerialEB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TypeEB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AssignBtn = new System.Windows.Forms.Button();
            this.TestSetGB = new System.Windows.Forms.GroupBox();
            this.GSCaliBtn = new System.Windows.Forms.Button();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.NiCaliGB = new System.Windows.Forms.GroupBox();
            this.NICaliLV = new System.Windows.Forms.ListView();
            this.button7 = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ParametersGB = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SetParameterBtn = new System.Windows.Forms.Button();
            this.SmoothPixEB = new System.Windows.Forms.TextBox();
            this.label74 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.edtSaturationLevel = new System.Windows.Forms.TextBox();
            this.StartPixelEB = new System.Windows.Forms.TextBox();
            this.label67 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.NumOfAvgEB = new System.Windows.Forms.TextBox();
            this.StopPixelEB = new System.Windows.Forms.TextBox();
            this.label69 = new System.Windows.Forms.Label();
            this.label70 = new System.Windows.Forms.Label();
            this.IntegrateTimeEB = new System.Windows.Forms.TextBox();
            this.DarkRefBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.specTimer = new System.Windows.Forms.Timer(this.components);
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SpecChart)).BeginInit();
            this.SpecSettingsGB.SuspendLayout();
            this.TestSetGB.SuspendLayout();
            this.NiCaliGB.SuspendLayout();
            this.ParametersGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Module List";
            // 
            // ModLB
            // 
            this.ModLB.FormattingEnabled = true;
            this.ModLB.Location = new System.Drawing.Point(15, 29);
            this.ModLB.Name = "ModLB";
            this.ModLB.Size = new System.Drawing.Size(163, 43);
            this.ModLB.TabIndex = 1;
            this.ModLB.SelectedIndexChanged += new System.EventHandler(this.ModLB_SelectedIndexChanged);
            // 
            // SpecChart
            // 
            this.SpecChart.AccessibleRole = System.Windows.Forms.AccessibleRole.IpAddress;
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisX.LabelAutoFitStyle = ((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)(((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.WordWrap)));
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisX.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea;
            chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea;
            chartArea1.BorderColor = System.Drawing.Color.OrangeRed;
            chartArea1.CursorY.LineWidth = 2;
            chartArea1.InnerPlotPosition.Auto = false;
            chartArea1.InnerPlotPosition.Height = 87.53736F;
            chartArea1.InnerPlotPosition.Width = 92.21973F;
            chartArea1.InnerPlotPosition.X = 6.66325F;
            chartArea1.InnerPlotPosition.Y = 3.01689F;
            chartArea1.IsSameFontSizeForAllAxes = true;
            chartArea1.Name = "ChartArea1";
            this.SpecChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            legend1.Position.Auto = false;
            legend1.Position.Height = 7.142857F;
            legend1.Position.Width = 10.8508F;
            legend1.Position.X = 10F;
            legend1.Position.Y = 2F;
            this.SpecChart.Legends.Add(legend1);
            this.SpecChart.Location = new System.Drawing.Point(15, 279);
            this.SpecChart.Margin = new System.Windows.Forms.Padding(2);
            this.SpecChart.Name = "SpecChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.SpecChart.Series.Add(series1);
            this.SpecChart.Size = new System.Drawing.Size(519, 263);
            this.SpecChart.TabIndex = 10;
            this.SpecChart.Text = "Mod1Chart";
            title1.Name = "Title1";
            this.SpecChart.Titles.Add(title1);
            // 
            // SpecSettingsGB
            // 
            this.SpecSettingsGB.Controls.Add(this.usedTimeEB);
            this.SpecSettingsGB.Controls.Add(this.label4);
            this.SpecSettingsGB.Controls.Add(this.setIOCB);
            this.SpecSettingsGB.Controls.Add(this.SerialEB);
            this.SpecSettingsGB.Controls.Add(this.label3);
            this.SpecSettingsGB.Controls.Add(this.TypeEB);
            this.SpecSettingsGB.Controls.Add(this.label2);
            this.SpecSettingsGB.Controls.Add(this.AssignBtn);
            this.SpecSettingsGB.Location = new System.Drawing.Point(184, 12);
            this.SpecSettingsGB.Name = "SpecSettingsGB";
            this.SpecSettingsGB.Size = new System.Drawing.Size(352, 106);
            this.SpecSettingsGB.TabIndex = 11;
            this.SpecSettingsGB.TabStop = false;
            this.SpecSettingsGB.Text = "groupBox1";
            // 
            // usedTimeEB
            // 
            this.usedTimeEB.Location = new System.Drawing.Point(62, 77);
            this.usedTimeEB.Name = "usedTimeEB";
            this.usedTimeEB.ReadOnly = true;
            this.usedTimeEB.Size = new System.Drawing.Size(152, 20);
            this.usedTimeEB.TabIndex = 53;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 52;
            this.label4.Text = "Used Time";
            // 
            // setIOCB
            // 
            this.setIOCB.AutoSize = true;
            this.setIOCB.Location = new System.Drawing.Point(220, 78);
            this.setIOCB.Margin = new System.Windows.Forms.Padding(4);
            this.setIOCB.Name = "setIOCB";
            this.setIOCB.Size = new System.Drawing.Size(117, 17);
            this.setIOCB.TabIndex = 51;
            this.setIOCB.Text = "Light source switch";
            this.setIOCB.UseVisualStyleBackColor = true;
            this.setIOCB.CheckedChanged += new System.EventHandler(this.setIOCB_CheckedChanged);
            // 
            // SerialEB
            // 
            this.SerialEB.Location = new System.Drawing.Point(62, 48);
            this.SerialEB.Name = "SerialEB";
            this.SerialEB.ReadOnly = true;
            this.SerialEB.Size = new System.Drawing.Size(152, 20);
            this.SerialEB.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Serial";
            // 
            // TypeEB
            // 
            this.TypeEB.Location = new System.Drawing.Point(62, 20);
            this.TypeEB.Name = "TypeEB";
            this.TypeEB.ReadOnly = true;
            this.TypeEB.Size = new System.Drawing.Size(152, 20);
            this.TypeEB.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Type";
            // 
            // AssignBtn
            // 
            this.AssignBtn.Location = new System.Drawing.Point(220, 17);
            this.AssignBtn.Name = "AssignBtn";
            this.AssignBtn.Size = new System.Drawing.Size(82, 25);
            this.AssignBtn.TabIndex = 13;
            this.AssignBtn.Text = "Assign";
            this.AssignBtn.UseVisualStyleBackColor = true;
            this.AssignBtn.Click += new System.EventHandler(this.AssignBtn_Click);
            // 
            // TestSetGB
            // 
            this.TestSetGB.Controls.Add(this.GSCaliBtn);
            this.TestSetGB.Controls.Add(this.comboBox5);
            this.TestSetGB.Controls.Add(this.label9);
            this.TestSetGB.Controls.Add(this.NiCaliGB);
            this.TestSetGB.Location = new System.Drawing.Point(542, 12);
            this.TestSetGB.Name = "TestSetGB";
            this.TestSetGB.Size = new System.Drawing.Size(436, 530);
            this.TestSetGB.TabIndex = 12;
            this.TestSetGB.TabStop = false;
            this.TestSetGB.Text = "Calibration";
            // 
            // GSCaliBtn
            // 
            this.GSCaliBtn.Location = new System.Drawing.Point(203, 14);
            this.GSCaliBtn.Name = "GSCaliBtn";
            this.GSCaliBtn.Size = new System.Drawing.Size(104, 27);
            this.GSCaliBtn.TabIndex = 4;
            this.GSCaliBtn.Text = "Calibrate";
            this.GSCaliBtn.UseVisualStyleBackColor = true;
            this.GSCaliBtn.Click += new System.EventHandler(this.GSCaliBtn_Click);
            // 
            // comboBox5
            // 
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Items.AddRange(new object[] {
            "ND Filter",
            "Golden Unit"});
            this.comboBox5.Location = new System.Drawing.Point(60, 16);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(121, 21);
            this.comboBox5.TabIndex = 3;
            this.comboBox5.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Type";
            // 
            // NiCaliGB
            // 
            this.NiCaliGB.Controls.Add(this.NICaliLV);
            this.NiCaliGB.Controls.Add(this.button7);
            this.NiCaliGB.Controls.Add(this.comboBox2);
            this.NiCaliGB.Controls.Add(this.label6);
            this.NiCaliGB.Controls.Add(this.comboBox1);
            this.NiCaliGB.Controls.Add(this.label5);
            this.NiCaliGB.Location = new System.Drawing.Point(6, 51);
            this.NiCaliGB.Name = "NiCaliGB";
            this.NiCaliGB.Size = new System.Drawing.Size(424, 381);
            this.NiCaliGB.TabIndex = 1;
            this.NiCaliGB.TabStop = false;
            this.NiCaliGB.Text = "ND Fliter Calibration";
            // 
            // NICaliLV
            // 
            this.NICaliLV.FullRowSelect = true;
            this.NICaliLV.HideSelection = false;
            this.NICaliLV.Location = new System.Drawing.Point(11, 73);
            this.NICaliLV.MultiSelect = false;
            this.NICaliLV.Name = "NICaliLV";
            this.NICaliLV.Size = new System.Drawing.Size(407, 300);
            this.NICaliLV.TabIndex = 7;
            this.NICaliLV.UseCompatibleStateImageBehavior = false;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(275, 28);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(91, 23);
            this.button7.TabIndex = 6;
            this.button7.Text = "Calibrate";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "1",
            "2"});
            this.comboBox2.Location = new System.Drawing.Point(209, 29);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(60, 21);
            this.comboBox2.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(155, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Module ";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(71, 28);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(78, 21);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "ND Fliter";
            // 
            // ParametersGB
            // 
            this.ParametersGB.Controls.Add(this.button2);
            this.ParametersGB.Controls.Add(this.SetParameterBtn);
            this.ParametersGB.Controls.Add(this.SmoothPixEB);
            this.ParametersGB.Controls.Add(this.label74);
            this.ParametersGB.Controls.Add(this.label47);
            this.ParametersGB.Controls.Add(this.edtSaturationLevel);
            this.ParametersGB.Controls.Add(this.StartPixelEB);
            this.ParametersGB.Controls.Add(this.label67);
            this.ParametersGB.Controls.Add(this.label68);
            this.ParametersGB.Controls.Add(this.NumOfAvgEB);
            this.ParametersGB.Controls.Add(this.StopPixelEB);
            this.ParametersGB.Controls.Add(this.label69);
            this.ParametersGB.Controls.Add(this.label70);
            this.ParametersGB.Controls.Add(this.IntegrateTimeEB);
            this.ParametersGB.Location = new System.Drawing.Point(15, 119);
            this.ParametersGB.Name = "ParametersGB";
            this.ParametersGB.Size = new System.Drawing.Size(519, 120);
            this.ParametersGB.TabIndex = 19;
            this.ParametersGB.TabStop = false;
            this.ParametersGB.Text = "Parameters";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(380, 84);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(132, 25);
            this.button2.TabIndex = 51;
            this.button2.Text = "Get Spectrum";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // SetParameterBtn
            // 
            this.SetParameterBtn.Location = new System.Drawing.Point(380, 25);
            this.SetParameterBtn.Margin = new System.Windows.Forms.Padding(4);
            this.SetParameterBtn.Name = "SetParameterBtn";
            this.SetParameterBtn.Size = new System.Drawing.Size(132, 25);
            this.SetParameterBtn.TabIndex = 49;
            this.SetParameterBtn.Text = "Set Parameters";
            this.SetParameterBtn.UseVisualStyleBackColor = true;
            this.SetParameterBtn.Click += new System.EventHandler(this.SetParameterBtn_Click);
            // 
            // SmoothPixEB
            // 
            this.SmoothPixEB.Location = new System.Drawing.Point(286, 52);
            this.SmoothPixEB.Margin = new System.Windows.Forms.Padding(4);
            this.SmoothPixEB.Name = "SmoothPixEB";
            this.SmoothPixEB.Size = new System.Drawing.Size(50, 20);
            this.SmoothPixEB.TabIndex = 36;
            this.SmoothPixEB.Text = "0";
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Location = new System.Drawing.Point(185, 55);
            this.label74.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(90, 13);
            this.label74.TabIndex = 35;
            this.label74.Text = "Smoothing Pixels:";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(55, 27);
            this.label47.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(57, 13);
            this.label47.TabIndex = 23;
            this.label47.Text = "Start Pixel:";
            // 
            // edtSaturationLevel
            // 
            this.edtSaturationLevel.Location = new System.Drawing.Point(288, 84);
            this.edtSaturationLevel.Margin = new System.Windows.Forms.Padding(4);
            this.edtSaturationLevel.Name = "edtSaturationLevel";
            this.edtSaturationLevel.Size = new System.Drawing.Size(50, 20);
            this.edtSaturationLevel.TabIndex = 34;
            this.edtSaturationLevel.Text = "1";
            this.edtSaturationLevel.Visible = false;
            // 
            // StartPixelEB
            // 
            this.StartPixelEB.Location = new System.Drawing.Point(120, 22);
            this.StartPixelEB.Margin = new System.Windows.Forms.Padding(4);
            this.StartPixelEB.Name = "StartPixelEB";
            this.StartPixelEB.Size = new System.Drawing.Size(50, 20);
            this.StartPixelEB.TabIndex = 24;
            this.StartPixelEB.Text = "0";
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(173, 87);
            this.label67.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(105, 13);
            this.label67.TabIndex = 33;
            this.label67.Text = "Saturation detection:";
            this.label67.Visible = false;
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(56, 55);
            this.label68.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(57, 13);
            this.label68.TabIndex = 25;
            this.label68.Text = "Stop Pixel:";
            // 
            // NumOfAvgEB
            // 
            this.NumOfAvgEB.Location = new System.Drawing.Point(120, 84);
            this.NumOfAvgEB.Margin = new System.Windows.Forms.Padding(4);
            this.NumOfAvgEB.Name = "NumOfAvgEB";
            this.NumOfAvgEB.Size = new System.Drawing.Size(50, 20);
            this.NumOfAvgEB.TabIndex = 32;
            this.NumOfAvgEB.Text = "0";
            // 
            // StopPixelEB
            // 
            this.StopPixelEB.Location = new System.Drawing.Point(120, 52);
            this.StopPixelEB.Margin = new System.Windows.Forms.Padding(4);
            this.StopPixelEB.Name = "StopPixelEB";
            this.StopPixelEB.Size = new System.Drawing.Size(50, 20);
            this.StopPixelEB.TabIndex = 26;
            this.StopPixelEB.Text = "0";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(5, 87);
            this.label69.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(106, 13);
            this.label69.TabIndex = 31;
            this.label69.Text = "Number of averages:";
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Location = new System.Drawing.Point(190, 25);
            this.label70.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(86, 13);
            this.label70.TabIndex = 27;
            this.label70.Text = "Integration Time:";
            // 
            // IntegrateTimeEB
            // 
            this.IntegrateTimeEB.Location = new System.Drawing.Point(288, 22);
            this.IntegrateTimeEB.Margin = new System.Windows.Forms.Padding(4);
            this.IntegrateTimeEB.Name = "IntegrateTimeEB";
            this.IntegrateTimeEB.Size = new System.Drawing.Size(49, 20);
            this.IntegrateTimeEB.TabIndex = 28;
            this.IntegrateTimeEB.Text = "0";
            // 
            // DarkRefBtn
            // 
            this.DarkRefBtn.Location = new System.Drawing.Point(12, 245);
            this.DarkRefBtn.Name = "DarkRefBtn";
            this.DarkRefBtn.Size = new System.Drawing.Size(117, 22);
            this.DarkRefBtn.TabIndex = 20;
            this.DarkRefBtn.Text = "Dark Reference";
            this.DarkRefBtn.UseVisualStyleBackColor = true;
            this.DarkRefBtn.Click += new System.EventHandler(this.DarkRefBtn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(135, 245);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 22);
            this.button1.TabIndex = 21;
            this.button1.Text = "White Reference";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(258, 245);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(117, 22);
            this.button3.TabIndex = 22;
            this.button3.Text = "Transmission";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(381, 245);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(146, 22);
            this.button4.TabIndex = 23;
            this.button4.Text = "Start Continuous Spectrum";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // specTimer
            // 
            this.specTimer.Interval = 250;
            this.specTimer.Tick += new System.EventHandler(this.specTimer_Tick);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(339, 279);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 52;
            this.button5.Text = "Scale up";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(435, 279);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 53;
            this.button6.Text = "Scale Down";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // WinSpectrometer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1005, 554);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.DarkRefBtn);
            this.Controls.Add(this.ParametersGB);
            this.Controls.Add(this.TestSetGB);
            this.Controls.Add(this.SpecSettingsGB);
            this.Controls.Add(this.SpecChart);
            this.Controls.Add(this.ModLB);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WinSpectrometer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Spectrometer Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WinSpectrometer_FormClosing);
            this.Load += new System.EventHandler(this.WinSpectrometer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SpecChart)).EndInit();
            this.SpecSettingsGB.ResumeLayout(false);
            this.SpecSettingsGB.PerformLayout();
            this.TestSetGB.ResumeLayout(false);
            this.TestSetGB.PerformLayout();
            this.NiCaliGB.ResumeLayout(false);
            this.NiCaliGB.PerformLayout();
            this.ParametersGB.ResumeLayout(false);
            this.ParametersGB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox ModLB;
        private System.Windows.Forms.DataVisualization.Charting.Chart SpecChart;
        private System.Windows.Forms.GroupBox SpecSettingsGB;
        private System.Windows.Forms.Button AssignBtn;
        private System.Windows.Forms.TextBox TypeEB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SerialEB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox setIOCB;
        private System.Windows.Forms.TextBox usedTimeEB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox TestSetGB;
        private System.Windows.Forms.GroupBox ParametersGB;
        private System.Windows.Forms.Button SetParameterBtn;
        private System.Windows.Forms.TextBox SmoothPixEB;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TextBox edtSaturationLevel;
        private System.Windows.Forms.TextBox StartPixelEB;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.TextBox NumOfAvgEB;
        private System.Windows.Forms.TextBox StopPixelEB;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.TextBox IntegrateTimeEB;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button DarkRefBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Timer specTimer;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox NiCaliGB;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListView NICaliLV;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button GSCaliBtn;
    }
}