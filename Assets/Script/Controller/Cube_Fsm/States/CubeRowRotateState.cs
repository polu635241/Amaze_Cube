using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public class CubeRowRotateState : CubeFlowState 
	{
		Vector3? mouseLastPos = null;

		public CubeRowRotateState (CubeController cubeController, CubeFlowController cubeFlowController) : base (cubeController, cubeFlowController)
		{
			
		}

		public override void Enter (CubeFlowState prevState)
		{
			base.Enter (prevState);

			mouseLastPos = null;

			rowRatateCacheData = cubeFlowData.RowRatateCacheData;
			cubeFlowData.RowRatateCacheData = null;
		}

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

			cubeEntityController.OnRowRotateFinish (cubeRowData, isPositive);

			return GetState<CubeStandbyState> ();
		}
	}
}
