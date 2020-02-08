using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class RotPairSurface
	{
		public int Index
		{
			get
			{
				return index;
			}
		}

		[SerializeField]
		int index;

		public Quaternion BindRot
		{
			get
			{
				return Quaternion.Euler (bindEuler);
			}
		}

		public Vector3 Forward
		{
			get
			{
				return (BindRot * Vector3.forward).normalized;
			}
		}

		[SerializeField]
		Vector3 bindEuler;

		public AxisPair HorizontalSetting
		{
			get
			{
				return horizontalSetting;
			}
		}

		[SerializeField]
		AxisPair horizontalSetting;

		[SerializeField]
		AxisPair verticalSetting;
	}
}