using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class CubeEntitySetting 
	{
		[SerializeField][Header("旋轉速度")][ReadOnly]
		float rotateSpeed;

		/// <summary>
		/// 旋轉速度
		/// </summary>
		/// <value>The rotate speed.</value>
		public float RotateSpeed
		{
			get
			{
				return rotateSpeed;
			}

			set
			{
				rotateSpeed = value;
			}
		}

		[SerializeField][Header("行轉動所需時間")][ReadOnly]
		float rowRotateTime;

		/// <summary>
		/// 行轉動所需時間
		/// </summary>
		/// <value>The row rotate time.</value>
		public float RowRotateTime
		{
			get
			{
				return rowRotateTime;
			}
		}

		[SerializeField][Header("行旋轉 所需的觸碰點移動長度")][ReadOnly]
		float rowRotateNeedLength;

		public float RowRotateNeedLength
		{
			get
			{
				return rowRotateNeedLength;
			}
		}
	}
}
