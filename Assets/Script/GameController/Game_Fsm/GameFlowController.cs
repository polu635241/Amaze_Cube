using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public class GameFlowController 
	{
		public GameFlowController (GameController gameController)
		{
			this.gameController = gameController;

			gameFlowRepository = new GameFlowRepository (gameController, this);

			ForceChangeState<GameStandbyState> ();
		}
		
		public GameController GameController
		{
			get
			{
				return gameController;
			}
		}
		
		GameController gameController;

		GameFlowState currentState;

		[SerializeField][ReadOnly]
		string currentStateInfo;

		GameFlowRepository gameFlowRepository;

		public void Stay (float deltaTime)
		{
			GameFlowState nextState = currentState.Stay (deltaTime);

			if (nextState != null) 
			{
				GameFlowState prevState = currentState;

				currentState.Exit ();
				currentState = nextState;
				currentState.Enter (prevState);

				RefreshCurrentStateInfo ();
			}
		}

		public T GetState<T> () where T:GameFlowState
		{
			return gameFlowRepository.GetState<T> () as T;
		}

		public void ForceChangeState<T> () where T:GameFlowState
		{
			GameFlowState prevState = currentState;

			if (currentState != null) 
			{
				currentState.Exit ();
			}

			GameFlowState nextState = GetState<T> ();
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
