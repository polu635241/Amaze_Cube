using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Controller
{
	[Serializable]
	public class CubeFlowController
	{
		public CubeFlowController (CubeController cube_Controller)
		{	
			cubeFlowRepository = new CubeFlowRepository (cube_Controller, this);
			ForceChangeState<CubeStandbyState> ();
		}

		CubeFlowState currentState;
		CubeFlowRepository cubeFlowRepository;

		[SerializeField][ReadOnly]
		string currentStateInfo;

		public void Stay(float deltaTime)
		{
			CubeFlowState nextState = currentState.Stay (deltaTime);

			if (nextState != null) 
			{
				CubeFlowState prevState = currentState;
				
				currentState.Exit ();
				currentState = nextState;
				currentState.Enter (prevState);

				RefreshCurrentStateInfo ();
			}
		}

		public T GetState<T> () where T:CubeFlowState
		{
			return cubeFlowRepository.GetState<T> () as T;
		}

		public void ForceChangeState<T> () where T:CubeFlowState
		{
			CubeFlowState prevState = currentState;
			
			if (currentState != null) 
			{
				currentState.Exit ();
			}
			
			CubeFlowState nextState = GetState<T> ();
			nextState.Enter (prevState);
			currentState = nextState;

			RefreshCurrentStateInfo ();
		}

		void RefreshCurrentStateInfo ()
		{
			currentStateInfo = currentState.GetType ().Name;
		}
	}
}