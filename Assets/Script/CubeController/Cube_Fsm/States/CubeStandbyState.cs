using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;

namespace Kun.Controller
{
	public class CubeStandbyState : CubeFlowState 
	{
		public CubeStandbyState (CubeController cubeController, CubeFlowController cubeFlowController) : base (cubeController, cubeFlowController)
		{

		}

		public override void Enter (CubeFlowState prevState)
		{
			base.Enter (prevState);
		}

		public override CubeFlowState Stay (float deltaTime)
		{
			Vector3 mousePos;

			if (cubeFlowData.RowRatateCacheData != null) 
			{
				return GetState<CubeGameRowRotateState> ();
			}

			if (inputReceiver.ScreenTrigger(out mousePos))
			{
				RaycastHit hit;

				if (cubeEntityController.Raycast (mousePos, out hit))
				{
					cubeFlowData.HitCache = hit;
					cubeFlowData.MousePosCache = mousePos;

					return GetState<CubeRowRotateStandbyState> ();
				}
				else
				{
					return GetState<CubeWholeRotateState> ();
				}
			}

			return null;
		}
	}
}
