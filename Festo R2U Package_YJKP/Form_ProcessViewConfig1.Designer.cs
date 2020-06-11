namespace Festo_R2U_Package_YJKP
{
    partial class Form_ProcessViewConfig1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_Submit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtn_byCount = new System.Windows.Forms.RadioButton();
            this.txt_Files = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rbtn_byProgramName = new System.Windows.Forms.RadioButton();
            this.rbtn_byMonth = new System.Windows.Forms.RadioButton();
            this.rbtn_byDay = new System.Windows.Forms.RadioButton();
            this.rbtn_byWeek = new System.Windows.Forms.RadioButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 276F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btn_Submit, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 206F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(816, 385);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "压装日志保存路径:";
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(313, 41);
            this.textBox1.Margin = new System.Windows.Forms.Padding(5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(466, 36);
            this.textBox1.TabIndex = 3;
            this.textBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDoubleClick);
            // 
            // btn_Submit
            // 
            this.btn_Submit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Submit.Location = new System.Drawing.Point(313, 302);
            this.btn_Submit.Margin = new System.Windows.Forms.Padding(5);
            this.btn_Submit.Name = "btn_Submit";
            this.btn_Submit.Size = new System.Drawing.Size(122, 42);
            this.btn_Submit.TabIndex = 4;
            this.btn_Submit.Text = "提交";
            this.btn_Submit.UseVisualStyleBackColor = true;
            this.btn_Submit.Click += new System.EventHandler(this.btn_Submit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 81);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 29);
            this.label2.TabIndex = 5;
            this.label2.Text = "文件夹整理方式:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.rbtn_byProgramName, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.rbtn_byMonth, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.rbtn_byDay, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbtn_byWeek, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(311, 84);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(470, 200);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 194F));
            this.tableLayoutPanel4.Controls.Add(this.rbtn_byCount, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.txt_Files, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 160);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(320, 38);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // rbtn_byCount
            // 
            this.rbtn_byCount.AutoCheck = false;
            this.rbtn_byCount.AutoSize = true;
            this.rbtn_byCount.Location = new System.Drawing.Point(3, 3);
            this.rbtn_byCount.Name = "rbtn_byCount";
            this.rbtn_byCount.Size = new System.Drawing.Size(53, 32);
            this.rbtn_byCount.TabIndex = 0;
            this.rbtn_byCount.TabStop = true;
            this.rbtn_byCount.Text = "每";
            this.rbtn_byCount.UseVisualStyleBackColor = true;
            this.rbtn_byCount.Visible = false;
            this.rbtn_byCount.Click += new System.EventHandler(this.rbtn_byCount_Click);
            // 
            // txt_Files
            // 
            this.txt_Files.Location = new System.Drawing.Point(62, 0);
            this.txt_Files.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.txt_Files.Name = "txt_Files";
            this.txt_Files.Size = new System.Drawing.Size(61, 36);
            this.txt_Files.TabIndex = 1;
            this.txt_Files.Text = "3000";
            this.txt_Files.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(126, 5);
            this.label4.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 29);
            this.label4.TabIndex = 2;
            this.label4.Text = "个一文件夹";
            this.label4.Visible = false;
            // 
            // rbtn_byProgramName
            // 
            this.rbtn_byProgramName.AutoCheck = false;
            this.rbtn_byProgramName.AutoSize = true;
            this.rbtn_byProgramName.Location = new System.Drawing.Point(3, 123);
            this.rbtn_byProgramName.Name = "rbtn_byProgramName";
            this.rbtn_byProgramName.Size = new System.Drawing.Size(190, 33);
            this.rbtn_byProgramName.TabIndex = 2;
            this.rbtn_byProgramName.TabStop = true;
            this.rbtn_byProgramName.Text = "按程序号分类";
            this.rbtn_byProgramName.UseVisualStyleBackColor = true;
            this.rbtn_byProgramName.Click += new System.EventHandler(this.rbtn_byProgramName_Click);
            // 
            // rbtn_byMonth
            // 
            this.rbtn_byMonth.AutoCheck = false;
            this.rbtn_byMonth.AutoSize = true;
            this.rbtn_byMonth.Location = new System.Drawing.Point(3, 83);
            this.rbtn_byMonth.Name = "rbtn_byMonth";
            this.rbtn_byMonth.Size = new System.Drawing.Size(216, 33);
            this.rbtn_byMonth.TabIndex = 1;
            this.rbtn_byMonth.TabStop = true;
            this.rbtn_byMonth.Text = "每月新建文件夹";
            this.rbtn_byMonth.UseVisualStyleBackColor = true;
            this.rbtn_byMonth.Click += new System.EventHandler(this.rbtn_byMonth_Click);
            // 
            // rbtn_byDay
            // 
            this.rbtn_byDay.AutoCheck = false;
            this.rbtn_byDay.AutoSize = true;
            this.rbtn_byDay.Location = new System.Drawing.Point(3, 3);
            this.rbtn_byDay.Name = "rbtn_byDay";
            this.rbtn_byDay.Size = new System.Drawing.Size(216, 33);
            this.rbtn_byDay.TabIndex = 0;
            this.rbtn_byDay.TabStop = true;
            this.rbtn_byDay.Text = "每天新建文件夹";
            this.rbtn_byDay.UseVisualStyleBackColor = true;
            this.rbtn_byDay.Click += new System.EventHandler(this.rbtn_byDays_Click);
            // 
            // rbtn_byWeek
            // 
            this.rbtn_byWeek.AutoCheck = false;
            this.rbtn_byWeek.AutoSize = true;
            this.rbtn_byWeek.Location = new System.Drawing.Point(3, 43);
            this.rbtn_byWeek.Name = "rbtn_byWeek";
            this.rbtn_byWeek.Size = new System.Drawing.Size(216, 33);
            this.rbtn_byWeek.TabIndex = 6;
            this.rbtn_byWeek.TabStop = true;
            this.rbtn_byWeek.Text = "每周新建文件夹";
            this.rbtn_byWeek.UseVisualStyleBackColor = true;
            this.rbtn_byWeek.Click += new System.EventHandler(this.rbtn_byWeek_Click);
            // 
            // Form_ProcessViewConfig1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 385);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("MetaPlusLF", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_ProcessViewConfig1";
            this.Text = "配置";
            this.Load += new System.EventHandler(this.Form_ProcessViewConfig_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_Submit;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.RadioButton rbtn_byCount;
        private System.Windows.Forms.TextBox txt_Files;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rbtn_byMonth;
        private System.Windows.Forms.RadioButton rbtn_byProgramName;
        private System.Windows.Forms.RadioButton rbtn_byDay;
        private System.Windows.Forms.RadioButton rbtn_byWeek;
    }
}

