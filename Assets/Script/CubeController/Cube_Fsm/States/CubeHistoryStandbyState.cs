using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public class CubeHistoryStandbyState : CubeFlowState 
	{

		public CubeHistoryStandbyState (CubeController cubeController, CubeFlowController cubeFlowController) : base (cubeController, cubeFlowController)
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