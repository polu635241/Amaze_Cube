using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public class CubeHistortyRowRotateState : CubeRowRotateState
    {

		public CubeHistortyRowRotateState(CubeController cubeController, CubeFlowController cubeFlowController) : base (cubeController, cubeFlowController)
		{
		}

		public override void Enter (CubeFlowState prevState)
		{
			base.Enter (prevState);

			GameFlowData.InRowRotate = true;

            rowRatateCacheData = GameFlowData.HistoryRowRatateCacheData;
            GameFlowData.HistoryRowRatateCacheData = null;

            ProcessRotateCache ();
		}

		public override void Exit ()
		{
			base.Exit ();

			GameFlowData.InRowRotate = false;
		}

        /// <summary>
        /// 應該回歸的上一個狀態
        /// </summary>
        /// <returns></returns>
        protected override CubeFlowState GetPreveState ()
        {
            return GetState<CubeHistoryStandbyState> ();
        }
	}
}
