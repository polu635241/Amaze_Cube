﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	public static class CubeBindDataExtension
	{
		public static CubeRowData GetEntityRow (this CubeRowBindData cubeBindDataRow, Dictionary<Transform,CubeCacheData> cubeCacheDataMappings)
		{
			List<CubeCacheData> cubeEntityDatas = new List<CubeCacheData> ();

			cubeBindDataRow.CubeEntitys.ForEach (cubeEntity=>
				{
					CubeCacheData cubeCacheData;

					if(cubeCacheDataMappings.TryGetValue(cubeEntity, out cubeCacheData))
					{
						cubeEntityDatas.Add(cubeCacheData);
					}
					else
					{
						Debug.LogError($"找不到對應的緩存檔 name -> {cubeEntity.name}");
					}
				});
			
			CubeCacheData rowCenterPoint;

			Transform rowCenterPointEntity = cubeBindDataRow.RowCenterPoint;

			if (!cubeCacheDataMappings.TryGetValue (rowCenterPointEntity, out rowCenterPoint))
			{
				Debug.LogError($"找不到對應的中心點緩存檔 name -> {rowCenterPointEntity.name}");
			}

			CubeRowData cubeEntityDataRow = new CubeRowData (cubeEntityDatas, rowCenterPoint);
			
			return cubeEntityDataRow;
			
		}
	}
}
