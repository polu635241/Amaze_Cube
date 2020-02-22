using System;
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

		HistoyDisplayUIController histoyDisplayUIController;

		public override void Enter (GameFlowState prevState)
		{
			base.Enter (prevState);
			flowUIManager.OnReceiveStatusSwitch (GameFlowUIStatus.History);
			histoyDisplayUIController = flowUIManager.GetRootController<HistoyDisplayUIController> ();
			histoyDisplayUIController.RegeistSpeedChangeEvent (OnPlaySpeedChange);
			histoyDisplayUIController.SetDefaultSpeed ();
			histoyDisplayUIController.SetProgress (0f);

			gameFlowData.InRowRotate = false;

			PlayHistoryGroup playHistoryGroup = gameController.ParseManager.PlayHistoryGroups [0];

			//複製 因為會有移除的操作 避免動到本體
			playHistorys = new List<PlayHistory> (playHistoryGroup.PlayHistorys);
			totalTime = playHistoryGroup.TotalTime;
		}

		List<PlayHistory> playHistorys;
		float totalTime;

		float playSpeed = 1f;

		void OnPlaySpeedChange (float newSpeed)
		{
			playSpeed = newSpeed;
		}

		public override GameFlowState Stay (float deltaTime)
		{
			float processDeltaTime = deltaTime * playSpeed;

			//時間是在底層處裡的 所以要先處理完deltaTime再傳給底層
			base.Stay (processDeltaTime);

			ProcessCubeRow (processDeltaTime);

			float gameTime = gameFlowData.FlowTime;

			if (gameTime <= totalTime)
			{
				float progress = gameTime / totalTime;

				if (progress > 1)
				{
					progress = 1;
				}

				histoyDisplayUIController.SetProgress (progress);
				flowUIManager.SetTime (gameTime);
			}

			return null;
		}

		void ProcessCubeRow (float deltaTime)
		{
			gameController.CubeController.CubeFlowController.Stay (deltaTime);

			if (!gameFlowData.InRowRotate)
			{
				float currentTime = gameFlowData.FlowTime;

				//可能因為延遲等等原因 一禎數旋轉多次
				List<PlayHistory> clonePlayHistorys = new List<PlayHistory> (playHistorys);

				//遞迴跟迭代發生在同一個集合時 會發生指標偏差
				clonePlayHistorys.ForEach ((playHistory) =>
				{
					if (currentTime > playHistory.Time)
					{
						playHistorys.Remove (playHistory);

						//如果是行旋轉那就等轉完再繼續
						if (playHistory.PlayHistoryStyle == PlayHistoryStyle.RowRotate)
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

			histoyDisplayUIController.RemoveSpeedChangeEvent (OnPlaySpeedChange);
		}
	}
}