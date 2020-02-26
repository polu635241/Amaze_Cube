using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public class GameHistoryState : GameFlowState
	{
		public GameHistoryState (GameController gameController, GameFlowController gameFlowController) : base (gameController, gameFlowController)
		{

		}

		HistoyDisplayUIController histoyDisplayUIController;
		PosTweenController progressController;

		const int RowRatateLerpCount = 10;

		public override void Enter (GameFlowState prevState)
		{
			base.Enter (prevState);
			flowUIManager.OnReceiveStatusSwitch (GameFlowUIStatus.History);
			BindController ();
			RefreshVarible ();
		}

		List<PlayHistoryProcessData> playHistoryProcessDatas;
		float totalTime;

		float playSpeed = 1f;

		bool inDrag = false;

		float progress;
		float gameTime;
		float prevFrameGameTime;
		int processIndex = -1;
		Vector3 screenPos;

		public override GameFlowState Stay (float deltaTime)
		{
			prevFrameGameTime = gameTime;

			//不觸發底層的時間流控制 這個狀態較為特殊 時間流自己管
			if (!inDrag)
			{
				ProcessTimeUpdate (deltaTime);

				CheckClickBar ();
			}
			else
			{
				UpdateValueBar ();
			}

			ProcessCubeRows ();

			histoyDisplayUIController.SetValue (progress);
			flowUIManager.SetTime (gameTime);
			gameFlowData.FlowTime = gameTime;
			return null;

		}

		void CheckClickBar ()
		{
			if (inputReceiver.ScreenTrigger (out screenPos))
			{
				Vector3 rectPos;

				if (progressController.CheckContatin (screenPos, out rectPos))
				{
					progressController.SetFixPos (rectPos);
					inDrag = true;
				}
			}
		}

		void UpdateValueBar ()
		{
			if (inputReceiver.ScreenTrigger (out screenPos))
			{
				progressController.AttachPoint (screenPos, out progress);

				gameTime = Mathf.Lerp (0, totalTime, progress);
			}
			else
			{
				inDrag = false;
			}
		}

		void ProcessTimeUpdate (float deltaTime)
		{
			float processDeltaTime = deltaTime * playSpeed;

			gameTime = gameFlowData.FlowTime + processDeltaTime;

			if (gameTime > totalTime)
			{
				gameTime = totalTime;
			}

			progress = gameTime / totalTime;

			if (progress > 1)
			{
				progress = 1;
			}
		}

		void ProcessCubeRows ()
		{
			if (prevFrameGameTime == gameTime)
			{
				return;
			}

			bool plus = gameTime > prevFrameGameTime;


			while (true)
			{
				bool chechProcessResult = false;

				if (plus)
				{
					chechProcessResult = CheckProcessNext ();
				}

				if (!chechProcessResult)
				{
					break;
				}
			}
		}

		bool CheckProcessNext ()
		{
			//最後了 沒得做了
			if (processIndex == playHistoryProcessDatas.Count - 1)
			{
				return false;
			}

			PlayHistoryProcessData nextProcessData = playHistoryProcessDatas[processIndex + 1];

			if (gameTime >= nextProcessData.Time)
			{
				ProcessCubeRow (nextProcessData);
				processIndex++;
				return true;
			}
			else
			{
				return false;
			}
		}

		void ProcessCubeRow (PlayHistoryProcessData playHistoryProcessData)
		{
			if (playHistoryProcessData.PlayHistoryStyle == PlayHistoryStyle.RowRotate)
			{
				ProcessRowRotateData (playHistoryProcessData.RowRotateHistoryProcessData);
			}
			else
			{
				ProcessWholeRotateData (playHistoryProcessData.WholeRotateHistoryProcessData);
			}
		}

		void ProcessWholeRotateData (WholeRotateHistoryProcessData wholeRotateHistoryProcessData)
		{
			Vector3 deltaEuler = wholeRotateHistoryProcessData.DeltaEuler;
			float deltaTime = wholeRotateHistoryProcessData.DeltaTime;
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

		void BindController ()
		{
			histoyDisplayUIController = flowUIManager.GetRootController<HistoyDisplayUIController> ();
			histoyDisplayUIController.RegeistSpeedChangeEvent (OnPlaySpeedChange);
			histoyDisplayUIController.Refresh ();
			progressController = histoyDisplayUIController.ProgressController;
		}

		void RefreshVarible ()
		{
			PlayHistoryGroup playHistoryGroup = gameController.ParseManager.PlayHistoryGroups[0];

			//複製 因為會有移除的操作 避免動到本體
			playHistoryProcessDatas = GetPlayHistoryProcessDatas (playHistoryGroup.PlayHistorys);
			totalTime = playHistoryGroup.TotalTime;

			inDrag = false;
			processIndex = -1;
		}

		void OnPlaySpeedChange (float newSpeed)
		{
			playSpeed = newSpeed;
		}

		public override void Exit ()
		{
			base.Exit ();

			histoyDisplayUIController.RemoveSpeedChangeEvent (OnPlaySpeedChange);
		}
	}
}