using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net.Config;
using System.IO;

namespace Festo_R2U_Package_YJKP
{
    public partial class Form_Customized : Form
    {
        public Form_Customized()
        {
            InitializeComponent();
            InitLog4Net();
        }

        private static void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }

        private void Form_Customized_Load(object sender, EventArgs e)
        {
            btn_CurrentCurve_Click(sender, e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new Form_Web(new Form_Init().WebLink).Show();
        }

        private void btn_CurrentCurve_Click(object sender, EventArgs e)
        {
            btn_CurrentCurve.BackColor = System.Drawing.SystemColors.InactiveCaption;
            btn_HIstoricalCurves.BackColor = System.Drawing.SystemColors.Control;
            panel_CUR.Location = panel1.Location;
            panel_CUR.Size = panel1.Size;
            panel_CUR.BringToFront();
        }

        private void btn_HIstoricalCurves_Click(object sender, EventArgs e)
        {
            btn_HIstoricalCurves.BackColor = System.Drawing.SystemColors.InactiveCaption;
            btn_CurrentCurve.BackColor = System.Drawing.SystemColors.Control;
            panel_HIST.Location = panel1.Location;
            panel_HIST.Size = panel1.Size;
            panel_HIST.BringToFront();
        }

        private void btn_CurrentCurve_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar=='s')
            {
                //弹出当前曲线设置窗口
            }
        }       
    }
}
