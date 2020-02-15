﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public class CubeRowRotateState : CubeFlowState 
	{
		float rowRotateTime;

		public CubeRowRotateState (CubeController cubeController, CubeFlowController cubeFlowController) : base (cubeController, cubeFlowController)
		{
			rowRotateTime = cubeEntitySetting.RowRotateTime;
		}

		public override void Enter (CubeFlowState prevState)
		{
			base.Enter (prevState);

			rowRatateCacheData = cubeFlowData.RowRatateCacheData;
			cubeFlowData.RowRatateCacheData = null;

			CubeRowData cubeRowData = rowRatateCacheData.CurrentRowData;

			Quaternion rowDeltaQuaternion = rowRatateCacheData.RowDeltaQuaternion;

			originRots = new List<Quaternion> ();
			targetRots = new List<Quaternion> ();

			cubeRowData.CubeCacheDatas.ForEach (cubeCacheData=>
				{
					Quaternion originRot = cubeCacheData.RowRot;
					Quaternion targetRot = rowDeltaQuaternion * originRot;

					originRots.Add (originRot);
					targetRots.Add (targetRot);
				});

			centerPointOriginRot = cubeRowData.RowCenterPoint.RowRot;
			centetPointTargetRot = rowDeltaQuaternion * centerPointOriginRot;

			throuthTime = 0f;
		}

		List<Quaternion> originRots;
		Quaternion centerPointOriginRot;

		List<Quaternion> targetRots;
		Quaternion centetPointTargetRot;

		float throuthTime;

		RowRatateCacheData rowRatateCacheData;

		public override CubeFlowState Stay (float deltaTime)
		{
			CubeRowData cubeRowData = rowRatateCacheData.CurrentRowData;

			Quaternion rowDeltaQuaternion = rowRatateCacheData.RowDeltaQuaternion;

			bool isPositive = rowRatateCacheData.IsPositive;

			cubeRowData.CubeCacheDatas.ForEach (cubeCacheData=>
				{
					cubeCacheData.DeltaSingleRot (rowDeltaQuaternion);
				});

			throuthTime += deltaTime;

			if (throuthTime < rowRotateTime) 
			{
				float progress = throuthTime / rowRotateTime;
				ProcessRowRotateProgress (progress);

				return null;
			}
			else
			{
				ProcessRowRotateProgress (1);
				cubeEntityController.OnRowRotateFinish (cubeRowData, isPositive);
				cubeFlowData.ClearCache ();
				return GetState<CubeStandbyState> ();
			}

		}

		void ProcessRowRotateProgress (float progress)
		{
			CubeRowData cubeRowData = rowRatateCacheData.CurrentRowData;

			cubeRowData.CubeCacheDatas.Map ((index, cubeCacheData)=>
				{	
					Quaternion originRot = originRots[index];
					Quaternion targetRot = targetRots[index];

					Quaternion currentRot = Quaternion.Lerp (originRot, targetRot, progress);

					cubeCacheData.SetSingleRot (currentRot);
				});

			Quaternion centerPointCurrentRot = Quaternion.Lerp (centerPointOriginRot, centetPointTargetRot, progress);

			cubeRowData.RowCenterPoint.SetSingleRot (centerPointCurrentRot);
		}
	}
}
