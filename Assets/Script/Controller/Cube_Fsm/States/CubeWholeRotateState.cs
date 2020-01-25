using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;

namespace Kun.Controller
{
	public class CubeWholeRotateState : CubeFlowState {

		public CubeWholeRotateState (CubeController cube_Controller, CubeFlowController cubeFlowController) : base (cube_Controller, cubeFlowController)
		{
			
		}

		public override void Enter (CubeFlowState prevState)
		{
			base.Enter (prevState);
		}

		public override CubeFlowState Stay (float deltaTime)
		{
			return null;
		}
	}
}
