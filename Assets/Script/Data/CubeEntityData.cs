using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;
using Kun.HardwareInput;

namespace Kun.Data
{
	[Serializable]
	public class CubeEntityData
	{
		public CubeEntityData (Transform centerPoint, Transform bindTransform)
		{
			this.centerPoint = centerPoint;

			groupRot = centerPoint.rotation;
			groupEuler = groupRot.eulerAngles;

			singleRot = Quaternion.identity;
			singleEuler = singleRot.eulerAngles;

			worldRot = groupRot;
			worldEuler = groupEuler;

			this.bindTransform = bindTransform;

			this.originRelativelyPos = centerPoint.InverseTransformPoint (bindTransform.position);
		}

		[SerializeField][ReadOnly]
		Transform centerPoint;

		[SerializeField][ReadOnly]
		Transform bindTransform;

		[SerializeField][ReadOnly]
		Vector3 originRelativelyPos;

		public void SetWholeRot (Quaternion wholeRot)
		{
			this.groupRot = wholeRot;
			this.groupEuler = groupRot.eulerAngles;

			Flush ();
		}

		public void DeltaSingleRot(Quaternion deltaRot)
		{
			singleRot = deltaRot * singleRot;
			singleEuler = singleRot.eulerAngles;

			Flush ();
		}

		void Flush ()
		{
			worldRot = groupRot * singleRot;
			worldEuler = worldRot.eulerAngles;
			
			//原始的相對座標當作旋轉矩 往新的方向轉
			Vector3 newPos = centerPoint.position + worldRot * originRelativelyPos;

			bindTransform.position = newPos;
			bindTransform.rotation = worldRot;
		}

		Quaternion groupRot;

		[SerializeField][ReadOnly]
		Vector3 groupEuler;

		Quaternion singleRot;

		[SerializeField][ReadOnly]
		Vector3 singleEuler;


		Quaternion worldRot;

		/// <summary>
		/// "world * single"
		/// </summary>
		[SerializeField][ReadOnly][Header("world * single")]
		Vector3 worldEuler;
	}
}