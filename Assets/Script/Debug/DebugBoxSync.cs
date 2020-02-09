using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;
using Kun.Data;
using Kun.HardwareInput;

namespace Kun.Controller
{
	public class DebugBoxSync : GenericEntityController 
	{
		[SerializeField]
		CubeController cubeController;

		void Awake ()
		{
			base.Awake ();
		}

		void Update ()
		{
			if (cubeController != null) 
			{
				CubeEntityController cubeEntityController = cubeController.CubeEntityController;
				
				if (cubeEntityController != null) 
				{
					m_Transform.rotation = cubeEntityController.CurrentWholeRot;
				}	
			}
		}
	}
}
