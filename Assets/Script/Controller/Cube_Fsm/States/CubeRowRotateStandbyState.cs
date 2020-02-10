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

			mouseBegintPos = cubeFlowData.MousePosCache;
		}

		public override CubeFlowState Stay (float deltaTime)
		{
			Vector3 mousePos;

			if (inputReceiver.ScreenTrigger(out mousePos))
			{	
				RaycastHit hit;
				
				if (cubeEntityController.Raycast (mousePos, out hit)) 
				{
					float mousePosDistance = Vector3.Distance (mouseBegintPos, mousePos);

					if (mousePosDistance > rowRotateNeedLength)
					{
						List<AxisDesciption> remainingAxisDesciptions = GetRemainingAxisDesciptions ();

						if (cubeEntityController.Raycast (mousePos, out hit))
						{
							Vector3 deltaHitPoint = hit.point - cubeFlowData.HitCache.point;

							remainingAxisDesciptions.ForEach (desc => 
							{
								desc.ReDot (deltaHitPoint);
							});

							remainingAxisDesciptions.Sort ((descA,descB)=>
								{
									float absDotValueA = Mathf.Abs (descA.DotValue);
									float absDotValueB = Mathf.Abs (descB.DotValue);

									Debug.LogError (descA.Axis);
									Debug.LogError (descA.DotValue);

									Debug.LogError (descB.Axis);
									Debug.LogError (descB.DotValue);

									//大到小排
									return -absDotValueA.CompareTo (absDotValueB);
								});

							AxisDesciption targetDesc = remainingAxisDesciptions [0];

							bool isPositive = (targetDesc.DotValue >= 0);

							RowRatateCacheData rowRatateCacheData = cubeEntityController.GetRowRatateCacheData (hitColl, targetDesc.Axis, isPositive);
							cubeFlowData.RowRatateCacheData = rowRatateCacheData;

							return GetState<CubeRowRotateState> ();
						}
						else	
						{
							return GetState<CubeWaitScreenUpState> ();
						}

						/*
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
						*/
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
			
		List<AxisDesciption> GetRemainingAxisDesciptions ()
		{
			List<AxisDesciption> remainingAxisDesciptions = new List<AxisDesciption> ();

			Vector3 hitNormal = cubeFlowData.HitCache.normal;
			Quaternion cubeWholeRot = cubeEntityController.CurrentWholeRot;

			//拿方塊的三軸 與 射線的法線 求內積 最大的那個就表示目前的平面與該軸垂直
			AxisDesciption xAxisDesciption = new AxisDesciption (RowRotateAxis.X, cubeWholeRot, hitNormal);
			remainingAxisDesciptions.Add (xAxisDesciption);

			AxisDesciption yAxisDesciption = new AxisDesciption (RowRotateAxis.Y, cubeWholeRot, hitNormal);
			remainingAxisDesciptions.Add (yAxisDesciption);

			AxisDesciption zAxisDesciption = new AxisDesciption (RowRotateAxis.Z, cubeWholeRot, hitNormal);
			remainingAxisDesciptions.Add (zAxisDesciption);

			remainingAxisDesciptions.Sort ((descA,descB)=>
				{
					float absDotValueA = Mathf.Abs (descA.DotValue);
					float absDotValueB = Mathf.Abs (descB.DotValue);

					//大到小排
					return -absDotValueA.CompareTo (absDotValueB);
				});

			//把垂直的軸排除掉 魔術方塊的機構上 垂直的軸不能轉
			Debug.LogError (remainingAxisDesciptions [0].Axis);
			Debug.LogError (remainingAxisDesciptions [0].DotValue);

			remainingAxisDesciptions.RemoveAt (0);

			return remainingAxisDesciptions;
		}
	}
}