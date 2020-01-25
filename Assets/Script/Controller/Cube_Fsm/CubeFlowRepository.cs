using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kun.Controller
{
	public class CubeFlowRepository
	{
		public CubeFlowRepository (CubeController cube_Controller, CubeFlowController cubeFlowController)
		{
			InitTable (cube_Controller, cubeFlowController);
		}
		
		Dictionary<Type,CubeFlowState> playerFlowStateDictTable;

		public CubeFlowState GetState<T> () where T:CubeFlowState
		{
			CubeFlowState state = null;

			Type type = typeof(T);

			if (playerFlowStateDictTable.TryGetValue (type, out state))
			{
				return state as T;
			}
			else
			{
				throw new UnityException (string.Format ("Can't get state -> {0}", type));
			}
		}

		void InitTable (CubeController cube_Controller, CubeFlowController cubeFlowController)
		{
			playerFlowStateDictTable = new Dictionary<Type, CubeFlowState> ();
			playerFlowStateDictTable.Add (typeof(CubeStandbyState), new CubeStandbyState (cube_Controller, cubeFlowController));
			playerFlowStateDictTable.Add (typeof(CubeWholeRotateState), new CubeWholeRotateState (cube_Controller, cubeFlowController));
		}
	}
}