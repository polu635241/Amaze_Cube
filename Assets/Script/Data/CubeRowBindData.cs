using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class CubeRowBindData 
	{
		public List<Transform> CubeEntitys
		{
			get
			{
				return cubeEntitys;
			}
		}

		[SerializeField]
		List<Transform> cubeEntitys;

		/// <summary>
		/// 中心點 參與旋轉 不參與換位
		/// </summary>
		/// <value>The row center point.</value>
		public Transform RowCenterPoint
		{
			get
			{
				return rowCenterPoint;
			}
		}

		[SerializeField][Header("中心點 參與旋轉 不參與換位")]
		Transform rowCenterPoint;
	}
}
