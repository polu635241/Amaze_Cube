using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class GameFlowData
	{
		public float FlowTime
		{
			get
			{
				return flowTime;
			}

			set
			{
				flowTime = value;
			}
		}
		
		[SerializeField][ReadOnly]
		float flowTime;

		/// <summary>
		/// 因為反序列化的時候 不能判斷抽象型別 所以使用變數緩存再透過內部的enum判斷回傳
		/// </summary>
		/// <param name="rowRotateAxis">Row rotate axis.</param>
		/// <param name="isPositive">If set to <c>true</c> is positive.</param>
		public void AddPlayRowRotateHistory (int rowIndex, RowRotateAxis rowRotateAxis, bool isPositive)
		{
			this.playHistoryGroup.AddPlayRowRotateHistory (flowTime, rowIndex, rowRotateAxis, isPositive);
		}

		/// <summary>
		/// 因為反序列化的時候 不能判斷抽象型別 所以使用變數緩存再透過內部的enum判斷回傳
		/// </summary>
		/// <param name="deltaPos">Delta position.</param>
		public void AddPlayWholeRotateHistory (Vector2 deltaPos)
		{
			this.playHistoryGroup.AddPlayWholeRotateHistory (flowTime, deltaPos);
		}

		public PlayHistoryGroup PlayHistoryGroup
		{
			get
			{
				return playHistoryGroup;
			}

			set
			{
				playHistoryGroup = value;
			}
		}

		PlayHistoryGroup playHistoryGroup;
	}
}