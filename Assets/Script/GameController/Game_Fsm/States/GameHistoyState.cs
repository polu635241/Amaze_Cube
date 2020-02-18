﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public class GameHistoyState : GameFlowState 
	{
		public  GameHistoyState (GameController gameController, GameFlowController gameFlowController) : base (gameController, gameFlowController)
		{
			
		}

		public override void Enter (GameFlowState prevState)
		{
			base.Enter (prevState);

			gameFlowData.InRowRotate = false;

			PlayHistoryGroup playHistoryGroup = gameController.ParseManager.PlayHistoryGroups [0];

			//複製 因為會有移除的操作 避免動到本體
			playHistorys = new List<PlayHistory> (playHistoryGroup.PlayHistorys);
			totalTime = playHistoryGroup.TotalTime;
		}

		List<PlayHistory> playHistorys;
		float totalTime;

		public override GameFlowState Stay (float deltaTime)
		{
			base.Stay (deltaTime);

			gameController.CubeController.CubeFlowController.Stay (deltaTime);

			if (!gameFlowData.InRowRotate)
			{
				float currentTime = gameFlowData.FlowTime;
				
				//可能因為延遲等等原因 一禎數旋轉多次
				List<PlayHistory> clonePlayHistorys = new List<PlayHistory>(playHistorys);

				//遞迴跟迭代發生在同一個集合時 會發生指標偏差
				clonePlayHistorys.ForEach ((playHistory)=>
					{
						if(currentTime>playHistory.Time)
						{
							playHistorys.Remove (playHistory);

							//如果是行旋轉那就等轉完再繼續
							if(playHistory.PlayHistoryStyle== PlayHistoryStyle.RowRotate)
							{
								ProcessRowRotateData (playHistory.RowRotateHistory);
								return;
							}
							else
							{
								ProcessWholeRotateData (playHistory.WholeRotateHistory, deltaTime);
							}
						}
					});
			}

			return null;
		}


		void ProcessWholeRotateData (WholeRotateHistory wholeRotateHistory, float deltaTime)
		{
			Vector3 deltaEuler = wholeRotateHistory.GetEuler ();
			cubeEntityController.RotateWhole (deltaEuler, deltaTime);
		}

		void ProcessRowRotateData (RowRotateHistory rowRotateHistory)
		{
			int cubeRowIndex = rowRotateHistory.RowIndex;
			RowRotateAxis axis = rowRotateHistory.RowRotateAxis;
			bool isPositive = rowRotateHistory.IsPositive;
			RowRatateCacheData rowRatateCacheData = cubeEntityController.GetRowRatateCacheData (cubeRowIndex, axis, isPositive);
			gameFlowData.HistoryRowRatateCacheData = rowRatateCacheData;
			cubeFlowController.ForceChangeState<CubeHistortyRowRotateState> ();
		}


		public override void Exit ()
		{
			base.Exit ();
		}
	}
}