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

			List<KeyValuePair<int,CubeBindDataGroup>> surfaceRootPairIndexs = new List<KeyValuePair<int, CubeBindDataGroup>> ();

			for (int i = 1; i < 7; i++) 
			{
				string surfaceRootAsset = string.Format (AssetKeys.RootFormat, i);
				
				CubeBindDataGroup cubeBindDataGroup = cubeRefBinder.GetComponent<CubeBindDataGroup> (surfaceRootAsset);

				surfaceRootPairIndexs.Add (new KeyValuePair<int, CubeBindDataGroup> (i, cubeBindDataGroup));
			}

			cubeEntityController = new CubeEntityController (centerPoint, surfaceRootPairIndexs, parseManager.CubeSetting.CubeEntitySetting);
		}
	}

}