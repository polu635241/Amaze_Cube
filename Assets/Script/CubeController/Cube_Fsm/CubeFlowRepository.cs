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
		
		Dictionary<Type,CubeFlowState> cubeFlowStateDictTable;

		public CubeFlowState GetState<T> () where T:CubeFlowState
		{
			CubeFlowState state = null;

			Type type = typeof(T);

			if (cubeFlowStateDictTable.TryGetValue (type, out state))
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
			cubeFlowStateDictTable = new Dictionary<Type, CubeFlowState> ();
			cubeFlowStateDictTable.Add (typeof(CubeStandbyState), new CubeStandbyState (cube_Controller, cubeFlowController));
			cubeFlowStateDictTable.Add (typeof(CubeWholeRotateState), new CubeWholeRotateState (cube_Controller, cubeFlowController));
			cubeFlowStateDictTable.Add (typeof(CubeRowRotateStandbyState), new CubeRowRotateStandbyState (cube_Controller, cubeFlowController));
			cubeFlowStateDictTable.Add (typeof(CubeGameRowRotateState), new CubeGameRowRotateState (cube_Controller, cubeFlowController));
			cubeFlowStateDictTable.Add (typeof(CubeWaitScreenUpState), new CubeWaitScreenUpState (cube_Controller, cubeFlowController));
			cubeFlowStateDictTable.Add (typeof(CubeHistoryStandbyState), new CubeHistoryStandbyState (cube_Controller, cubeFlowController));
			cubeFlowStateDictTable.Add (typeof(CubeHistortyRowRotateState), new CubeHistortyRowRotateState (cube_Controller, cubeFlowController));
		}
	}
}