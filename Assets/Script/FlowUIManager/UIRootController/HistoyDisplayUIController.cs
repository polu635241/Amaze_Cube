using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Kun.Data;

namespace Kun.Tool
{
	public class HistoyDisplayUIController : UIRootController
	{
		public HistoyDisplayUIController (RefBinder refBinder) : base (refBinder.gameObject)
		{
			progressTweenController = refBinder.GetComponent<PosTweenController> (AssetKeys.ProgressValuePosController);
		}

		PosTweenController progressTweenController;

		public void SetProgress (float progress)
		{
			progressTweenController.SetValue (progress);
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