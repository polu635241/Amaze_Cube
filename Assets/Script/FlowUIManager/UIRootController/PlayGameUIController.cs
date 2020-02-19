using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Data;

namespace Kun.Tool
{
	public class PlayGameUIController : UIRootController
	{
		public PlayGameUIController (GameObject bindGO) : base (bindGO)
		{
			
		}


		public override void SwitchStatus (GameFlowUIStatus status)
		{
			base.SwitchStatus (status);

			//待機中 才能玩
			switch (status) 
			{
			case GameFlowUIStatus.GameStart:
				{
					bindGO.SetActive (false);
					break;
				}

			case GameFlowUIStatus.Standby:
				{
					bindGO.SetActive (true);
					break;
				}
			}
		}
	}

}