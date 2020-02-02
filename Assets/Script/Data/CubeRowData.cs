using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class CubeRowData
	{
		public CubeRowData(int rowIndex, List<CubeCacheData> cubeEntityDatas)
		{
			this.rowIndex = rowIndex;
			this.cubeCacheDatas = new List<CubeCacheData> (cubeEntityDatas);
		}
		
		public int RowIndex
		{
			get
			{
				return rowIndex;
			}
		}

		[SerializeField][ReadOnly]
		int rowIndex;

		public List<CubeCacheData> CubeCacheDatas
		{
			get
			{
				return cubeCacheDatas;
			}
		}

		[SerializeField][ReadOnly]
		List<CubeCacheData> cubeCacheDatas;

		public bool CheckDataExist(Collider coll)
		{
			return cubeCacheDatas.Exists (cubeCacheData=>
				{
					return cubeCacheData.RecieveColl == coll;
				});
		}
	}
}