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
			this.cubeEntityDatas = new List<CubeCacheData> (cubeEntityDatas);
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

		public List<CubeCacheData> CubeEntityDatas
		{
			get
			{
				return cubeEntityDatas;
			}
		}

		[SerializeField][ReadOnly]
		List<CubeCacheData> cubeEntityDatas;
	}
}