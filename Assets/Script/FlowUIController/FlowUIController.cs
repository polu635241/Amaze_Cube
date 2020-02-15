using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public class FlowUIController
	{
		public void SetUp(RefBinder uiRootRefBinder, Action<GameFlowUIStatus> onGameFlowUIClickCallback, Action onApplicationQuitClickCallback)
		{
			ProcessGameFLowBtn (uiRootRefBinder, AssetKeys.GameStartBtn, GameFlowUIStatus.GameStart);
			ProcessGameFLowBtn (uiRootRefBinder, AssetKeys.ResetBtn, GameFlowUIStatus.Reset);

			GameObject timeTextGO = uiRootRefBinder.GetGameobject (AssetKeys.TimeText);
			timeText = timeTextGO.GetComponent<Text> ();

			Button applicationQuitBtn = uiRootRefBinder.GetComponent<Button> (AssetKeys.ApplicationQuitBtn);
			applicationQuitBtn.onClick.AddListener (OnApplicationQuitClick);

			this.onGameFlowUIClickEvent = onGameFlowUIClickCallback;
			this.onApplicationQuitClickEvent = onApplicationQuitClickCallback;
		}

		Text timeText;

		GameFlowUIStatus currentBtnStatus;

		event Action<GameFlowUIStatus> onGameFlowUIClickEvent;

		event Action onApplicationQuitClickEvent;

		Dictionary<GameFlowUIStatus,GameObject> statusPairEntitys = new Dictionary<GameFlowUIStatus, GameObject>();

		public void SetTime(float time)
		{
			string timeStr = Tool.Tool.TimeTransferMilliSecond (time);
			timeText.text = timeStr;
		}

		void ProcessGameFLowBtn (RefBinder uiRootRefBinder, string assetKey, GameFlowUIStatus bindStatus)
		{
			GameObject gameFlowBtnGO = uiRootRefBinder.GetGameobject (assetKey);
			Button gameFlowBtn = gameFlowBtnGO.GetComponent<Button> ();
			gameFlowBtn.onClick.AddListener (OnGameFlowUIClick);

			statusPairEntitys.Add (bindStatus, gameFlowBtnGO);
		}

		public void Reset ()
		{
			SetTime (0f);
		}

		void OnGameFlowUIClick()
		{
			//Callback觸發當前狀態 
			onGameFlowUIClickEvent.Invoke (currentBtnStatus);

			//並關閉按鈕實體
			GameObject originBtnGO = statusPairEntitys [currentBtnStatus];
			originBtnGO.SetActive (false);

			//切換為新的狀態
			currentBtnStatus = currentBtnStatus.NextStatus ();

			//並開啟對應的按鈕實體
			GameObject currentBtnGO = statusPairEntitys [currentBtnStatus];
			currentBtnGO.SetActive (true);
		}

		void OnApplicationQuitClick ()
		{
			onApplicationQuitClickEvent.Invoke ();
		}
	}

}