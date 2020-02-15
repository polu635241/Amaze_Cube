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

		public override void Enter (GameFlowState prevState)
		{
			base.Enter (prevState);
		}

		public override GameFlowState Stay (float deltaTime)
		{
			base.Stay (deltaTime);
			
			gameController.CubeController.Stay (deltaTime);

			gameController.FlowUIController.SetTime (gameFlowData.FlowTime);

			return null;
		}
	}
}