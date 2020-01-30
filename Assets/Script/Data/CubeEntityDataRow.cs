using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class CubeEntityDataRow
	{
		public CubeEntityDataRow(int rowIndex, List<CubeEntityData> cubeEntityDatas)
		{
			this.rowIndex = rowIndex;
			this.cubeEntityDatas = new List<CubeEntityData> (cubeEntityDatas);
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

		public List<CubeEntityData> CubeEntityDatas
		{
			get
			{
				return cubeEntityDatas;
			}
		}

		[SerializeField][ReadOnly]
		List<CubeEntityData> cubeEntityDatas;
	}
}