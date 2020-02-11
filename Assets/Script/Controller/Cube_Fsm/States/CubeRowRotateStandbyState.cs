﻿using System;
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
						if (cubeEntityController.Raycast (mousePos, out hit))
						{
							

							return GetState<CubeRowRotateState> ();
						}
						else	
						{
							return GetState<CubeWaitScreenUpState> ();
						}
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

		void ProcessRowRotateData (RaycastHit hit)
		{
			List<AxisDesciption> remainingAxisDesciptions = GetRemainingAxisDesciptions ();
			Vector3 deltaHitPoint = hit.point - cubeFlowData.HitCache.point;

			remainingAxisDesciptions.ForEach (desc => 
				{
					desc.ReDot (deltaHitPoint);
				});

			remainingAxisDesciptions.Sort ((descA,descB)=>
				{
					float absDotValueA = Mathf.Abs (descA.DotValue);
					float absDotValueB = Mathf.Abs (descB.DotValue);

					// 找最接近0 垂直的  轉軸會垂直施力方向
					return absDotValueA.CompareTo (absDotValueB);
				});

			AxisDesciption targetDesc = remainingAxisDesciptions [0];
			AxisDesciption anotherDesc = remainingAxisDesciptions [1];

			//手碰到方塊的時候 會有一個與射線法線相反的力去推動方塊
			Vector3 inverseNormal = hit.normal * -1;

			//兩個分力作cross
			Vector3 crossForce = Vector3.Cross (deltaHitPoint.normalized, inverseNormal);

			//如果求出來的 與 軸向 dot是負數的話 代表是向 逆時鐘旋轉
			targetDesc.ReDot (crossForce);

			bool isPositive = (targetDesc.DotValue >= 0);

			RowRatateCacheData rowRatateCacheData = cubeEntityController.GetRowRatateCacheData (hitColl, targetDesc.Axis, isPositive);
			cubeFlowData.RowRatateCacheData = rowRatateCacheData;
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
			remainingAxisDesciptions.RemoveAt (0);

			return remainingAxisDesciptions;
		}
	}
}