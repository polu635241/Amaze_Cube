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
		public CubeRowData(List<CubeCacheData> cubeEntityDatas, CubeCacheData rowCenterPoint)
		{
			this.cubeCacheDatas = new List<CubeCacheData> (cubeEntityDatas);
			this.rowCenterPoint = rowCenterPoint;
		}

		public CubeRowData ()
		{
			
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

		/// <summary>
		/// 中心點 參與旋轉 不參與換位
		/// </summary>
		/// <value>The row center point.</value>
		public CubeCacheData RowCenterPoint
		{
			get
			{
				return rowCenterPoint;
			}

			set
			{
				rowCenterPoint = value;
			}
		}

		CubeCacheData rowCenterPoint;

		public void SetUp (CubeRowData other)
		{
			this.cubeCacheDatas = new List<CubeCacheData> (other.cubeCacheDatas);

			this.rowCenterPoint = other.rowCenterPoint;
		}

		public bool CheckDataExist(Collider coll)
		{
			return cubeCacheDatas.Exists (cubeCacheData=>
				{
					return cubeCacheData.RecieveColl == coll;
				});
		}

        public bool CheckDataExist (CubeCacheData _cubeCacheData)
        {
            return cubeCacheDatas.Exists(cubeCacheData =>
            {
                return cubeCacheData == _cubeCacheData;
            });
        }
    }
}