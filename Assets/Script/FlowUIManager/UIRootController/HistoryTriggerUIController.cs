using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Data;

namespace Kun.Tool
{
	public class HistoryTriggerUIController : UIRootController
	{
		public HistoryTriggerUIController (GameObject bindGO) : base (bindGO)
		{

		}

		public override void SwitchStatus (GameFlowUIStatus status)
		{
			base.SwitchStatus (status);

			//開始玩 才能重置
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

			case GameFlowUIStatus.History:
				{
					bindGO.SetActive (false);
					break;
				}
			}
		}
	}
}