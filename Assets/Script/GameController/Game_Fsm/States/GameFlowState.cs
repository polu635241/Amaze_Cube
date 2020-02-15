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

		public GameFlowState (GameController gameController, GameFlowController gameFlowController)
		{
			this.gameController = gameController;
			this.gameFlowController = gameFlowController;
		}

		public virtual void Enter(GameFlowState prevState)
		{
			//			#if UNITY_EDITOR
			//			Debug.LogError ($" enter -> {this.GetType ()}");
			//			#endif
		}

		public virtual GameFlowState Stay (float deltaTime)
		{
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
