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
		public CubeEntityController (Transform centerPoint, List<KeyValuePair<int,CubeBindDataGroup>> surfaceRootPairIndexs, CubeEntitySetting cubeEntitySetting)
		{
			this.centerPoint = centerPoint;

			currentWholeRot = centerPoint.rotation;
			currentWholeEuler = centerPoint.eulerAngles;

			InitCubeEntityDatas (centerPoint, surfaceRootPairIndexs);

			this.cubeEntitySetting = cubeEntitySetting;
		}
		
		[SerializeField]
		Transform centerPoint;

		[SerializeField][ReadOnly]
		List<CubeEntityDataGroup> cubeEntityDataGroups = new List<CubeEntityDataGroup> ();

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

			//TODO
			cubeEntityDataGroups.ForEach (cubeEntityDataGroup=>
				{
					cubeEntityDataGroup.SetWholeRot (currentWholeRot);
				});
		}

		void InitCubeEntityDatas (Transform centerPoint, List<KeyValuePair<int,CubeBindDataGroup>> surfaceRootPairIndexs)
		{
			cubeEntityDataGroups = new List<CubeEntityDataGroup> ();

			surfaceRootPairIndexs.ForEach (pair=>
				{
					int groupInfo = pair.Key;
					
					CubeBindDataGroup cubeBindDataGroup = pair.Value;

					CubeEntityDataGroup cubeEntityDataGroup = cubeBindDataGroup.GetEntityGroup (groupInfo, centerPoint);

                    cubeEntityDataGroups.Add (cubeEntityDataGroup);

                });
		}
	}
}
