using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Kun.Tool;

namespace Kun.Data
{
	[CustomEditor(typeof(CubeBindDataGroup))]
	public class CubeBindDataGroupEditor : SerializedObjectEditor<CubeBindDataGroup> 
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

			List<CubeBindDataRow> horizontalRows = new List<CubeBindDataRow> ();
			List<CubeBindDataRow> verticalRows = new List<CubeBindDataRow> ();

			for (int i = 0; i < rowCount; i++) 
			{
				CubeBindDataRow horizontalRow = new CubeBindDataRow (i, row_Source);

				horizontalRows.Add (horizontalRow);

				CubeBindDataRow verticalRow = new CubeBindDataRow (i, row_Source);

				verticalRows.Add (verticalRow);
			}

			runtimeScript.HorizontalRows = horizontalRows;
			runtimeScript.VerticalRows = verticalRows;
		}
	}
}
