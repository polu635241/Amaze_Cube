using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;
using Kun.HardwareInput;

namespace Kun.Data
{
	[Serializable]
	public class CubeCacheData
	{
		public CubeCacheData (Transform centerPoint, Transform bindTransform)
		{
			this.bindTransform = bindTransform;

			this.originRelativelyPos = centerPoint.InverseTransformPoint (bindTransform.position);
		}

		[SerializeField][ReadOnly]
		Transform bindTransform;

		public Transform BindTransform
		{
			get
			{
				return bindTransform;
			}
		}

		[SerializeField]
		Vector3 originRelativelyPos;

		public Vector3 OriginRelativelyPos
		{
			get
			{
				return originRelativelyPos;
			}
		}
	}
}