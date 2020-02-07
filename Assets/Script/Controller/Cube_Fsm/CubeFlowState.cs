using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public abstract class CubeFlowState
	{
		protected CubeController cube_Controller;
		protected CubeEntitySetting cubeEntitySetting;
		protected CubeFlowController cubeFlowController;
		protected InputReceiver inputReceiver;
		protected CubeFlowData cubeFlowData;

		public CubeFlowState (CubeController cube_Controller, CubeFlowController cubeFlowController)
		{
			this.cube_Controller = cube_Controller;
			this.cubeFlowController = cubeFlowController;
			this.inputReceiver = cube_Controller.InputReceiver;
			CubeSetting cubeSetting = cube_Controller.ParseManager.CubeSetting;
			this.cubeEntitySetting = cubeSetting.CubeEntitySetting;

			this.cubeFlowData = cubeFlowController.CubeFlowData;
		}

		public virtual void Enter(CubeFlowState prevState)
		{
//			#if UNITY_EDITOR
//			Debug.LogError ($" enter -> {this.GetType ()}");
//			#endif
		}

		public virtual CubeFlowState Stay (float deltaTime)
		{
			return null;
		}

		public virtual void Exit()
		{
//			#if UNITY_EDITOR
//			Debug.LogError ($" exit -> {this.GetType ()}");
//			#endif
		}

		protected CubeFlowState GetState<T> () where T:CubeFlowState
		{
			return cubeFlowController.GetState<T> ();
		}
	}
}
