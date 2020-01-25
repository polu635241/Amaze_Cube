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
	}
}
