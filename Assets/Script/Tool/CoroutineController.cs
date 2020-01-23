using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;
using MEC;

namespace Kun.Tool
{
	public class CoroutineController
	{
		/// <summary>
		/// 是否每次run 都把上次沒跑完的清掉
		/// </summary>
		/// <param name="process">Process.</param>
		/// <param name="forceReset">If set to <c>true</c> force reset.</param>
		public CoroutineController(Func<IEnumerator<float>> process, bool forceReset = true)
		{
			this.process = process;
			this.forceReset = forceReset;
		}

		Func<IEnumerator<float>> process;
		CoroutineHandle handle;

		/// <summary>
		/// 是否每次run 都把上次沒跑完的清掉
		/// </summary>
		[SerializeField][ReadOnly]
		bool forceReset;

		public void Run ()
		{
			if (forceReset) 
			{
				Timing.KillCoroutines (handle);
			}

			handle = Timing.RunCoroutine (process ());
		}

		[ContextMenu("Stop")]
		public void Stop ()
		{
			Timing.KillCoroutines (handle);
		}
	}
}
