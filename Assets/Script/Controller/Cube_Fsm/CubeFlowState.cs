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
		protected CubeController cubeController;
		protected CubeEntitySetting cubeEntitySetting;
		protected SurfaceSetting surfaceSetting;
		protected CubeEntityController cubeEntityController;
		protected CubeFlowController cubeFlowController;
		protected InputReceiver inputReceiver;
		protected CubeFlowData cubeFlowData;
		protected Transform mainCamreaTransform;

		public CubeFlowState (CubeController cubeController, CubeFlowController cubeFlowController)
		{
			this.cubeController = cubeController;
			this.cubeFlowController = cubeFlowController;
			this.inputReceiver = cubeController.InputReceiver;
			ParseManager parseManager = cubeController.ParseManager;
			CubeSetting cubeSetting = parseManager.CubeSetting;
			this.surfaceSetting = parseManager.SurfaceSetting;
			this.cubeEntitySetting = cubeSetting.CubeEntitySetting;

			this.cubeFlowData = cubeFlowController.CubeFlowData;
			this.cubeEntityController = cubeController.CubeEntityController;
			this.mainCamreaTransform = cubeEntityController.MainCameraTransform;
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
