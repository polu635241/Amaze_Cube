using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class AxisDesciption 
	{
		public AxisDesciption (RowRotateAxis axis, Quaternion bindRot, Vector3 hitNormal)
		{
			this.axis = axis;

			this.bindDirVector3 = GetBindDirVector3 (axis, bindRot);

			dotValue = Vector3.Dot (bindDirVector3, hitNormal);
		}
		
		public RowRotateAxis Axis
		{
			get
			{
				return axis;
			}
		}
		
		[SerializeField][ReadOnly]
		RowRotateAxis axis;

		//對應的Up Right Forward
		public Vector3 BindDirVector3
		{
			get
			{
				return bindDirVector3;
			}
		}

		[SerializeField][ReadOnly]
		Vector3 bindDirVector3;

		public float DotValue
		{
			get
			{
				return dotValue;
			}
		}

		[SerializeField][ReadOnly]
		float dotValue;

		public void ReDot (Vector3 newVelocity)
		{
			dotValue = Vector3.Dot (bindDirVector3, newVelocity.normalized);
		}

		Vector3 GetBindDirVector3 (RowRotateAxis axis, Quaternion bindRot)
		{
			switch (axis) 
			{
			case RowRotateAxis.X:
				{
					return bindRot * Vector3.right;
				}

			case RowRotateAxis.Y:
				{
					return bindRot * Vector3.up;
				}

			case RowRotateAxis.Z:
				{
					return bindRot * Vector3.forward;
				}

			default:
				{
					throw new Exception ("BindDirVector3 mapping fail");
				}
			}
		}
	}
}
