using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;
using Kun.Data;

namespace Kun.Tool
{
	public class ParseManager 
	{
		public void ParseSettings()
		{
			DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath);
			DirInfoParent = dirInfo.Parent.FullName;

			cubeSetting = JsonLoader<CubeSetting> ();
		}
		
		[SerializeField][ReadOnly]
		CubeSetting cubeSetting;

		public CubeSetting CubeSetting
		{
			get
			{
				return cubeSetting;
			}
		}

        /// <summary>
        /// 若檔名與泛型名一樣 則直接套用
        /// </summary>
        /// <returns>The loader.</returns>
        /// <param name="dataName">Data name.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T JsonLoader<T>(string dataName = "")
        {
            if (dataName == "")
            {
                Type type = typeof(T);

                dataName = type.Name;
            }

            var allLines = "";

            DirectoryInfo settingdirInfo = new DirectoryInfo(SettingFolderPath);
            string filePath = Path.Combine(settingdirInfo.FullName, dataName + ".json");

			if (File.Exists(filePath))
			{
				using (StreamReader sr = new StreamReader(filePath))
				{
					allLines = sr.ReadToEnd();
				}

				if (string.IsNullOrEmpty(allLines))
				{
					Debug.LogErrorFormat("無法讀取設定! 請檢查{0}是否存在!", dataName);
				}
			}
			else
			{
				throw new Exception (string.Format ("路徑不存在{0}", filePath));
			}

            T process = JsonUtility.FromJson<T>(allLines);

            return process;
        }

		static string DirInfoParent;

        public const string SettingPath = "GameSetting";

        static string settingfolderPath = "";

        public static string SettingFolderPath
        {
            get
            {
                if (string.IsNullOrEmpty(settingfolderPath))
                {
					settingfolderPath = Path.Combine(DirInfoParent, SettingPath);
                }

                return settingfolderPath;
            }
        }

		public void AppendPlayerHistoryGroup (PlayHistoryGroup data)
		{	
			using (StreamWriter sw = new StreamWriter (HistoryFullPath, true))
			{
				sw.WriteLine (JsonUtility.ToJson (data));
			}
		}

		const string HistoryPath = "hisp.kp";

		static string historyFullPath;

		public static string HistoryFullPath
		{
			get
			{
				if (string.IsNullOrEmpty(historyFullPath))
				{
					historyFullPath = Path.Combine (DirInfoParent, HistoryPath);
				}

				return historyFullPath;
			}
		}
    }
}
