using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Kun.Tool
{
	public class PlyerHistoryGroupUnility : EditorWindow
	{

		[MenuItem ("Tool/PlyerHistoryGroup/Clear")]
		static void ClearPlyerHistoryGroup ()
		{
			string historyFullPath = ParseManager.HistoryFullPath;

			//本來就沒有存檔 所以直接跳走就好
			if (!File.Exists (historyFullPath))
			{
				return;
			}

			using (StreamWriter streamWriter = new StreamWriter (ParseManager.HistoryFullPath, false))
			{
				streamWriter.Write ("");
			}
		}
	}
}