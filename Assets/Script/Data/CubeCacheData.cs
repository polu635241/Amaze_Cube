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
			this.centerPoint = centerPoint;
			this.bindTransform = bindTransform;
			this.receiveColl = bindTransform.GetComponent<Collider> ();

			wholeRot = centerPoint.rotation;
			wholeEuler = wholeRot.eulerAngles;

			worldRot = wholeRot;
			worldEuler = wholeEuler;

			rowRot = Quaternion.identity;
			rowEuler = rowRot.eulerAngles;

			this.originRelativelyPos = centerPoint.InverseTransformPoint (bindTransform.position);
		}

		[SerializeField][ReadOnly]
		Transform centerPoint;

		[SerializeField][ReadOnly]
		Transform bindTransform;

		[SerializeField][ReadOnly]
		Collider receiveColl;

		public Collider RecieveColl
		{
			get
			{
				return receiveColl;
			}
		}

		[SerializeField][ReadOnly]
		Vector3 originRelativelyPos;

		public void SetWholeRot (Quaternion wholeRot)
		{
			this.wholeRot = wholeRot;
			this.wholeEuler = wholeRot.eulerAngles;

			Flush ();
		}

		public void DeltaSingleRot(Quaternion deltaRot)
		{
			rowRot = deltaRot * rowRot;
			rowEuler = rowRot.eulerAngles;

			Flush ();
		}

		void Flush ()
		{	
			worldRot = wholeRot * rowRot;
			worldEuler = worldRot.eulerAngles;
			
			//原始的相對座標當作旋轉矩 往新的方向轉
			Vector3 newPos = centerPoint.position + worldRot * originRelativelyPos;

			bindTransform.position = newPos;
			bindTransform.rotation = worldRot;
		}

		Quaternion wholeRot;

		[SerializeField][ReadOnly][Header("整個方塊群體的旋轉")]
		Vector3 wholeEuler;

		Quaternion rowRot;

		[SerializeField][ReadOnly][Header("所在行的旋轉")]
		Vector3 rowEuler;

		Quaternion worldRot;

		/// <summary>
		/// "whole * row * single"
		/// </summary>
		[SerializeField][ReadOnly][Header("whole * row")]
		Vector3 worldEuler;
	}
}