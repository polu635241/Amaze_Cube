using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kun.HardwareInput
{
	public class KeyboardMouseInputReceiver:InputReceiver
	{		
		public override bool Jump ()
		{
			return Input.GetKeyDown (KeyCode.Space);
		}

		public override bool Right ()
		{
			return Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow);
		}

		public override bool Left ()
		{
			return Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow);
		}

		public override bool Up ()
		{
			return (Input.GetKey (KeyCode.UpArrow)||Input.GetKey (KeyCode.W));
		}

		public override bool Down ()
		{
			return (Input.GetKey (KeyCode.DownArrow)||Input.GetKey (KeyCode.S));
		}

		public override bool Rush ()
		{
			return Input.GetKey (KeyCode.LeftShift);
		}

		public override bool ScreenTrigger (out Vector3 triggerPoint)
		{
			if (Input.GetMouseButton(0)) 
			{
				triggerPoint = Input.mousePosition;
				return true;
			}
			else
			{
				triggerPoint = Vector3.zero;
				return false;
			}
		}
	}
}