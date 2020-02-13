using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	public class RowRatateCacheData
	{
		public RowRatateCacheData (CubeRowData currentRowData, Quaternion rowDeltaQuaternion, bool isPositive)
		{
			this.rowDeltaQuaternion = rowDeltaQuaternion;
			this.isPositive = isPositive;
            this.currentRowData = currentRowData;
		}

		public CubeRowData CurrentRowData
		{
			get
			{
				return currentRowData;
			}
		}

		CubeRowData currentRowData;

		public Quaternion RowDeltaQuaternion
		{
			get
			{
				return rowDeltaQuaternion;
			}
		}
		
		Quaternion rowDeltaQuaternion;
		
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