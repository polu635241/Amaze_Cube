using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class CubeEntityDataGroup
	{
		public CubeEntityDataGroup (int info, List<CubeEntityDataRow> horizontalRows, List<CubeEntityDataRow> verticalRows)
		{
			this.info = info;
			this.horizontalRows = new List<CubeEntityDataRow> (horizontalRows);
			this.verticalRows = new List<CubeEntityDataRow> (verticalRows);
		}
		
		[SerializeField][ReadOnly][Header("對應骰子的1~6對應的面數")]
		int info;

		public List<CubeEntityDataRow> HorizontalRows
		{
			get
			{
				return horizontalRows;
			}
		}

		List<CubeEntityDataRow> horizontalRows;

		public List<CubeEntityDataRow> VerticalRows
		{
			get
			{
				return verticalRows;
			}
		}

		List<CubeEntityDataRow> verticalRows;

		[NonSerialized]
		CubeEntityDataGroup rightGroupData;

		[NonSerialized]
		CubeEntityDataGroup leftGroupData;

		[NonSerialized]
		CubeEntityDataGroup upGroupData;

		[NonSerialized]
		CubeEntityDataGroup downGroupData;
	}
}
