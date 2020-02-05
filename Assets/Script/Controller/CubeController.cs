using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;
using Kun.Data;
using Kun.HardwareInput;

namespace Kun.Controller
{
	public class CubeController : GenericEntityController 
	{
		InputReceiver inputReceiver;

		public InputReceiver InputReceiver
		{
			get
			{
				return inputReceiver;
			}
		}

		ParseManager parseManager;

		[SerializeField][ReadOnly]
		CubeFlowController cubeFlowController;

		[SerializeField][ReadOnly]
		CubeEntityController cubeEntityController;

		public CubeEntityController CubeEntityController
		{
			get
			{
				return cubeEntityController;
			}
		}

		protected override void Awake ()
		{
			base.Awake ();
			InitController ();
		}

		void Update ()
		{
			float deltaTime = Time.deltaTime;

			cubeFlowController.Stay (deltaTime);
		}

		void InitController ()
		{
			parseManager = new ParseManager ();
			parseManager.ParseSettings ();
			
			inputReceiver = new KeyboardMouseInputReceiver ();

			RefBinder refBinder = this.GetComponent<RefBinder> ();

			Camera mainCamera = refBinder.GetComponent<Camera> (AssetKeys.MainCamera);

			cubeFlowController = new CubeFlowController (this, mainCamera);

			CubeBindData cubeTotalBindData = refBinder.GetComponent<CubeBindData> (AssetKeys.Cube);

			cubeEntityController = new CubeEntityController (cubeTotalBindData, parseManager.CubeSetting.CubeEntitySetting);
		}
	}

}