using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class CubeFlowData
	{
		public RowRatateCacheData RowRatateCacheData
		{
			get
			{
				return rowRatateCacheData;
			}

			set
			{
				rowRatateCacheData = value;
			}
		}
		
		[SerializeField][ReadOnly]
		RowRatateCacheData rowRatateCacheData = null;
	}
}
