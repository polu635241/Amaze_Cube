using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;
using Kun.HardwareInput;

namespace Kun.Controller
{
	public class CubeController : GenericEntityController {

		InputReceiver inputReceiver;

		public InputReceiver InputReceiver
		{
			get
			{
				return inputReceiver;
			}
		}

		[SerializeField][ReadOnly]
		CubeFlowController cubeFlowController;

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
			inputReceiver = new KeyboardMouseInputReceiver ();
			cubeFlowController = new CubeFlowController (this);
		}
	}
}