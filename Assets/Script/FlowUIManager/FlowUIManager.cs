using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public class FlowUIManager
	{
		public void SetUp(RefBinder uiRootRefBinder, Action<GameFlowUICmd> onGameFlowUIClickCallback, Action onApplicationQuitClickCallback)
		{
			ProcessGameFLowBtn (uiRootRefBinder);

			GameObject timeTextGO = uiRootRefBinder.GetGameobject (AssetKeys.TimeText);
			timeText = timeTextGO.GetComponent<Text> ();

			Button applicationQuitBtn = uiRootRefBinder.GetComponent<Button> (AssetKeys.ApplicationQuitBtn);
			applicationQuitBtn.onClick.AddListener (OnApplicationQuitClick);

			this.onGameFlowUIClickEvent = onGameFlowUIClickCallback;
			this.onApplicationQuitClickEvent = onApplicationQuitClickCallback;
		}

		Text timeText;

		event Action<GameFlowUICmd> onGameFlowUIClickEvent;

		event Action onApplicationQuitClickEvent;

		List<UIRootController> uiRootControllers = new List<UIRootController> ();

		public void SetTime(float time)
		{
			string timeStr = Tool.Tool.TimeTransferMilliSecond (time);
			timeText.text = timeStr;
		}

		void ProcessGameFLowBtn (RefBinder uiRootRefBinder)
		{
			GameObject gameStartBtnGO = uiRootRefBinder.GetGameobject (AssetKeys.GameStartBtn);
			Button gameStartBtn = gameStartBtnGO.GetComponent<Button> ();
			gameStartBtn.onClick.AddListener (()=>
				{
					OnGameFlowUIClick (GameFlowUICmd.GameStart);
				});
			PlayGameUIController playGameUIController = new PlayGameUIController (gameStartBtnGO);
			uiRootControllers.Add (playGameUIController);

			GameObject resetBtnGO = uiRootRefBinder.GetGameobject (AssetKeys.ResetBtn);
			Button resetBtn = resetBtnGO.GetComponent<Button> ();
			resetBtn.onClick.AddListener (()=>
				{
					OnGameFlowUIClick (GameFlowUICmd.Reset);
				});
			ResetUIController resetUIController = new ResetUIController (resetBtnGO);
			uiRootControllers.Add (resetUIController);
		}


		/// <summary>
		/// 收到遊戲核心切換狀態
		/// </summary>
		/// <param name="status">Status.</param>
		public void OnReceiveStatusSwitch (GameFlowUIStatus status)
		{
			uiRootControllers.ForEach (uiRootController=>
				{
					uiRootController.SwitchStatus (status);
				});
		}

		/// <summary>
		/// 透過按鈕接收到點擊切換狀態
		/// </summary>
		/// <param name="status">Status.</param>
		void OnGameFlowUIClick(GameFlowUICmd cmd)
		{
			//Callback觸發當前狀態 
			onGameFlowUIClickEvent.Invoke (cmd);
		}

		public void Reset ()
		{
			SetTime (0f);
		}


		void OnApplicationQuitClick ()
		{
			onApplicationQuitClickEvent.Invoke ();
		}
	}

}