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
	}
}