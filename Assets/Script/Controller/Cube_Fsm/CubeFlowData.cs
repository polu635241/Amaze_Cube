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

		public RaycastHit HitCache
		{
			get
			{
				return hitCache;
			}

			set
			{
				hitCache = value;
			}
		}

		[SerializeField][ReadOnly]
		RaycastHit hitCache;

		public Vector3 MousePosCache
		{
			get
			{
				return mousePosCache;
			}

			set
			{
				mousePosCache = value;
			}
		}

		[SerializeField][ReadOnly]
		Vector3 mousePosCache;
	}
}
