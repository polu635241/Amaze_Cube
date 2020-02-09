using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class PosDeltaData
	{
		public PosDeltaData (int axisIndex, bool isPositive)
		{
			this.axisIndex = axisIndex;
			this.isPositive = isPositive;
		}

		public int AxisIndex
		{
			get
			{
				return axisIndex;
			}
		}

		[SerializeField][ReadOnly]
		int axisIndex;

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
}