using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Kun.Tool
{
	public class BuildWindow : EditorWindow
    {
		static bool developmentBuild = false;
		static bool copyPDB_File = false;
		
		static List<string> oldTexts;
		
		[MenuItem("Tool/Build Windows")]
		static void ShowWindow ()
        {
			BuildWindow window = GetWindow<BuildWindow> ("Build Window");
			window.minSize = window.maxSize = new Vector2 (400f, 150f);
			window.Show ();
		}

		void OnGUI ()
		{
			EditorGUILayout.LabelField ("Build Settings");
			developmentBuild = EditorGUILayout.Toggle ("Development Build", developmentBuild);
			copyPDB_File = EditorGUILayout.Toggle ("Copy PDB File", copyPDB_File);

			EditorGUILayout.Space ();

            //透過UI發佈
			if (GUILayout.Button("Build"))
            {
				BuildFromUI (developmentBuild, copyPDB_File);
            }
        }

        static void BuildFromCI()
        {
			List<string> commandLineArgs = new List<string> (System.Environment.GetCommandLineArgs ());

			string rootPath = "";

			int rootPathIndex = commandLineArgs.FindIndex (x => x == "-destinationPath");

			if (rootPathIndex > 0) 
			{
				rootPath = commandLineArgs [rootPathIndex + 1];
			} 
			else
            {
				throw new UnityException("can't find -destinationPath");
            }

			bool _developmentBuild = false;

			int developmentBuildIndex = commandLineArgs.FindIndex (x => x == "-developmentBuild");

            if (developmentBuildIndex > 0)
            {
				string _developmentBuildGetPar = commandLineArgs [developmentBuildIndex + 1];
				_developmentBuild = ConvertMsgToBool (_developmentBuildGetPar);
            }
            else
            {
				throw new UnityException ("can't find -developmentBuild");
            }

			bool _copyPDB_File = false;

			int copyPDB_FileIndex = commandLineArgs.FindIndex (x => x == "-copyPDB_File");

            if (copyPDB_FileIndex > 0)
            {
				string _copyPDB_File_GetPar = commandLineArgs [copyPDB_FileIndex + 1];
				_copyPDB_File = ConvertMsgToBool (_copyPDB_File_GetPar);
            }
            else
            {
				throw new UnityException ("can't find -_copyPDB_File");
            }


			Build (_developmentBuild, _copyPDB_File, rootPath);
        }

        static bool ConvertMsgToBool(string input)
        {
			string processInput = input.ToLower ();

            if (processInput == "true")
            {
				return true;
            }
            else if (processInput == "false")
            {
                return false;
            }
            else
            {
				throw new UnityException ("can't parse {input}");
            }
        }

        static void BuildFromUI(bool developmentBuild, bool copyPDB_File)
        {
			string defaultRootPath = PlayerPrefs.GetString ("BuildRootPath", string.Empty);
			string rootPath = EditorUtility.SaveFolderPanel ("Choose location for root directory", defaultRootPath, string.Empty);
			Build (developmentBuild, copyPDB_File, rootPath);
        }

        static void Build(bool developmentBuild, bool copyPDB_File, string rootPath)
        {
            if (string.IsNullOrEmpty(rootPath))
            {
				return;
            }
            else
            {
				DirectoryInfo dirInfo = new DirectoryInfo (rootPath);
				PlayerPrefs.SetString ("BuildRootPath", dirInfo.Parent.FullName);
            }

            PlayerSettings.defaultIsFullScreen = true;

			string buildPath = GetBuildPath (rootPath);
            if (developmentBuild)
            {
				BuildPipeline.BuildPlayer (GetAllScenePaths (), buildPath,
					BuildTarget.StandaloneWindows64, BuildOptions.Development | BuildOptions.ShowBuiltPlayer | BuildOptions.ConnectWithProfiler);
            }
            else
            {
				BuildPipeline.BuildPlayer (GetAllScenePaths (), buildPath,
					BuildTarget.StandaloneWindows64, BuildOptions.ShowBuiltPlayer);
            }

            if (copyPDB_File)
            {
				EditorUserBuildSettings.SetPlatformSettings ("Standalone", "CopyPDBFiles", "true");
            }
            else
            {
				EditorUserBuildSettings.SetPlatformSettings ("Standalone", "CopyPDBFiles", "false");
            }

			CopySetting (buildPath);

			PlayerSettings.defaultIsFullScreen = false;
        }

		static string[] GetAllScenePaths ()
        {
			List<string> scenes = new List<string>();

            for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
            {
				if (EditorBuildSettings.scenes [i].enabled)
                {
					scenes.Add (EditorBuildSettings.scenes [i].path);
                }
            }

			return scenes.ToArray ();
        }

		static string GetBuildPath (string rootPath)
        {
			string productName = PlayerSettings.productName;

			return rootPath + string.Format ("/{0}", productName) + string.Format ("/{0}.exe", productName);
        }

        /// <summary>
        /// 這邊傳入的是exe檔位置 因為專案名稱會變 傳入root folder名稱不準 所以傳入資料夾名稱 往上找一層
        /// 比傳入root 再往下傳一層好
        /// </summary>
        /// <param name="outputPath">Output path.</param>
		static void CopySetting(string outputPath)
        {
			DirectoryInfo outputInfo = new DirectoryInfo (outputPath);

			DirectoryInfo sourceInfo = new DirectoryInfo (Application.dataPath);
            #region 生成資料夾
			CopyDirectory(sourceInfo, outputInfo, "GameSetting");
            #endregion
        }

		static void CopyDirectory (DirectoryInfo sourceDirectoryInfo, DirectoryInfo outputDirectoryInfo, params string[] folderNames)
        {
			Array.ForEach (folderNames, (folderName) => 
				{
					string folderPath = Path.Combine (outputDirectoryInfo.Parent.FullName, folderName);

					Directory.CreateDirectory (folderPath);

					string sourceFolderPath = Path.Combine (sourceDirectoryInfo.Parent.FullName, folderName);

					DirectoryInfo sourceFolderInfo = new DirectoryInfo (sourceFolderPath);

					FileInfo[] subFileInfos = sourceFolderInfo.GetFiles ();

					Array.ForEach(subFileInfos, subFileInfo=>
						{
							string fileName = subFileInfo.Name;

							string sourceFilePath = Path.Combine (sourceFolderPath, fileName);

							string createFilePath = Path.Combine (folderPath, fileName);

							File.Copy (sourceFilePath, createFilePath);
						});
				});
        }
    }
}