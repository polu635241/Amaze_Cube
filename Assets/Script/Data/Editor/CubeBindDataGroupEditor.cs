using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Kun.Tool;

namespace Kun.Data
{
	[CustomEditor(typeof(CubeGroupBindData))]
	public class CubeBindDataGroupEditor : SerializedObjectEditor<CubeGroupBindData> 
	{
		int rowCount = 3;

		int rowItemCount = 3;

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			GUILayout.Space (10);

			EditorTool.DrawInVertical (()=>
				{
					rowCount = EditorGUILayout.IntField("rowCount", rowCount);

					rowItemCount = EditorGUILayout.IntField("rowItemCount", rowItemCount);

					if (GUILayout.Button ("Reset Row", BiggerFontSizeBtnGUIStyle, GUILayout.Height (buttonHeight)))
					{
						ResetRow ();
					}
				}, boxSkin);
		}

		void ResetRow ()
		{
			Transform[] row_Source = new Transform[rowItemCount];

			List<CubeRowBindData> horizontalRows = new List<CubeRowBindData> ();
			List<CubeRowBindData> verticalRows = new List<CubeRowBindData> ();

			for (int i = 0; i < rowCount; i++) 
			{
				CubeRowBindData horizontalRow = new CubeRowBindData (i, row_Source);

				horizontalRows.Add (horizontalRow);

				CubeRowBindData verticalRow = new CubeRowBindData (i, row_Source);

				verticalRows.Add (verticalRow);
			}

			runtimeScript.HorizontalRows = horizontalRows;
			runtimeScript.VerticalRows = verticalRows;
		}
	}
}
