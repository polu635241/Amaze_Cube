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

			RefBinder cubeRefBinder = refBinder.GetComponent<RefBinder> (AssetKeys.Cube);

			Transform centerPoint = cubeRefBinder.GetComponent<Transform> (AssetKeys.CenterPoint);

			List<KeyValuePair<int,CubeGroupBindData>> surfaceRootPairIndexs = new List<KeyValuePair<int, CubeGroupBindData>> ();

			for (int i = 1; i < 7; i++) 
			{
				string surfaceRootAsset = string.Format (AssetKeys.RootFormat, i);
				
				CubeGroupBindData cubeBindDataGroup = cubeRefBinder.GetComponent<CubeGroupBindData> (surfaceRootAsset);

				surfaceRootPairIndexs.Add (new KeyValuePair<int, CubeGroupBindData> (i, cubeBindDataGroup));
			}

			cubeEntityController = new CubeEntityController (centerPoint, surfaceRootPairIndexs, parseManager.CubeSetting.CubeEntitySetting);
		}
	}

}