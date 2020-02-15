using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public class GamePlayState : GameFlowState 
	{
		public  GamePlayState (GameController gameController, GameFlowController gameFlowController) : base (gameController, gameFlowController)
		{
			
		}

		float throughTime;

		public override void Enter (GameFlowState prevState)
		{
			base.Enter (prevState);
			throughTime = 0f;
		}

		public override GameFlowState Stay (float deltaTime)
		{
			gameController.CubeController.Stay (deltaTime);

			throughTime += deltaTime;

			gameController.FlowUIController.SetTime (throughTime);

			return null;
		}
	}
}