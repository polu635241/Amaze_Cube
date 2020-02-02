using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	public static class CubeBindDataExtension
	{
		public static CubeRowData GetEntityRow (this CubeRowBindData cubeBindDataRow, Transform centerPoint)
		{
			List<CubeCacheData> cubeEntityDatas = new List<CubeCacheData> ();

			cubeBindDataRow.CubeEntitys.ForEach (cubeEntity=>
				{
					CubeCacheData cubeEntityData = new CubeCacheData (centerPoint, cubeEntity);
					cubeEntityDatas.Add (cubeEntityData);
				});
			
			CubeRowData cubeEntityDataRow = new CubeRowData (cubeBindDataRow.RowIndex, cubeEntityDatas);

			return cubeEntityDataRow;
		}

		public static CubeGroupData GetEntityGroup (this CubeGroupBindData cubeBindDataGroup, int groupInfo, Transform centerPoint)
		{
			List<CubeRowData> horizontalRows = GetEntityRows (cubeBindDataGroup.HorizontalRows, centerPoint);
			List<CubeRowData> verticalRows = GetEntityRows (cubeBindDataGroup.VerticalRows, centerPoint);

			List<CubeRowBindData> bindHorizontalRows = cubeBindDataGroup.HorizontalRows;
			List<CubeRowBindData> bindVerticalRows = cubeBindDataGroup.VerticalRows;

			CubeGroupData cubeEntityDataGroup = new CubeGroupData (groupInfo, horizontalRows, verticalRows);

			return cubeEntityDataGroup;
		}


		static List<CubeRowData> GetEntityRows (List<CubeRowBindData> bindDataRows, Transform centerPoint)
		{
			List<CubeRowData> entityRows = new List<CubeRowData> (); 

			if (bindDataRows != null) 
			{
				bindDataRows.ForEach (row=>
					{
						CubeRowData cubeEntityDataRow = row.GetEntityRow (centerPoint);

						entityRows.Add(cubeEntityDataRow);
					});
			}

			return entityRows;
		}
	}
}
