using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public class GameStandbyState : GameFlowState 
	{
		public GameStandbyState (GameController gameController, GameFlowController gameFlowController) : base (gameController, gameFlowController)
		{
			
		}

		public override void Enter (GameFlowState prevState)
		{
			base.Enter (prevState);
			flowUIManager.OnReceiveStatusSwitch (GameFlowUIStatus.Standby);
		}
	}
}