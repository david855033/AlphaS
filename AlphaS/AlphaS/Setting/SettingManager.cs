﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AlphaS.Settings
{
    public class SettingManager : ISettingManager
    {
        readonly string SETTING_FILE_PATH;
        public SettingManager(string settingFilePath)
        {
            SETTING_FILE_PATH = settingFilePath;
            loadSettings();
        }

        Dictionary<string, string> settings = new Dictionary<string, string>();

        private void loadSettings()
        {
            bool isSettingFileExist = File.Exists(SETTING_FILE_PATH);
            if (isSettingFileExist)
            {
                using (StreamReader sr = new StreamReader(SETTING_FILE_PATH, Encoding.Default))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] spltline = sr.ReadLine().Split('=');
                        settings.Add(spltline.First(), spltline.Last());
                    }
                }
            }
        }

        public string getSetting(string fieldName)
        {
            if (settings.ContainsKey(fieldName))
            {
                return settings[fieldName];
            }
            return "";
        }

        public void saveSetting(string fieldName, string Content)
        {
            if (settings.ContainsKey(fieldName))
            {
                settings[fieldName] = Content;
            }
            else
            {
                settings.Add(fieldName, Content);
            }

            StringBuilder newContent = new StringBuilder();
            foreach (var s in settings)
            {
                newContent.AppendLine(s.Key + "=" + s.Value);
            }
            using (var sw = new StreamWriter(SETTING_FILE_PATH, false, Encoding.Default))
            {
                sw.Write(newContent.ToString());
            }
        }


    }
}
