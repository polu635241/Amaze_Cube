using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.HardwareInput;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public class CubeRowRotateStandbyState : CubeFlowState 
	{
		float rowRotateNeedLength;
		Vector3 mouseBegintPos = new Vector3 ();

		List<RotPairSurface> rotPairSurfaces;

		RotPairSurface currentRotPairSurface;
		Quaternion wholeInverseRot = Quaternion.identity;
		Quaternion currentSurfaceInverseRot = Quaternion.identity;

		Collider hitColl;

		public CubeRowRotateStandbyState (CubeController cubeController, CubeFlowController cubeFlowController) : base (cubeController, cubeFlowController)
		{
			rowRotateNeedLength = cubeEntitySetting.RowRotateNeedLength;
			rotPairSurfaces = new List<RotPairSurface> (surfaceSetting.rotPairSurfaces);
		}

		public override void Enter (CubeFlowState prevState)
		{
			base.Enter (prevState);

			ProcessHit (cubeFlowData.HitCache);

			Debug.Log ($"index -> {currentRotPairSurface.Index}");

			mouseBegintPos = cubeFlowData.HitCache.point;
		}

		public override CubeFlowState Stay (float deltaTime)
		{
			Vector3 mousePos;

			if (inputReceiver.ScreenTrigger(out mousePos))
			{	
				RaycastHit hit;
				
				if (cubeEntityController.Raycast (mousePos, out hit)) 
				{
					Vector3 targetPos = hit.point;
					
					float mousePosDistance = Vector3.Distance (mouseBegintPos, targetPos);

					if (mousePosDistance > rowRotateNeedLength)
					{
						Vector3 deltaPos = targetPos - mouseBegintPos;

						Vector3 processDeltaPos = wholeInverseRot * currentSurfaceInverseRot * deltaPos;

						PosDeltaData posDeltaData = GetPosDeltaData (processDeltaPos);

						RowRatateCacheData rowRatateCacheData = GetRowRatateCacheData (hitColl, posDeltaData.IsHorizontal, posDeltaData.IsPositive);
						cubeFlowData.RowRatateCacheData = rowRatateCacheData;

						return GetState<CubeRowRotateState> ();
					}
				}
				else
				{
					//回到待命狀態
					return GetState<CubeStandbyState> ();
				}
			}
			else
			{
				//回到待命狀態
				return GetState<CubeStandbyState> ();
			}

			return null;
		}

		PosDeltaData GetPosDeltaData(Vector3 posDelta)
		{
			bool isHorizontal;
			bool isPositive;

			float deltaX = posDelta.x;
			float deltaY = posDelta.y;

			if (Mathf.Abs (deltaX) > Mathf.Abs (deltaY))
			{
				isHorizontal = true;

				if (deltaX > 0) 
				{
					isPositive = true;
				}
				else
				{
					isPositive = false;
				}
			}
			else
			{
				isHorizontal = false;

				if (deltaY > 0) 
				{
					isPositive = true;
				}
				else
				{
					isPositive = false;
				}
			}

			return new PosDeltaData (isHorizontal, isPositive);
		}

		void ProcessHit (RaycastHit hitCache)
		{
			hitColl = hitCache.collider;
			
			Quaternion currentWholeRot = cubeEntityController.CurrentWholeRot;

			wholeInverseRot = Quaternion.Inverse (currentWholeRot);

			//把該法線 從world 轉到方塊的local
			Vector3 processNormal = wholeInverseRot * hitCache.normal;

			RotPairSurface findRotPairSurface = rotPairSurfaces.Find (rotPairSurface =>
				{
					return rotPairSurface.Forward.Approximately (processNormal);
				});

			if (findRotPairSurface != null) 
			{
				currentRotPairSurface = findRotPairSurface;

				currentSurfaceInverseRot = Quaternion.Inverse (currentRotPairSurface.BindRot);
			}
			else
			{
				Debug.LogError ($"can't get RotPairSurface, normal -> {hitCache.normal}");
			}
		}

		RowRatateCacheData GetRowRatateCacheData (Collider hitColl, bool isHorizontal, bool isPositive)
		{
			AxisPair settingAxisPair = null;

			if (isHorizontal) 
			{
				settingAxisPair = currentRotPairSurface.HorizontalSetting;
			}
			else
			{
				settingAxisPair = currentRotPairSurface.VerticalSetting;
			}

			bool processIsPositive = Tool.Tool.ProcessBool (settingAxisPair.IsPositive, isPositive);

			RowRotateAxis axis = settingAxisPair.Axis;

			RowRatateCacheData rowRatateCacheData = cubeEntityController.GetRowRatateCacheData (hitColl, axis, processIsPositive);

			return rowRatateCacheData;
		}
			
		AxisPair GetAxisPair (Vector3 deltaPos)
		{
			return null;
		}
	}
}