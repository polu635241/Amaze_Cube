using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class GameFlowData
	{
		public float FlowTime
		{
			get
			{
				return flowTime;
			}

			set
			{
				flowTime = value;
			}
		}
		
		[SerializeField][ReadOnly]
		float flowTime;
	}
}