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
		const int RowRatateLerpCount = 10;

		public override void Enter (GameFlowState prevState)
		{
			base.Enter (prevState);
			flowUIManager.OnReceiveStatusSwitch (GameFlowUIStatus.History);
			histoyDisplayUIController = flowUIManager.GetRootController<HistoyDisplayUIController> ();
			histoyDisplayUIController.RegeistSpeedChangeEvent (OnPlaySpeedChange);
			histoyDisplayUIController.SetDefaultSpeed ();
			histoyDisplayUIController.SetProgress (0f);

			PlayHistoryGroup playHistoryGroup = gameController.ParseManager.PlayHistoryGroups [0];

			//複製 因為會有移除的操作 避免動到本體
			playHistoryProcessDatas = GetPlayHistoryProcessDatas (playHistoryGroup.PlayHistorys);
			totalTime = playHistoryGroup.TotalTime;
		}

		List<PlayHistoryProcessData> playHistoryProcessDatas;
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
			float currentTime = gameFlowData.FlowTime;

			//可能因為延遲等等原因 一禎數旋轉多次
			List<PlayHistoryProcessData> clonePlayHistoryProcessDatas = new List<PlayHistoryProcessData> (playHistoryProcessDatas);

			//遞迴跟迭代發生在同一個集合時 會發生指標偏差
			clonePlayHistoryProcessDatas.ForEach ((playHistoryProcessData) =>
			{
				if (currentTime > playHistoryProcessData.Time)
				{
					playHistoryProcessDatas.Remove (playHistoryProcessData);

					//如果是行旋轉那就等轉完再繼續
					if (playHistoryProcessData.PlayHistoryStyle == PlayHistoryStyle.RowRotate)
					{
						ProcessRowRotateData (playHistoryProcessData.RowRotateHistoryProcessData);
					}
					else
					{
						ProcessWholeRotateData (playHistoryProcessData.WholeRotateHistory, deltaTime);
					}
				}
			});
		}

		void ProcessWholeRotateData (WholeRotateHistory wholeRotateHistory, float deltaTime)
		{
			Vector3 deltaEuler = wholeRotateHistory.GetEuler ();
			cubeEntityController.RotateWhole (deltaEuler, deltaTime);
		}

		void ProcessRowRotateData (RowRotateHistoryProcessData rowRotateProcessData)
		{
			int cubeRowIndex = rowRotateProcessData.RowIndex;
			RowRotateAxis axis = rowRotateProcessData.RowRotateAxis;
			CubeRowData cubeRowData = cubeEntityController.GetCubeRowData (axis, cubeRowIndex);
			cubeRowData.CubeCacheDatas.ForEach (cubeCacheData=> 
			{
				cubeCacheData.DeltaRowRot (rowRotateProcessData.PartRowRotate);
			});

			if (rowRotateProcessData.IsFinish)
			{
				cubeEntityController.OnRowRotateFinish (cubeRowData, rowRotateProcessData.IsPositive);
			}
		}

		List<PlayHistoryProcessData> GetPlayHistoryProcessDatas (List<PlayHistory> playHistorys)
		{
			List<PlayHistoryProcessData> playHistoryProcessDatas = new List<PlayHistoryProcessData> ();

			playHistorys.ForEach (playHistory=> 
			{
				float time = playHistory.Time;

				if (playHistory.PlayHistoryStyle == PlayHistoryStyle.WholeRotate)
				{
					PlayHistoryProcessData processData = PlayHistoryProcessData.GetWholeRotateData (time, playHistory.WholeRotateHistory);
					playHistoryProcessDatas.Add (processData);
				}
				else
				{
					List<PlayHistoryProcessData> processDatas = GetRowRotateHistoryDatas (playHistory);
					playHistoryProcessDatas.AddRange (processDatas);
				}
			});

			return playHistoryProcessDatas;
		}

		List<PlayHistoryProcessData> GetRowRotateHistoryDatas (PlayHistory playHistory)
		{
			List<PlayHistoryProcessData> playHistoryProcessDatas = new List<PlayHistoryProcessData> ();

			float rowRotateTime = parseManager.CubeSetting.CubeEntitySetting.RowRotateTime;
			float partRowRotateTime = rowRotateTime / RowRatateLerpCount;
			float beginTime = playHistory.Time;

			RowRotateHistory rowRotateHistory = playHistory.RowRotateHistory;

			RowRotateAxis rowRotateAxis = rowRotateHistory.RowRotateAxis;
			bool isPositive = rowRotateHistory.IsPositive;

			Quaternion rowRotate = CubeUnility.GetDeltaQuaternion (rowRotateAxis, isPositive);

			Quaternion partRowRotate = Quaternion.Slerp (Quaternion.identity, rowRotate, ((float)1 / RowRatateLerpCount));

			for (int i = 1 ; i <= RowRatateLerpCount ; i++)
			{
				//最後一frame
				bool isFinish = (i == RowRatateLerpCount);
				float processBeginTime = partRowRotateTime * i + beginTime;
				PlayHistoryProcessData rowRotateHistoryProcessData = PlayHistoryProcessData.GetRowRotateData (processBeginTime, rowRotateHistory, partRowRotate, isFinish);
				playHistoryProcessDatas.Add (rowRotateHistoryProcessData);
			}

			
			return playHistoryProcessDatas;
		}


		public override void Exit ()
		{
			base.Exit ();

			histoyDisplayUIController.RemoveSpeedChangeEvent (OnPlaySpeedChange);
		}
	}
}