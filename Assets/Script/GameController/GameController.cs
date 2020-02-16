using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;
using Kun.Data;
using Kun.HardwareInput;

namespace Kun.Controller
{
	public class GameController : MonoBehaviour 
	{
		public CubeController CubeController
		{
			get
			{
				return cubeController;
			}
		}

		[SerializeField][ReadOnly]
		CubeController cubeController;

		public GameFlowController GameFlowController
		{
			get
			{
				return gameFlowController;
			}
		}

		[SerializeField][ReadOnly]
		GameFlowController gameFlowController;

		public FlowUIController FlowUIController
		{
			get
			{
				return flowUIController;
			}
		}

		FlowUIController flowUIController;

		public ParseManager ParseManager
		{
			get
			{
				return parseManager;
			}
		}

		[SerializeField][ReadOnly]
		ParseManager parseManager;

		KeyboardMouseInputReceiver keyboardMouseInputReceiver;

		public PlyerHistoryGroupFlusher PlyerHistoryGroupFlusher 
		{
			get;
			private set;
		}

		// Use this for initialization
		void Awake () 
		{
			keyboardMouseInputReceiver = new KeyboardMouseInputReceiver ();
			
			parseManager = new ParseManager ();
			parseManager.ParseSettings ();

			PlyerHistoryGroupFlusher = new PlyerHistoryGroupFlusher (parseManager.FlushPlayerHistoryGroup);
			PlyerHistoryGroupFlusher.Run ();

			GameObject sceneRefBinderGo = GameObject.FindWithTag (Tags.SceneRefBinder);

			RefBinder sceneRefBinder = sceneRefBinderGo.GetComponent<RefBinder> ();

			GameObject cubeGo = sceneRefBinder.GetGameobject (AssetKeys.Cube);
			cubeController = cubeGo.AddComponent<CubeController> ();
			cubeController.Init (sceneRefBinder, parseManager.CubeSetting, keyboardMouseInputReceiver, this);

			RefBinder uIRootRefBinder = sceneRefBinder.GetComponent<RefBinder> (AssetKeys.UIRoot);
			flowUIController = new FlowUIController ();
			flowUIController.SetUp (uIRootRefBinder, OnGameFlowUIClick, OnApplicationQuitClick);

			gameFlowController = new GameFlowController (this);
		}
		
		// Update is called once per frame
		void Update () 
		{
			float deltaTime = Time.deltaTime;

			gameFlowController.Stay (deltaTime);
		}

		public void OnGameFlowUIClick (GameFlowUIStatus enterStatus)
		{
			switch (enterStatus) 
			{
			case GameFlowUIStatus.GameStart:
				{
					OnGameStart ();
					break;
				}

			case GameFlowUIStatus.Reset:
				{
					OnGameReset ();
					break;
				}
			}
		}

		void OnGameStart()
		{
			gameFlowController.ForceChangeState<GamePlayState> ();
		}

		void OnGameReset()
		{
			gameFlowController.ForceChangeState<GameStandbyState> ();
			cubeController.Reset ();
			flowUIController.Reset ();
		}

		void OnApplicationQuitClick()
		{
			Application.Quit ();
		}
	}
}