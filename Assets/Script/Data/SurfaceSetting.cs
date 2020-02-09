using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[CreateAssetMenu(menuName="Setting/SurfaceSetting")]
	public class SurfaceSetting : ScriptableObject 
	{
		public List<RotPairSurface> rotPairSurfaces = new List<RotPairSurface> ();
	}
}
