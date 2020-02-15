using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Data;

namespace Kun.Tool
{
	public static class FlowUIExtension 
	{
		public static GameFlowUIStatus NextStatus (this GameFlowUIStatus status)
		{
			switch (status) 
			{
			case GameFlowUIStatus.GameStart:
				{
					return GameFlowUIStatus.Reset;
				}

			case GameFlowUIStatus.Reset:
				{
					return GameFlowUIStatus.GameStart;
				}

			default:
				{
					throw new UnityException ("Mapping fail");
				}
			}
		}
		
	}
}
