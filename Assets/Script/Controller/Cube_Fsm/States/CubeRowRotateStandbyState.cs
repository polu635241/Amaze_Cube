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
					Vector3 mouseEndPos = hit.point;
					
					float mousePosDistance = Vector3.Distance (mouseBegintPos, mouseEndPos);

					if (mousePosDistance > rowRotateNeedLength)
					{
						Vector3 deltaPos = mouseEndPos - mouseBegintPos;

						Vector3 deltaPosInbox = wholeInverseRot * deltaPos;

//						Vector3 processDeltaPos = currentSurfaceInverseRot * deltaPosInbox;

						Debug.Log ($"deltaPos -> {deltaPos}");
						Debug.Log ($"deltaPosInbox -> {deltaPosInbox}");
//						Debug.Log ($"processDeltaPos -> {processDeltaPos}");

						PosDeltaData posDeltaData = GetPosDeltaData (deltaPosInbox);

						RowRatateCacheData rowRatateCacheData = GetRowRatateCacheData (hitColl, posDeltaData);
						cubeFlowData.RowRatateCacheData = rowRatateCacheData;

						return GetState<CubeRowRotateState> ();
					}
				}
				else
				{
					//進到等待放開狀態
					return GetState<CubeWaitScreenUpState> ();
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
			int axisIndex;
			bool isPositive;

			float delta_0 = GetValue (0, posDelta);
			float delta_1 = GetValue (1, posDelta);

			if (Mathf.Abs (delta_0) > Mathf.Abs (delta_1))
			{
				axisIndex = 0;

				if (delta_0 > 0) 
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
				axisIndex = 1;

				if (delta_1 > 0) 
				{
					isPositive = true;
				}
				else
				{
					isPositive = false;
				}
			}

			return new PosDeltaData (axisIndex, isPositive);
		}

		float GetValue(int index, Vector3 posDelta)
		{
			PositionSource positionSource = currentRotPairSurface.AxisPairs [index].PositionSource;

			switch (positionSource) 
			{
			case PositionSource.X:
				{
					return posDelta.x;
				}

			case PositionSource.Y:
				{
					return posDelta.y;
				}

			case PositionSource.Z:
				{
					return posDelta.z;
				}

			default:
				{
					Debug.LogError ("Mapping fail");
					return 0f;
				}
			}
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

		RowRatateCacheData GetRowRatateCacheData (Collider hitColl, PosDeltaData posDeltaData)
		{
			AxisPair settingAxisPair = null;

			settingAxisPair = currentRotPairSurface.AxisPairs[posDeltaData.AxisIndex];

			bool processIsPositive = Tool.Tool.ProcessBool (settingAxisPair.IsPositive, posDeltaData.IsPositive);

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