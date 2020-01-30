using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class CubeBindDataRow 
	{
		public CubeBindDataRow(int rowIndex, IEnumerable<Transform> cubeEntitys)
		{
			this.rowIndex = rowIndex;
			this.cubeEntitys = new List<Transform> (cubeEntitys);
		}
		
		public CubeBindDataRow()
		{
			
		}
		
		public int RowIndex
		{
			get
			{
				return rowIndex;
			}
		}

		[SerializeField]
		int rowIndex;

		public List<Transform> CubeEntitys
		{
			get
			{
				return cubeEntitys;
			}
		}

		[SerializeField]
		List<Transform> cubeEntitys;
	}
}
