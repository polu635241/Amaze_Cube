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
		[SerializeField][ReadOnly]
		CubeController cubeController;

		[SerializeField][ReadOnly]
		GameFlowController gameFlowController;

		[SerializeField][ReadOnly]
		ParseManager parseManager;

		KeyboardMouseInputReceiver keyboardMouseInputReceiver;


		// Use this for initialization
		void Awake () 
		{
			keyboardMouseInputReceiver = new KeyboardMouseInputReceiver ();
			
			parseManager = new ParseManager ();
			parseManager.ParseSettings ();
			
			GameObject sceneRefBinderGo = GameObject.FindWithTag (Tags.SceneRefBinder);

			RefBinder sceneRefBinder = sceneRefBinderGo.GetComponent<RefBinder> ();

			GameObject cubeGo = sceneRefBinder.GetGameobject (AssetKeys.Cube);
			cubeController = cubeGo.AddComponent<CubeController> ();
			cubeController.Init (sceneRefBinder, parseManager.CubeSetting, keyboardMouseInputReceiver);

			gameFlowController = new GameFlowController (this);
		}
		
		// Update is called once per frame
		void Update () 
		{
			float deltaTime = Time.deltaTime;

			gameFlowController.Stay (deltaTime);
			cubeController.Stay (deltaTime);
		}
	}
}