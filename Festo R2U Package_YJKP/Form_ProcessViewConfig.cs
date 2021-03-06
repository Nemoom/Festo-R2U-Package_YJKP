﻿using System;
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
    public partial class Form_ProcessViewConfig : Form
    {
        string CurProgramName = "";
        Form_Customized MainForm;
        string[] ProgramNames;

        public Form_ProcessViewConfig(Form_Customized mForm,string ProgramName = "")
        {
            InitializeComponent();
            CurProgramName = ProgramName;
            MainForm = mForm;
        }

        public string WatchPath
        {
            get
            {
                if (File.Exists("config.ini"))
                {
                    IniParser ini = new IniParser("config.ini");
                    if (!ini.KeyExists("WatchPath"))
                    {
                        return "";
                    }
                    string s_Path = ini.GetSetting("WatchPath");
                    try
                    {
                        return s_Path;
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
                IniParser ini = new IniParser("config.ini");
                ini.AddSetting("WatchPath", value.ToString());                
            }
        }

        public double K_Threshold
        {
            get
            {
                if (File.Exists("config.ini"))
                {
                    IniFile ini = new IniFile("config.ini");
                    if (!ini.KeyExists("K_Threshold", CurProgramName))
                    {
                        return -1;
                    }
                    string s_max = ini.Read("K_Threshold", CurProgramName);
                    try
                    {
                        return Convert.ToDouble(s_max);
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                IniFile ini = new IniFile("config.ini");
                ini.Write("K_Threshold", value.ToString(),CurProgramName);
            }
        }

        public double Min_X
        {
            get
            {
                if (File.Exists("config.ini"))
                {
                    IniFile ini = new IniFile("config.ini");
                    if (!ini.KeyExists("Min_X", CurProgramName))
                    {
                        return -1;
                    }
                    string s_max = ini.Read("Min_X", CurProgramName);
                    try
                    {
                        return Convert.ToDouble(s_max);
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                IniFile ini = new IniFile("config.ini");
                ini.Write("Min_X", value.ToString(), CurProgramName);
            }
        }

        public int Continuity
        {
            get
            {
                if (File.Exists("config.ini"))
                {
                    IniFile ini = new IniFile("config.ini");
                    if (!ini.KeyExists("Continuity", CurProgramName))
                    {
                        return -1;
                    }
                    string s_max = ini.Read("Continuity", CurProgramName);
                    try
                    {
                        return Convert.ToInt16(s_max);
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                IniFile ini = new IniFile("config.ini");
                ini.Write("Continuity", value.ToString(), CurProgramName);
            }
        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {
            WatchPath = textBox1.Text;
            if (comboBox1.Text!="")
            {
                CurProgramName = comboBox1.Text;
                new IniParser("config.ini").AddSetting(CurProgramName, null, "Programs");
                K_Threshold = Convert.ToDouble(nUD_K.Value);
                Continuity = Convert.ToInt16(nUD_Continuity.Value);
                Min_X = Convert.ToDouble(nUD_MinX_Value.Value);
            }
            MainForm.fileSystemWatcher1.Path = WatchPath;    
            MainForm.fileSystemWatcher1.EnableRaisingEvents = true;
        }        

        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {                
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurProgramName = comboBox1.SelectedItem.ToString();
            nUD_K.Value = Convert.ToDecimal(K_Threshold);
            nUD_Continuity.Value = Convert.ToDecimal(Continuity);
            nUD_MinX_Value.Value = Convert.ToDecimal(Min_X);
        }

        private void Form_ProcessViewConfig_Load(object sender, EventArgs e)
        {
            ProgramNames = new IniParser("config.ini").EnumSection("Programs");
            for (int i = 0; i < ProgramNames.Length; i++)
            {
                comboBox1.Items.Add(ProgramNames[i]);
            }
            textBox1.Text = WatchPath;
            if (CurProgramName != "")
            {
                comboBox1.SelectedItem = CurProgramName;
                nUD_K.Value = Convert.ToDecimal(K_Threshold);
                nUD_Continuity.Value = Convert.ToDecimal(Continuity);
                nUD_MinX_Value.Value = Convert.ToDecimal(Min_X);
            }
        }
    }
}
