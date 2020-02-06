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
		public CubeRowData(List<CubeCacheData> cubeEntityDatas)
		{
			this.cubeCacheDatas = new List<CubeCacheData> (cubeEntityDatas);
		}

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