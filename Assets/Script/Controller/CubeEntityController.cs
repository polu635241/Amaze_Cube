using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;
using Kun.Data;
using Kun.HardwareInput;

namespace Kun.Controller
{
	[Serializable]
	public class CubeEntityController
	{
		public CubeEntityController (Transform centerPoint, List<Transform> subCubes, CubeEntitySetting cubeEntitySetting)
		{
			this.centerPoint = centerPoint;

			currentWholeRot = centerPoint.rotation;
			currentWholeEuler = centerPoint.eulerAngles;

			cubeCacheDatas = new List<CubeCacheData> ();
			subCubes.ForEach (subCube=>
				{
					cubeCacheDatas.Add (new CubeCacheData (centerPoint, subCube));
				});

			this.cubeEntitySetting = cubeEntitySetting;
		}
		
		[SerializeField]
		Transform centerPoint;

		[SerializeField][ReadOnly]
		List<CubeCacheData> cubeCacheDatas;

		[SerializeField][ReadOnly]
		CubeEntitySetting cubeEntitySetting;

		[SerializeField][ReadOnly]
		Quaternion currentWholeRot;

		[SerializeField][ReadOnly]
		Vector3 currentWholeEuler;

		public void RotateWhole (Vector3 deltaEuler, float deltaTime)
		{
			Vector3 processDeltaEuler = deltaEuler * cubeEntitySetting.RotateSpeed * deltaTime;

			Quaternion deltaRot = Quaternion.Euler (processDeltaEuler);

			SetWholeRotCache (deltaRot);
			ProcessCubeCacheDatas ();
		}

		void SetWholeRotCache(Quaternion deltaRot)
		{
			currentWholeRot = deltaRot * currentWholeRot;
			currentWholeEuler = currentWholeRot.eulerAngles;
		}

		void ProcessCubeCacheDatas ()
		{
			cubeCacheDatas.ForEach (cubeCacheData=>
				{
					Transform bindTransform = cubeCacheData.BindTransform;

					//原始的相對座標當作旋轉矩 往新的方向轉
					Vector3 newPos = centerPoint.position + currentWholeRot * cubeCacheData.OriginRelativelyPos;

					bindTransform.rotation = currentWholeRot;
					bindTransform.position = newPos;
				});
		}
	}
}
