using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class CubeRowBindData 
	{
		public List<Transform> CubeEntitys
		{
			get
			{
				return cubeEntitys;
			}
		}

		[SerializeField]
		List<Transform> cubeEntitys;
	}
}
