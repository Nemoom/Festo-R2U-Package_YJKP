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
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new Form_Web(new Form_Init().WebLink).Show();
        }       
    }
}
