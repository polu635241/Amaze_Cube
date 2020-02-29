using System;
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

		public PlayHistoryProcessData GetReverseData ()
		{
			PlayHistoryProcessData reverseData = new PlayHistoryProcessData ();
			reverseData.time = this.time;
			reverseData.playHistoryStyle = this.playHistoryStyle;
			reverseData.MappingReverseRowData (this);

			return reverseData;
		}

		void MappingReverseRowData (PlayHistoryProcessData source)
		{
			this.playHistoryStyle = source.playHistoryStyle;

			switch (source.playHistoryStyle) 
			{
			case PlayHistoryStyle.WholeRotate:
				{
					this.wholeRotateHistoryProcessData = source.wholeRotateHistoryProcessData.GetReverseData ();
					break;
				}

			case PlayHistoryStyle.RowRotate:
				{
					this.rowRotateHistoryProcessData = source.rowRotateHistoryProcessData.GetReverseData ();
					break;
				}

			default:
				{
					throw new NotSupportedException ("Mapping fail");
				}
			}
		}

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

		RowRotateHistoryProcessData()
		{
			
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

		public RowRotateHistoryProcessData GetReverseData ()
		{
			RowRotateHistoryProcessData reverseData = new RowRotateHistoryProcessData ();
			reverseData.rowRotateAxis = this.rowRotateAxis;
			reverseData.partRowRotate = Quaternion.Inverse (this.partRowRotate);
			reverseData.rowIndex = this.rowIndex;
			reverseData.isPositive = !this.isPositive;
			reverseData.isFinish = this.isFinish;
			return reverseData;
		}
	}

	public class WholeRotateHistoryProcessData
	{
		public WholeRotateHistoryProcessData (WholeRotateHistory wholeRotateHistory)
		{
			this.deltaRot = wholeRotateHistory.GetRot ();
		}

		WholeRotateHistoryProcessData ()
		{
			
		}

		public Quaternion DeltaRot
		{
			get 
			{
				return deltaRot;
			}
		}

		Quaternion deltaRot;

		public WholeRotateHistoryProcessData GetReverseData ()
		{
			WholeRotateHistoryProcessData reverseData = new WholeRotateHistoryProcessData ();
			reverseData.deltaRot = Quaternion.Inverse (this.deltaRot);
			return reverseData;
		}
	}
}