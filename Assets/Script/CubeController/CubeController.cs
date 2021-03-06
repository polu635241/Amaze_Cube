﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;
using Kun.Data;
using Kun.HardwareInput;

namespace Kun.Controller
{
	//配合Editor 保留MonoBehaviour
	public class CubeController : MonoBehaviour
	{
		public GameController GameController{ get; private set;}
		
		public InputReceiver InputReceiver{ get; private set;}

		public CubeFlowController CubeFlowController
		{
			get
			{
				return cubeFlowController;
			}
		}

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

		public CubeSetting CubeSetting{ get; private set;}

		public void Init (RefBinder sceneRefBinder, CubeSetting cubeSetting, InputReceiver inputReceiver, GameController gameController)
		{
			this.InputReceiver = inputReceiver;
			this.GameController = gameController;
			InitController (sceneRefBinder, cubeSetting);
		}

		public void Stay (float deltaTime)
		{
			cubeFlowController.Stay (deltaTime);
		}

		void InitController (RefBinder sceneRefBinder, CubeSetting cubeSetting)
		{
			this.CubeSetting = cubeSetting;

			Camera mainCamera = sceneRefBinder.GetComponent<Camera> (AssetKeys.MainCamera);

			// 控制器會直接長在方塊上
			CubeBindData cubeTotalBindData = this.GetComponent<CubeBindData> ();

			cubeEntityController = new CubeEntityController (mainCamera, cubeTotalBindData, cubeSetting.CubeEntitySetting);

			cubeFlowController = new CubeFlowController (this);

			Destroy (cubeTotalBindData);
		}
			
		public void Reset ()
		{
			cubeEntityController.Reset ();
			cubeFlowController.Reset ();
		}
	}

}