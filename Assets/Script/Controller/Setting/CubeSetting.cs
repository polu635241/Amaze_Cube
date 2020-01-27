using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class CubeSetting 
	{
		[SerializeField][Header("方塊實體相關設定")][ReadOnly]
		CubeEntitySetting cubeEntitySetting;

		/// <summary>
		/// 方塊實體相關設定
		/// </summary>
		/// <value>The character setting.</value>
		public CubeEntitySetting CubeEntitySetting
		{
			get
			{
				return cubeEntitySetting;
			}
		}
	}
}
