﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
    [Serializable]
    public class PlayHistoryProcessData
    {
		PlayHistoryProcessData ()
		{
			
		}

		public PlayHistoryStyle PlayHistoryStyle
		{
            get 
            {
                return playHistoryStyle;
            }
        }

		PlayHistoryStyle playHistoryStyle;

		public float Time
		{
			get
			{
				return time;
			}
		}

		[SerializeField][ReadOnly]
		float time;

		public WholeRotateHistoryProcessData WholeRotateHistoryProcessData
		{
			get
			{
				return wholeRotateHistoryProcessData;
			}
		}

		[SerializeField][ReadOnly]
		WholeRotateHistoryProcessData wholeRotateHistoryProcessData;

		public RowRotateHistoryProcessData RowRotateHistoryProcessData
		{
			get
			{
				return rowRotateHistoryProcessData;
			}
		}

		[SerializeField][ReadOnly]
		RowRotateHistoryProcessData rowRotateHistoryProcessData;


		public static PlayHistoryProcessData GetWholeRotateData (float time, WholeRotateHistory wholeRotateHistory)
		{
			PlayHistoryProcessData playHistoryProcessData = new PlayHistoryProcessData ();
			playHistoryProcessData.playHistoryStyle = PlayHistoryStyle.WholeRotate;
			playHistoryProcessData.time = time;
			//避免改到原始參考
			playHistoryProcessData.wholeRotateHistoryProcessData = new WholeRotateHistoryProcessData (wholeRotateHistory);
			return playHistoryProcessData;
		}

		public static PlayHistoryProcessData GetRowRotateData (float time, RowRotateHistory rowRotateHistory, Quaternion partRowRotate, bool isFinish)
		{
			PlayHistoryProcessData playHistoryProcessData = new PlayHistoryProcessData ();
			playHistoryProcessData.playHistoryStyle = PlayHistoryStyle.RowRotate;

			playHistoryProcessData.time = time;
			playHistoryProcessData.rowRotateHistoryProcessData = new RowRotateHistoryProcessData (rowRotateHistory, partRowRotate, isFinish);
			return playHistoryProcessData;
		}
	}

	public class RowRotateHistoryProcessData
	{
		public RowRotateHistoryProcessData (RowRotateHistory rowRotateHistory, Quaternion partRowRotate, bool isFinish)
		{
			this.rowRotateAxis = rowRotateHistory.RowRotateAxis;
			this.partRowRotate = partRowRotate;
			this.rowIndex = rowRotateHistory.RowIndex;
			this.isPositive = rowRotateHistory.IsPositive;
			this.isFinish = isFinish;
		}

		public int RowIndex
		{
			get
			{
				return rowIndex;
			}
		}

		[SerializeField]
		[ReadOnly]
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

		public Quaternion PartRowRotate
		{
			get
			{
				return partRowRotate;
			}
		}

		[SerializeField][ReadOnly]
		Quaternion partRowRotate;

		public bool IsPositive 
		{
			get 
			{
				return isPositive;
			}
		}

		[SerializeField][ReadOnly]
		bool isPositive;


		public bool IsFinish
		{
			get 
			{
				return isFinish;
			}
		}

		[SerializeField][ReadOnly]
		bool isFinish;
	}

	public class WholeRotateHistoryProcessData
	{
		public WholeRotateHistoryProcessData (WholeRotateHistory wholeRotateHistory)
		{
			this.deltaEuler = wholeRotateHistory.GetEuler ();
			this.deltaTime = wholeRotateHistory.DeltaTime;
		}

		public Vector3 DeltaEuler
		{
			get 
			{
				return deltaEuler;
			}
		}

		Vector3 deltaEuler;

		public float DeltaTime
		{
			get 
			{
				return deltaTime;
			}
		}

		float deltaTime;
	}
}