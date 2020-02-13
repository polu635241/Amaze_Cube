using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kun.Controller
{
	public class GameFlowRepository
	{
		public GameFlowRepository (GameController gameController, GameFlowController gameFlowController)
		{
			InitTable (gameController, gameFlowController);
		}

		Dictionary<Type,GameFlowState> playerFlowStateDictTable;

		public GameFlowState GetState<T> () where T:GameFlowState
		{
			GameFlowState state = null;

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

		void InitTable (GameController gameController, GameFlowController gameFlowController)
		{
			playerFlowStateDictTable = new Dictionary<Type, GameFlowState> ();
		}
	}
}