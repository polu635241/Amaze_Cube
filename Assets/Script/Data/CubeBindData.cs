#define DrawDebugLine
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	public class CubeBindData : MonoBehaviour 
	{
		public Transform CenterPoint
		{
			get
			{
				return centerPoint;
			}
		}
		
		[SerializeField]
		Transform centerPoint;

		public List<Transform> CubeEntitys
		{
			get
			{
				return cubeEntitys;
			}
		}

		[SerializeField]
		List<Transform> cubeEntitys = new List<Transform> ();

		public List<CubeRowBindData> X_RotateRows
		{
			get
			{
				return x_RotateRows;
			}
		}

		[SerializeField]
		List<CubeRowBindData> x_RotateRows = new List<CubeRowBindData> ();

		public List<CubeRowBindData> Y_RotateRows
		{
			get
			{
				return y_RotateRows;
			}
		}

		[SerializeField]
		List<CubeRowBindData> y_RotateRows = new List<CubeRowBindData> ();

		public List<CubeRowBindData> Z_RotateRows
		{
			get
			{
				return z_RotateRows;
			}
		}

		[SerializeField]
		List<CubeRowBindData> z_RotateRows = new List<CubeRowBindData> ();
	}
}
