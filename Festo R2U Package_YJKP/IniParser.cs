﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Reflection;

namespace Festo_R2U_Package_YJKP
{
    public class IniParser
    {
        private Hashtable keyPairs = new Hashtable();
        private String iniFilePath;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        private struct SectionPair
        {
            public String Section;
            public String Key;
        }

        /// <summary>
        /// Opens the INI file at the given path and enumerates the values in the IniParser.
        /// </summary>
        /// <param name="iniPath">Full path to INI file.</param>
        public IniParser(String iniPath)
        {
            TextReader iniFile = null;
            String strLine = null;
            String currentRoot = null;
            String[] keyPair = null;

            iniFilePath = iniPath;

            if (File.Exists(iniPath))
            {
                try
                {
                    iniFile = new StreamReader(iniPath);

                    strLine = iniFile.ReadLine();

                    while (strLine != null)
                    {
                        strLine = strLine.Trim();

                        if (strLine != "")
                        {
                            if (strLine.StartsWith("[") && strLine.EndsWith("]"))
                            {
                                currentRoot = strLine.Substring(1, strLine.Length - 2);
                            }
                            else
                            {
                                keyPair = strLine.Split(new char[] { '=' }, 2);

                                SectionPair sectionPair;
                                String value = null;

                                if (currentRoot == null)
                                    currentRoot = "ROOT";

                                sectionPair.Section = currentRoot;
                                sectionPair.Key = keyPair[0];

                                if (keyPair.Length > 1)
                                    value = keyPair[1];

                                keyPairs.Add(sectionPair, value);
                            }
                        }

                        strLine = iniFile.ReadLine();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (iniFile != null)
                        iniFile.Close();
                }
            }
            else
                throw new FileNotFoundException("Unable to locate " + iniPath);

        }

        /// <summary>
        /// Returns the value for the given section, key pair.
        /// </summary>
        /// <param name="sectionName">Section name.</param>
        /// <param name="settingName">Key name.</param>
        public String GetSetting(String settingName, String sectionName = null)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName ?? EXE;
            sectionPair.Key = settingName;

            return (String)keyPairs[sectionPair];
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return GetSetting(Key,Section ?? EXE) != null;
        }
        /// <summary>
        /// Enumerates all lines for given section.
        /// </summary>
        /// <param name="sectionName">Section to enum.</param>
        public String[] EnumSection(String sectionName = null)
        {
            ArrayList tmpArray = new ArrayList();

            foreach (SectionPair pair in keyPairs.Keys)
            {
                if (pair.Section == (sectionName ?? EXE))
                    tmpArray.Add(pair.Key);
            }

            return (String[])tmpArray.ToArray(typeof(String));
        }

        /// <summary>
        /// Adds or replaces a setting to the table to be saved.
        /// </summary>
        /// <param name="sectionName">Section to add under.</param>
        /// <param name="settingName">Key name to add.</param>
        /// <param name="settingValue">Value of key.</param>
        public void AddSetting(String settingName, String settingValue, String sectionName = null)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName ?? EXE;
            sectionPair.Key = settingName;

            if (keyPairs.ContainsKey(sectionPair))
                keyPairs.Remove(sectionPair);

            keyPairs.Add(sectionPair, settingValue);
            SaveSettings();
        }

        ///// <summary>
        ///// Adds or replaces a setting to the table to be saved with a null value.
        ///// </summary>
        ///// <param name="sectionName">Section to add under.</param>
        ///// <param name="settingName">Key name to add.</param>
        //public void AddSetting(String settingName, String sectionName = null)
        //{
        //    AddSetting(settingName, null, sectionName ?? EXE);
        //    SaveSettings();
        //}

        /// <summary>
        /// Remove a setting.
        /// </summary>
        /// <param name="sectionName">Section to add under.</param>
        /// <param name="settingName">Key name to add.</param>
        public void DeleteSetting(String settingName, String sectionName = null)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName ?? EXE;
            sectionPair.Key = settingName;

            if (keyPairs.ContainsKey(sectionPair))
                keyPairs.Remove(sectionPair);
            SaveSettings();
        }

        /// <summary>
        /// Save settings to new file.
        /// </summary>
        /// <param name="newFilePath">New file path.</param>
        public void SaveSettings(String newFilePath)
        {
            ArrayList sections = new ArrayList();
            String tmpValue = "";
            String strToSave = "";

            foreach (SectionPair sectionPair in keyPairs.Keys)
            {
                if (!sections.Contains(sectionPair.Section))
                    sections.Add(sectionPair.Section);
            }

            foreach (String section in sections)
            {
                strToSave += ("[" + section + "]\r\n");

                foreach (SectionPair sectionPair in keyPairs.Keys)
                {
                    if (sectionPair.Section == section)
                    {
                        tmpValue = (String)keyPairs[sectionPair];

                        if (tmpValue != null)
                            tmpValue = "=" + tmpValue;

                        strToSave += (sectionPair.Key + tmpValue + "\r\n");
                    }
                }

                strToSave += "\r\n";
            }

            try
            {
                TextWriter tw = new StreamWriter(newFilePath);
                tw.Write(strToSave);
                tw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save settings back to ini file.
        /// </summary>
        public void SaveSettings()
        {
            SaveSettings(iniFilePath);
        }
    }
}
