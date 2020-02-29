using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class PlayHistoryGroup
	{
		public PlayHistoryGroup (DateTime startTime)
		{
			this.startTime = startTime.ToString ("yyyy-MM-dd");
			this.playHistorys = new List<PlayHistory> ();
		}
		
		public string StartTime
		{
			get
			{
				return startTime;
			}
		}
		
		[SerializeField]
		string startTime;

		public float TotalTime
		{
			get
			{
				return totalTime;
			}

			set
			{
				totalTime = value;
			}
		}

		[SerializeField]
		float totalTime;

		public List<PlayHistory> PlayHistorys
		{
			get
			{
				return playHistorys;
			}
		}

		[SerializeField][ReadOnly]
		List<PlayHistory> playHistorys = new List<PlayHistory> ();

		/// <summary>
		/// 因為反序列化的時候 不能判斷抽象型別 所以使用變數緩存再透過內部的enum判斷回傳
		/// </summary>
		/// <param name="time">Time.</param>
		/// <param name="rowRotateAxis">Row rotate axis.</param>
		/// <param name="isPositive">If set to <c>true</c> is positive.</param>
		public void AddPlayRowRotateHistory (float time, int rowIndex, RowRotateAxis rowRotateAxis, bool isPositive)
		{
			PlayHistory rowRotateHistory = PlayHistory.GetRowRotateHistory (time, rowIndex, rowRotateAxis, isPositive);
			this.playHistorys.Add (rowRotateHistory);
		}

		/// <summary>
		/// 因為反序列化的時候 不能判斷抽象型別 所以使用變數緩存再透過內部的enum判斷回傳
		/// </summary>
		/// <param name="time">Time.</param>
		/// <param name="deltaPos">Delta position.</param>
		public void AddPlayWholeRotateHistory (float time, Vector2 deltaPos)
		{
			PlayHistory wholeRotateHistory = PlayHistory.GetWholeRotateHistory (time, deltaPos);
			this.playHistorys.Add (wholeRotateHistory);
		}
	}
}