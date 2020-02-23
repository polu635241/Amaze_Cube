using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public abstract class GameFlowState
	{
		protected GameController gameController;
		protected GameFlowController gameFlowController;
		protected GameFlowData gameFlowData;
		protected ParseManager parseManager;
		protected FlowUIManager flowUIManager;

		protected CubeController cubeController;
		protected CubeEntityController cubeEntityController;
		protected CubeFlowController cubeFlowController;

		public GameFlowState (GameController gameController, GameFlowController gameFlowController)
		{
			this.gameController = gameController;
			this.gameFlowController = gameFlowController;
			this.gameFlowData = gameFlowController.GameFlowData;
			this.flowUIManager = gameController.FlowUIController;
			this.parseManager = gameController.ParseManager;

			this.cubeController = gameController.CubeController;
			this.cubeEntityController = cubeController.CubeEntityController;
			this.cubeFlowController = cubeController.CubeFlowController;
		}

		public virtual void Enter(GameFlowState prevState)
		{
			gameFlowData.FlowTime = 0f;
		}

		public virtual GameFlowState Stay (float deltaTime)
		{
			gameFlowData.FlowTime += deltaTime;
			return null;
		}

		public virtual void Exit()
		{
			//			#if UNITY_EDITOR
			//			Debug.LogError ($" exit -> {this.GetType ()}");
			//			#endif
		}

		protected GameFlowState GetState<T> () where T : GameFlowState
		{
			return gameFlowController.GetState<T> ();
		}
	}
}
