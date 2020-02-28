using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class PlayHistory
	{
		PlayHistory ()
		{
			
		}
		
		public float Time
		{
			get
			{
				return time;
			}
		}

		[SerializeField][ReadOnly]
		float time;

		public PlayHistoryStyle PlayHistoryStyle
		{
			get
			{
				return playHistoryStyle;
			}
		}

		[SerializeField][ReadOnly]
		PlayHistoryStyle playHistoryStyle;
		
		public WholeRotateHistory WholeRotateHistory
		{
			get
			{
				return wholeRotateHistory;
			}
		}
		
		[SerializeField][ReadOnly]
		WholeRotateHistory wholeRotateHistory;

		public RowRotateHistory RowRotateHistory
		{
			get
			{
				return rowRotateHistory;
			}
		}

		[SerializeField][ReadOnly]
		RowRotateHistory rowRotateHistory;

		/// <summary>
		/// 因為反序列化的時候 不能判斷抽象型別 所以使用變數緩存再透過內部的enum判斷回傳
		/// </summary>
		/// <returns>The play row rotate history.</returns>
		/// <param name="time">Time.</param>
		/// <param name="deltaPos">Delta position.</param>
		public static PlayHistory GetRowRotateHistory (float time, int rowIndex, RowRotateAxis rowRotateAxis, bool isPositive)
		{
			PlayHistory playHistory = new PlayHistory ();
			playHistory.time = time;
			playHistory.playHistoryStyle = PlayHistoryStyle.RowRotate;
			playHistory.rowRotateHistory = new RowRotateHistory (rowIndex, rowRotateAxis, isPositive);
			return playHistory;
		}

		/// <summary>
		//  因為反序列化的時候 不能判斷抽象型別 所以使用變數緩存再透過內部的enum判斷回傳
		/// </summary>
		/// <returns>The play whole rotate history.</returns>
		/// <param name="time">Time.</param>
		/// <param name="rowRotateAxis">Row rotate axis.</param>
		/// <param name="isPositive">If set to <c>true</c> is positive.</param>
		public static PlayHistory GetWholeRotateHistory (float time, Vector2 deltaPos, float deltaTime)
		{
			PlayHistory playHistory = new PlayHistory ();
			playHistory.time = time;
			playHistory.playHistoryStyle = PlayHistoryStyle.WholeRotate;
			playHistory.wholeRotateHistory = new WholeRotateHistory (deltaPos, deltaTime);
			return playHistory;
		}
	
	}

	public enum PlayHistoryStyle
	{
		WholeRotate,
		RowRotate
	}

	[Serializable]
	public class RowRotateHistory
	{
		public RowRotateHistory (int rowIndex, RowRotateAxis rowRotateAxis, bool isPositive)
		{
			this.rowRotateAxis = rowRotateAxis;
			this.isPositive = isPositive;
			this.rowIndex = rowIndex;
		}

		public int RowIndex
		{
			get
			{
				return rowIndex;
			}
		}

		[SerializeField][ReadOnly]
		int rowIndex;

		public RowRotateAxis RowRotateAxis
		{
			get
			{
				return rowRotateAxis;
			}
		}

		[SerializeField][ReadOnly]
		RowRotateAxis rowRotateAxis;
		
		public bool IsPositive
		{
			get
			{
				return isPositive;
			}
		}
		
		[SerializeField][ReadOnly]
		bool isPositive;
	}

	[Serializable]
	public class WholeRotateHistory
	{
		public WholeRotateHistory (Vector2 deltaPos, float deltaTime)
		{
			this.deltaPosX = deltaPos.x;
			this.deltaPosY = deltaPos.y;
			this.deltaTime = deltaTime;
		}

		WholeRotateHistory ()
		{
			
		}

		[SerializeField][ReadOnly]
		float deltaPosX;

		[SerializeField][ReadOnly]
		float deltaPosY;

		public float DeltaTime
		{
			get 
			{
				return deltaTime;
			}
		}

		[SerializeField][ReadOnly]
		float deltaTime;

		public Quaternion GetRot ()
		{
			return Tool.Tool.GetPosToRot (deltaPosX, deltaPosY);
		}
	}
}