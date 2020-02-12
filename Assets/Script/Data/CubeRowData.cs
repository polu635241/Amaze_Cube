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

		CubeRowData()
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

		public CubeRowData GetDeepClone ()
		{
			CubeRowData cloneRowData = new CubeRowData ();

			cloneRowData.cubeCacheDatas = new List<CubeCacheData> ();

			this.cubeCacheDatas.ForEach ((data)=>
				{
					CubeCacheData cloneData = data.GetDeepClone();
					
					cloneRowData.cubeCacheDatas.Add(cloneData);
				});

			cloneRowData.rowCenterPoint = this.rowCenterPoint.GetDeepClone ();

			return cloneRowData;
		}

		public bool CheckDataExist(Collider coll)
		{
			return cubeCacheDatas.Exists (cubeCacheData=>
				{
					return cubeCacheData.RecieveColl == coll;
				});
		}
	}
}