using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;

namespace Kun.Controller
{
	public class CubeWholeRotateState : CubeFlowState {

		Vector3? mouseLastPos = null;

		public CubeWholeRotateState (CubeController cube_Controller, CubeFlowController cubeFlowController) : base (cube_Controller, cubeFlowController)
		{
			
		}

		public override void Enter (CubeFlowState prevState)
		{
			base.Enter (prevState);

			mouseLastPos = null;
		}

		public override CubeFlowState Stay (float deltaTime)
		{
			Vector3 mousePos;
			
			if (inputReceiver.ScreenTrigger(out mousePos))
			{
				//開始滑的第一frame無視掉 之後每一frame都比較跟上一個frame的位移取標準化
				if (mouseLastPos != null)
				{
					Vector3 deltaPos = (mousePos - mouseLastPos.Value);

					Vector3 deltaEnler = new Vector3 (deltaPos.y, deltaPos.x * -1);

					cube_Controller.CubeEntityController.RotateWhole (deltaEnler, deltaTime);
				}

				mouseLastPos = mousePos;
			}
			else
			{
				//回到待命狀態
				return GetState<CubeStandbyState> ();
			}

			return null;
		}
	}
}
