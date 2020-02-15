using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class CubeRowIndexData
	{
		public CubeRowIndexData (CubeRowData cubeRowData, List<CubeCacheData> allCubeEntityDatas)
		{
			this.cubeCacheDataIndexs = new List<int> ();

			cubeRowData.CubeCacheDatas.ForEach (cubeEntityData=>
				{
					int index = allCubeEntityDatas.IndexOf (cubeEntityData);
					cubeCacheDataIndexs.Add(index);
				});

			int rowCenterPointIndex = allCubeEntityDatas.IndexOf (cubeRowData.RowCenterPoint);
			this.rowCenterPointIndex = rowCenterPointIndex;
		}

		public List<int> CubeCacheDataIndexs
		{
			get
			{
				return cubeCacheDataIndexs;
			}
		}

		[SerializeField][ReadOnly]
		List<int> cubeCacheDataIndexs;

		/// <summary>
		/// 中心點 參與旋轉 不參與換位
		/// </summary>
		/// <value>The row center point.</value>
		public int RowCenterPointIndex
		{
			get
			{
				return rowCenterPointIndex;
			}

			set
			{
				rowCenterPointIndex = value;
			}
		}

		int rowCenterPointIndex;

		public CubeRowData GetValue (List<CubeCacheData> allCubeEntityDatas)
		{
			List<CubeCacheData> cubeCacheDatas = new List<CubeCacheData> ();

			cubeCacheDataIndexs.ForEach (index=>
				{
					CubeCacheData cubeCacheData = allCubeEntityDatas[index];
					cubeCacheDatas.Add (cubeCacheData);
				});

			CubeCacheData rowCenterCubeCacheData = allCubeEntityDatas [rowCenterPointIndex];

			CubeRowData cubeRowData = new CubeRowData (cubeCacheDatas, rowCenterCubeCacheData);

			return cubeRowData;
		}
	}
}