using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kun.HardwareInput
{
	/// <summary>
	/// 方便後續抽換輸入模組
	/// </summary>
	public abstract class InputReceiver
	{
		public abstract bool Jump ();

		public abstract bool Right ();

		public abstract bool Left ();

		public abstract bool Up ();

		public abstract bool Down();

		public abstract bool Rush ();

		public abstract bool TriggerDown (out Vector3 triggerPoint);
	}
}
