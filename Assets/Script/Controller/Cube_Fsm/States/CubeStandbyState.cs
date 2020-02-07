using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;

namespace Kun.Controller
{
	public class CubeStandbyState : CubeFlowState 
	{
		public CubeStandbyState (CubeController cubeController, CubeFlowController cubeFlowController) : base (cubeController, cubeFlowController)
		{

		}

		public override void Enter (CubeFlowState prevState)
		{
			base.Enter (prevState);
		}

		public override CubeFlowState Stay (float deltaTime)
		{
			Vector3 mousePos;

			if (inputReceiver.ScreenTrigger(out mousePos))
			{
				RaycastHit hit;

				Ray ray = cubeFlowController.MainCamera.ScreenPointToRay (mousePos);

				string hitName;

				Vector3 beginPos = ray.origin;
				float lineLength = 10f;
				Vector3 endPos = beginPos + ray.direction * lineLength;

				Debug.DrawLine(beginPos, endPos, Color.red, 0.1f);

				if (Physics.Raycast(ray, out hit))
				{
					//TODO Internal Single Row Rotate
				}
				else
				{
					return GetState<CubeWholeRotateState> ();
				}
			}

			return null;
		}
	}
}
