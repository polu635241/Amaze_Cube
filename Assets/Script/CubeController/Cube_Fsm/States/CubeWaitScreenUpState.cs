using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;

namespace Kun.Controller
{
	public class CubeWaitScreenUpState : CubeFlowState 
	{
		public CubeWaitScreenUpState (CubeController cubeController, CubeFlowController cubeFlowController) : base (cubeController, cubeFlowController)
		{
			
		}
		
		public override CubeFlowState Stay (float deltaTime)
		{
			Vector3 triggerPoint;
			
			if (!inputReceiver.ScreenTrigger (out triggerPoint))
			{
				return GetState<CubeStandbyState> ();
			}
			
			return null;
		}
	}
}