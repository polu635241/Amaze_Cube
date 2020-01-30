using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	public static class CubeBindDataExtension
	{
		public static CubeEntityDataRow GetEntityRow (this CubeBindDataRow cubeBindDataRow, Transform centerPoint)
		{
			List<CubeEntityData> cubeEntityDatas = new List<CubeEntityData> ();

			cubeBindDataRow.CubeEntitys.ForEach (cubeEntity=>
				{
					CubeEntityData cubeEntityData = new CubeEntityData (centerPoint, cubeEntity);
					cubeEntityDatas.Add (cubeEntityData);
				});
			
			CubeEntityDataRow cubeEntityDataRow = new CubeEntityDataRow (cubeBindDataRow.RowIndex, cubeEntityDatas);

			return cubeEntityDataRow;
		}

		public static CubeEntityDataGroup GetEntityGroup (this CubeBindDataGroup cubeBindDataGroup, int groupInfo, Transform centerPoint)
		{
			List<CubeEntityDataRow> horizontalRows = GetEntityRows (cubeBindDataGroup.HorizontalRows, centerPoint);
			List<CubeEntityDataRow> verticalRows = GetEntityRows (cubeBindDataGroup.VerticalRows, centerPoint);

			List<CubeBindDataRow> bindHorizontalRows = cubeBindDataGroup.HorizontalRows;
			List<CubeBindDataRow> bindVerticalRows = cubeBindDataGroup.VerticalRows;

			CubeEntityDataGroup cubeEntityDataGroup = new CubeEntityDataGroup (groupInfo, horizontalRows, verticalRows);

			return cubeEntityDataGroup;
		}


		static List<CubeEntityDataRow> GetEntityRows (List<CubeBindDataRow> bindDataRows, Transform centerPoint)
		{
			List<CubeEntityDataRow> entityRows = new List<CubeEntityDataRow> (); 

			if (bindDataRows != null) 
			{
				bindDataRows.ForEach (row=>
					{
						CubeEntityDataRow cubeEntityDataRow = row.GetEntityRow (centerPoint);

						entityRows.Add(cubeEntityDataRow);
					});
			}

			return entityRows;
		}
	}
}
