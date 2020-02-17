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

            rowRatateCacheData = GameFlowData.HistoryRowRatateCacheData;
            GameFlowData.HistoryRowRatateCacheData = null;

            ProcessRotateCache ();
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
