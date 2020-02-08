﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class AxisPair
	{
		public RowRotateAxis Axis
		{
			get
			{
				return axis;
			}
		}

		[SerializeField]
		RowRotateAxis axis;

		public bool IsPositive
		{
			get
			{
				return isPositive;
			}
		}

		[SerializeField]
		bool isPositive;
	}
}