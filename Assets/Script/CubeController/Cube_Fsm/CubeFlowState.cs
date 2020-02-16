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
		protected GameController gameController;

		/// <summary>
		/// 這個物件初始化在建構式之後 所以透過屬性回傳才能抓到正確的物件
		/// </summary>
		/// <value>The game flow data.</value>
		protected GameFlowController GameFlowController
		{
			get
			{
				return gameController.GameFlowController;
			}
		}
		protected CubeController cubeController;
		protected CubeEntitySetting cubeEntitySetting;
		protected CubeEntityController cubeEntityController;
		protected CubeFlowController cubeFlowController;
		protected InputReceiver inputReceiver;
		protected CubeFlowData cubeFlowData;
		protected Transform mainCamreaTransform;

		/// <summary>
		/// 這個物件會被多次初始化 所以透過屬性回傳才能抓到正確的物件
		/// </summary>
		/// <value>The game flow data.</value>
		protected GameFlowData GameFlowData
		{
			get
			{
				return GameFlowController.GameFlowData;
			}
		}

		public CubeFlowState (CubeController cubeController, CubeFlowController cubeFlowController)
		{
			this.cubeController = cubeController;
			this.cubeFlowController = cubeFlowController;
			this.inputReceiver = cubeController.InputReceiver;
			this.cubeEntitySetting = cubeController.CubeSetting.CubeEntitySetting;

			this.cubeFlowData = cubeFlowController.CubeFlowData;
			this.cubeEntityController = cubeController.CubeEntityController;
			this.mainCamreaTransform = cubeEntityController.MainCameraTransform;
			this.gameController = cubeController.GameController;
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
