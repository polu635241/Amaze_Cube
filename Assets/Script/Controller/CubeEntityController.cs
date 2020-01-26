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

			cubeEntityDatas = new List<CubeEntityData> ();
			subCubes.ForEach (subCube=>
				{
					cubeEntityDatas.Add (new CubeEntityData (centerPoint, subCube));
				});

			this.cubeEntitySetting = cubeEntitySetting;
		}
		
		[SerializeField]
		Transform centerPoint;

		[SerializeField][ReadOnly]
		List<CubeEntityData> cubeEntityDatas;

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

			currentWholeRot = deltaRot * currentWholeRot;
			currentWholeEuler = currentWholeRot.eulerAngles;

			cubeEntityDatas.ForEach (cubeCacheData=>
				{
					cubeCacheData.SetWholeRot (currentWholeRot);
				});
		}
	}
}
