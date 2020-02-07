using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	public class RowRatateCacheData
	{
		public RowRatateCacheData(Quaternion rowDeltaQuaternion, bool isPositive)
		{
			this.rowDeltaQuaternion = rowDeltaQuaternion;
			this.isPositive = isPositive;
		}
		
		public Quaternion RowDeltaQuaternion
		{
			get
			{
				return rowDeltaQuaternion;
			}
		}
		
		Quaternion rowDeltaQuaternion;
		
		public CubeRowData CurrentRowData;
		
		CubeRowData currentRowData;
		
		public bool IsPositive
		{
			get
			{
				return isPositive;
			}
		}
		
		bool isPositive;
	}
}