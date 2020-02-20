using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Data;

namespace Kun.Tool
{
	public class HistoyDisplayUIController : UIRootController
	{
		public HistoyDisplayUIController (GameObject bindGO) : base (bindGO)
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
					bindGO.SetActive (false);
					break;
				}

            case GameFlowUIStatus.History:
                {
                    bindGO.SetActive (true);
                    break;
                }
			}
		}
	}
}