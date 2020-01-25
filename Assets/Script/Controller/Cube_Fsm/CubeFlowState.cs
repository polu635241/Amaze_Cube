using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;

namespace Kun.Controller
{
	public abstract class CubeFlowState
	{
		protected CubeController cube_Controller;
		protected CubeFlowController cubeFlowController;
		protected InputReceiver inputReceiver;

		public CubeFlowState (CubeController cube_Controller, CubeFlowController cubeFlowController)
		{
			this.cube_Controller = cube_Controller;
			this.cubeFlowController = cubeFlowController;
			this.inputReceiver = cube_Controller.InputReceiver;
		}

		public virtual void Enter(CubeFlowState prevState)
		{
			
		}

		public virtual CubeFlowState Stay (float deltaTime)
		{
			return null;
		}

		public virtual void Exit()
		{

		}

		protected CubeFlowState GetState<T> () where T:CubeFlowState
		{
			return cubeFlowController.GetState<T> ();
		}
	}
}
