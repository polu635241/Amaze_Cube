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

		public FlowUIManager FlowUIController
		{
			get
			{
				return flowUIController;
			}
		}

		FlowUIManager flowUIController;

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
			flowUIController = new FlowUIManager ();
			flowUIController.SetUp (uIRootRefBinder, parseManager, OnGameFlowUIClick, OnApplicationQuitClick);

			gameFlowController = new GameFlowController (this);
		}
		
		// Update is called once per frame
		void Update () 
		{
			float deltaTime = Time.deltaTime;

			gameFlowController.Stay (deltaTime);
		}

		public void OnGameFlowUIClick (GameFlowUICmd cmd)
		{
			switch (cmd) 
			{
			case GameFlowUICmd.GameStart:
				{
					OnGameStart ();
					break;
				}

			case GameFlowUICmd.PlayHistory:
				{
					OnHistoryPlay ();
					break;
				}

			case GameFlowUICmd.Reset:
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

		void OnHistoryPlay ()
		{
			gameFlowController.ForceChangeState<GameHistoyState> ();
		}

		void OnGameReset()
		{
			gameFlowController.ForceChangeState<GameStandbyState> ();
			cubeController.Reset ();
			flowUIController.Reset ();
		}

		[ContextMenu("Test")]
		void Test ()
		{
			gameFlowController.ForceChangeState<GameHistoyState> ();
		}

		void OnApplicationQuitClick()
		{
			Application.Quit ();
		}
	}
}