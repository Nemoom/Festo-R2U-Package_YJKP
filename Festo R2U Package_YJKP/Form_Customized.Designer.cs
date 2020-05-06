namespace Festo_R2U_Package_YJKP
{
    partial class Form_Customized
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Customized));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel_CUR = new System.Windows.Forms.Panel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel_Tools = new System.Windows.Forms.Panel();
            this.btn_CaptureDIsplay = new System.Windows.Forms.Button();
            this.btn_BundlePlot = new System.Windows.Forms.Button();
            this.btn_Move = new System.Windows.Forms.Button();
            this.btn_Reduce = new System.Windows.Forms.Button();
            this.btn_Enlarge = new System.Windows.Forms.Button();
            this.btn_AutoZoom = new System.Windows.Forms.Button();
            this.tableLayoutPanel_ProcessView = new System.Windows.Forms.TableLayoutPanel();
            this.panel_HIST = new System.Windows.Forms.Panel();
            this.tableLayoutPanel_HIST = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_CurrentCurve = new System.Windows.Forms.Button();
            this.btn_HIstoricalCurves = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel_Count = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_CountOK = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_CountNG = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_CountTotal = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_Result = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel_CUR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel_Tools.SuspendLayout();
            this.panel_HIST.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel_Count.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(868, 509);
            this.splitContainer1.SplitterDistance = 654;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel_CUR);
            this.panel1.Controls.Add(this.panel_HIST);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(650, 437);
            this.panel1.TabIndex = 1;
            // 
            // panel_CUR
            // 
            this.panel_CUR.Controls.Add(this.chart1);
            this.panel_CUR.Controls.Add(this.panel_Tools);
            this.panel_CUR.Controls.Add(this.tableLayoutPanel_ProcessView);
            this.panel_CUR.Location = new System.Drawing.Point(72, 21);
            this.panel_CUR.Name = "panel_CUR";
            this.panel_CUR.Size = new System.Drawing.Size(544, 351);
            this.panel_CUR.TabIndex = 0;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(493, 313);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // panel_Tools
            // 
            this.panel_Tools.AutoScroll = true;
            this.panel_Tools.Controls.Add(this.btn_CaptureDIsplay);
            this.panel_Tools.Controls.Add(this.btn_BundlePlot);
            this.panel_Tools.Controls.Add(this.btn_Move);
            this.panel_Tools.Controls.Add(this.btn_Reduce);
            this.panel_Tools.Controls.Add(this.btn_Enlarge);
            this.panel_Tools.Controls.Add(this.btn_AutoZoom);
            this.panel_Tools.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Tools.Location = new System.Drawing.Point(493, 0);
            this.panel_Tools.Name = "panel_Tools";
            this.panel_Tools.Size = new System.Drawing.Size(51, 313);
            this.panel_Tools.TabIndex = 1;
            // 
            // btn_CaptureDIsplay
            // 
            this.btn_CaptureDIsplay.BackgroundImage = global::Festo_R2U_Package_YJKP.Properties.Resources.曲线对比__2_;
            this.btn_CaptureDIsplay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_CaptureDIsplay.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_CaptureDIsplay.Location = new System.Drawing.Point(0, 255);
            this.btn_CaptureDIsplay.Name = "btn_CaptureDIsplay";
            this.btn_CaptureDIsplay.Size = new System.Drawing.Size(51, 51);
            this.btn_CaptureDIsplay.TabIndex = 5;
            this.btn_CaptureDIsplay.UseVisualStyleBackColor = true;
            // 
            // btn_BundlePlot
            // 
            this.btn_BundlePlot.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_BundlePlot.BackgroundImage")));
            this.btn_BundlePlot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_BundlePlot.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_BundlePlot.Location = new System.Drawing.Point(0, 204);
            this.btn_BundlePlot.Name = "btn_BundlePlot";
            this.btn_BundlePlot.Size = new System.Drawing.Size(51, 51);
            this.btn_BundlePlot.TabIndex = 4;
            this.btn_BundlePlot.UseVisualStyleBackColor = true;
            // 
            // btn_Move
            // 
            this.btn_Move.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Move.BackgroundImage")));
            this.btn_Move.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Move.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_Move.Location = new System.Drawing.Point(0, 153);
            this.btn_Move.Name = "btn_Move";
            this.btn_Move.Size = new System.Drawing.Size(51, 51);
            this.btn_Move.TabIndex = 3;
            this.btn_Move.UseVisualStyleBackColor = true;
            // 
            // btn_Reduce
            // 
            this.btn_Reduce.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Reduce.BackgroundImage")));
            this.btn_Reduce.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Reduce.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_Reduce.Location = new System.Drawing.Point(0, 102);
            this.btn_Reduce.Name = "btn_Reduce";
            this.btn_Reduce.Size = new System.Drawing.Size(51, 51);
            this.btn_Reduce.TabIndex = 2;
            this.btn_Reduce.UseVisualStyleBackColor = true;
            // 
            // btn_Enlarge
            // 
            this.btn_Enlarge.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Enlarge.BackgroundImage")));
            this.btn_Enlarge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Enlarge.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_Enlarge.Location = new System.Drawing.Point(0, 51);
            this.btn_Enlarge.Name = "btn_Enlarge";
            this.btn_Enlarge.Size = new System.Drawing.Size(51, 51);
            this.btn_Enlarge.TabIndex = 1;
            this.btn_Enlarge.UseVisualStyleBackColor = true;
            // 
            // btn_AutoZoom
            // 
            this.btn_AutoZoom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_AutoZoom.BackgroundImage")));
            this.btn_AutoZoom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_AutoZoom.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_AutoZoom.Location = new System.Drawing.Point(0, 0);
            this.btn_AutoZoom.Name = "btn_AutoZoom";
            this.btn_AutoZoom.Size = new System.Drawing.Size(51, 51);
            this.btn_AutoZoom.TabIndex = 0;
            this.btn_AutoZoom.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel_ProcessView
            // 
            this.tableLayoutPanel_ProcessView.ColumnCount = 2;
            this.tableLayoutPanel_ProcessView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_ProcessView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_ProcessView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel_ProcessView.Location = new System.Drawing.Point(0, 313);
            this.tableLayoutPanel_ProcessView.Name = "tableLayoutPanel_ProcessView";
            this.tableLayoutPanel_ProcessView.RowCount = 1;
            this.tableLayoutPanel_ProcessView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_ProcessView.Size = new System.Drawing.Size(544, 38);
            this.tableLayoutPanel_ProcessView.TabIndex = 0;
            this.tableLayoutPanel_ProcessView.Visible = false;
            // 
            // panel_HIST
            // 
            this.panel_HIST.Controls.Add(this.tableLayoutPanel_HIST);
            this.panel_HIST.Location = new System.Drawing.Point(30, 123);
            this.panel_HIST.Name = "panel_HIST";
            this.panel_HIST.Size = new System.Drawing.Size(529, 293);
            this.panel_HIST.TabIndex = 1;
            // 
            // tableLayoutPanel_HIST
            // 
            this.tableLayoutPanel_HIST.ColumnCount = 1;
            this.tableLayoutPanel_HIST.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_HIST.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_HIST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_HIST.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_HIST.Name = "tableLayoutPanel_HIST";
            this.tableLayoutPanel_HIST.RowCount = 3;
            this.tableLayoutPanel_HIST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel_HIST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_HIST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel_HIST.Size = new System.Drawing.Size(529, 293);
            this.tableLayoutPanel_HIST.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btn_CurrentCurve, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_HIstoricalCurves, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 437);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(650, 68);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btn_CurrentCurve
            // 
            this.btn_CurrentCurve.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btn_CurrentCurve.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_CurrentCurve.Location = new System.Drawing.Point(3, 3);
            this.btn_CurrentCurve.Name = "btn_CurrentCurve";
            this.btn_CurrentCurve.Size = new System.Drawing.Size(319, 62);
            this.btn_CurrentCurve.TabIndex = 0;
            this.btn_CurrentCurve.Text = "当前曲线";
            this.btn_CurrentCurve.UseVisualStyleBackColor = false;
            this.btn_CurrentCurve.Click += new System.EventHandler(this.btn_CurrentCurve_Click);
            this.btn_CurrentCurve.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btn_CurrentCurve_KeyPress);
            // 
            // btn_HIstoricalCurves
            // 
            this.btn_HIstoricalCurves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_HIstoricalCurves.Location = new System.Drawing.Point(328, 3);
            this.btn_HIstoricalCurves.Name = "btn_HIstoricalCurves";
            this.btn_HIstoricalCurves.Size = new System.Drawing.Size(319, 62);
            this.btn_HIstoricalCurves.TabIndex = 1;
            this.btn_HIstoricalCurves.Text = "历史曲线";
            this.btn_HIstoricalCurves.UseVisualStyleBackColor = true;
            this.btn_HIstoricalCurves.Click += new System.EventHandler(this.btn_HIstoricalCurves_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel_Count);
            this.splitContainer2.Panel1.Controls.Add(this.lbl_Result);
            this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer2.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer2.Panel2.Margin = new System.Windows.Forms.Padding(3);
            this.splitContainer2.Panel2.Padding = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.splitContainer2.Size = new System.Drawing.Size(210, 509);
            this.splitContainer2.SplitterDistance = 435;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel_Count
            // 
            this.panel_Count.Controls.Add(this.label4);
            this.panel_Count.Controls.Add(this.label1);
            this.panel_Count.Controls.Add(this.lbl_CountOK);
            this.panel_Count.Controls.Add(this.label2);
            this.panel_Count.Controls.Add(this.label6);
            this.panel_Count.Controls.Add(this.lbl_CountNG);
            this.panel_Count.Controls.Add(this.label3);
            this.panel_Count.Controls.Add(this.lbl_CountTotal);
            this.panel_Count.Controls.Add(this.label5);
            this.panel_Count.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Count.Location = new System.Drawing.Point(0, 67);
            this.panel_Count.Name = "panel_Count";
            this.panel_Count.Size = new System.Drawing.Size(206, 113);
            this.panel_Count.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "OK:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(154, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "pcs";
            // 
            // lbl_CountOK
            // 
            this.lbl_CountOK.Location = new System.Drawing.Point(107, 15);
            this.lbl_CountOK.Name = "lbl_CountOK";
            this.lbl_CountOK.Size = new System.Drawing.Size(50, 17);
            this.lbl_CountOK.TabIndex = 6;
            this.lbl_CountOK.Text = "0";
            this.lbl_CountOK.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(154, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "pcs";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "TOTAL:";
            // 
            // lbl_CountNG
            // 
            this.lbl_CountNG.Location = new System.Drawing.Point(107, 48);
            this.lbl_CountNG.Name = "lbl_CountNG";
            this.lbl_CountNG.Size = new System.Drawing.Size(50, 17);
            this.lbl_CountNG.TabIndex = 7;
            this.lbl_CountNG.Text = "0";
            this.lbl_CountNG.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(154, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "pcs";
            // 
            // lbl_CountTotal
            // 
            this.lbl_CountTotal.Location = new System.Drawing.Point(107, 81);
            this.lbl_CountTotal.Name = "lbl_CountTotal";
            this.lbl_CountTotal.Size = new System.Drawing.Size(50, 17);
            this.lbl_CountTotal.TabIndex = 8;
            this.lbl_CountTotal.Text = "0";
            this.lbl_CountTotal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "NG:";
            // 
            // lbl_Result
            // 
            this.lbl_Result.BackColor = System.Drawing.Color.ForestGreen;
            this.lbl_Result.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_Result.Font = new System.Drawing.Font("MetaPlusLF", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Result.ForeColor = System.Drawing.Color.Black;
            this.lbl_Result.Location = new System.Drawing.Point(0, 0);
            this.lbl_Result.Name = "lbl_Result";
            this.lbl_Result.Size = new System.Drawing.Size(206, 67);
            this.lbl_Result.TabIndex = 0;
            this.lbl_Result.Text = "OK";
            this.lbl_Result.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(9, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(188, 46);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Form_Customized
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 509);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("MetaPlusLF", 8F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_Customized";
            this.Text = "Festo servo press kit";
            this.Load += new System.EventHandler(this.Form_Customized_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel_CUR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel_Tools.ResumeLayout(false);
            this.panel_HIST.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel_Count.ResumeLayout(false);
            this.panel_Count.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbl_Result;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_CountOK;
        private System.Windows.Forms.Label lbl_CountNG;
        private System.Windows.Forms.Label lbl_CountTotal;
        private System.Windows.Forms.Panel panel_Count;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_CurrentCurve;
        private System.Windows.Forms.Button btn_HIstoricalCurves;
        private System.Windows.Forms.Panel panel_CUR;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_ProcessView;
        private System.Windows.Forms.Panel panel_HIST;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_HIST;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Panel panel_Tools;
        private System.Windows.Forms.Button btn_AutoZoom;
        private System.Windows.Forms.Button btn_CaptureDIsplay;
        private System.Windows.Forms.Button btn_BundlePlot;
        private System.Windows.Forms.Button btn_Move;
        private System.Windows.Forms.Button btn_Reduce;
        private System.Windows.Forms.Button btn_Enlarge;
    }
}