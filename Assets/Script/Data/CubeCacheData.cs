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

			wholeRot = Quaternion.identity;

			worldRot = Quaternion.identity;

			rowRot = Quaternion.identity;

			this.originRelativelyPos = centerPoint.InverseTransformPoint (bindTransform.position);
		}

		CubeCacheData()
		{
			
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

		Quaternion wholeRot;

		public Quaternion RowRot
		{
			get
			{
				return rowRot;
			}
		}

		Quaternion rowRot;

		Quaternion worldRot;

		public void Reset ()
		{
			this.wholeRot = Quaternion.identity;

			this.rowRot = Quaternion.identity;

			Flush ();
		}

		public void SetWholeRot (Quaternion wholeRot)
		{
			this.wholeRot = wholeRot;

			Flush ();
		}

		public void SetSingleRot(Quaternion rowRot)
		{
			this.rowRot = rowRot;

			Flush ();
		}

		public void DeltaSingleRot(Quaternion deltaRot)
		{
			rowRot = deltaRot * rowRot;

			Flush ();
		}

		void Flush ()
		{	
			worldRot = wholeRot * rowRot;

			//原始的相對座標當作旋轉矩 往新的方向轉
			Vector3 newPos = centerPoint.position + worldRot * originRelativelyPos;

			bindTransform.position = newPos;
			bindTransform.rotation = worldRot;
		}
	}
}