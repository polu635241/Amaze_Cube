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

			worldRot = wholeRot;

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

		//Vector3,Quaternion 和 Transform不能序列化 所以不能靠序列化成bitArray來達成DeepClone
		public CubeCacheData GetDeepClone ()
		{
			CubeCacheData cloneData = new CubeCacheData ();
			cloneData.centerPoint = this.centerPoint;
			cloneData.bindTransform = this.bindTransform;
			cloneData.receiveColl = this.receiveColl;
			cloneData.originRelativelyPos = this.originRelativelyPos;
			cloneData.wholeRot = this.wholeRot;
			cloneData.rowRot = this.rowRot;
			return cloneData;
		}

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