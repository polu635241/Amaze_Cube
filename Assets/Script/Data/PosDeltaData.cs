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
		public PosDeltaData (bool isHorizontal, bool isPositive)
		{
			this.isHorizontal = isHorizontal;
			this.isPositive = isPositive;
		}

		public bool IsHorizontal
		{
			get
			{
				return isHorizontal;
			}
		}

		[SerializeField][ReadOnly]
		bool isHorizontal;

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