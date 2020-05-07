using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Festo_R2U_Package_YJKP
{
    public partial class Form_Init : Form
    {
        public Form_Init()
        {
            InitializeComponent();
        }

        public int DefaultWindowIndex
        {
            get
            {
                if (File.Exists("config.ini"))
                {
                    IniFile ini = new IniFile("config.ini");
                    if (!ini.KeyExists("DefaultWindowIndex"))
                    {
                        return 0;
                    }
                    string s_max = ini.Read("DefaultWindowIndex");
                    try
                    {
                        return Convert.ToInt16(s_max);
                    }
                    catch (Exception)
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                IniFile ini = new IniFile("config.ini");
                ini.Write("DefaultWindowIndex", value.ToString());
            }
        }

        public string WebLink
        {
            get
            {
                if (File.Exists("config.ini"))
                {
                    IniFile ini = new IniFile("config.ini");
                    if (!ini.KeyExists("WebLink"))
                    {
                        return "";
                    }
                    string s_max = ini.Read("WebLink");
                    try
                    {
                        return s_max;
                    }
                    catch (Exception)
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            set
            {
                IniFile ini = new IniFile("config.ini");
                ini.Write("WebLink", value.ToString());                
            }
        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {
            WebLink = textBox1.Text;
            DefaultWindowIndex = Convert.ToInt16(comboBox1.SelectedItem.ToString());
            switch (DefaultWindowIndex)
            {
                case 0:
                    Form_Web mForm_Web = new Form_Web(WebLink);
                    mForm_Web.Show();
                    this.Hide();
                    break;
                case 1:
                    new Form_Customized().Show();
                    this.Hide();
                    break;
                default:
                    break;
            }
        }

        private void Form_Init_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = DefaultWindowIndex;
            textBox1.Text = WebLink;
        }
    }
}
