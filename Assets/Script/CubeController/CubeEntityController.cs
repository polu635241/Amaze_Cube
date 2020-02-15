﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;
using Kun.Data;
using Kun.HardwareInput;

namespace Kun.Controller
{
	[Serializable]
	public class CubeEntityController
	{
		public CubeEntityController (Camera mainCamera, CubeBindData cubeTotalBindData, CubeEntitySetting cubeEntitySetting)
		{
			this.centerPoint = cubeTotalBindData.CenterPoint;

			currentWholeRot = centerPoint.rotation;
			currentWholeEuler = centerPoint.eulerAngles;

			InitCubeEntityDatas (cubeTotalBindData);

			this.cubeEntitySetting = cubeEntitySetting;

			this.mainCamera = mainCamera;

			mainCameraTransform = mainCamera.transform;
		}

		const int intervalCount = 2;

		[SerializeField]
		Transform centerPoint;

		[SerializeField][ReadOnly]
		CubeEntitySetting cubeEntitySetting;

		public Quaternion CurrentWholeRot
		{
			get
			{
				return currentWholeRot;
			}
		}

		[SerializeField][ReadOnly]
		Quaternion currentWholeRot;

		[SerializeField][ReadOnly]
		Vector3 currentWholeEuler;

		[SerializeField][ReadOnly]
		List<CubeCacheData> cubeCacheDatas = new List<CubeCacheData> ();


		[SerializeField]
		Camera mainCamera;

		public Transform MainCameraTransform
		{
			get
			{
				return mainCameraTransform;
			}
		}

		[SerializeField][ReadOnly]
		CubeWholeData originCubeWholeData;

		[SerializeField][ReadOnly]
		CubeWholeData gamePlayCubeWholeData;

		[SerializeField]
		Transform mainCameraTransform;

		public RowRatateCacheData GetRowRatateCacheData(Collider receiveColl, RowRotateAxis axis, bool isPositive)
		{
			List<CubeRowData> rotateRows = GetRotateRowsGroup (axis);

			CubeRowData ownerRow = rotateRows.Find (rotateRow=>
				{
					return rotateRow.CheckDataExist (receiveColl);
				});

			if (ownerRow != null) 
			{
				Quaternion deltaQuaterniotn = GetDeltaQuaternion (axis, isPositive);

				RowRatateCacheData rowRatateCacheData = new RowRatateCacheData (ownerRow, deltaQuaterniotn, isPositive);

				return rowRatateCacheData;
			}
			else
			{
				string name = receiveColl.gameObject.name;
				throw new UnityException ($"找不到所屬群組 name -> {name}, axis -> {axis}");
			}
		}

		public void OnRowRotateFinish (CubeRowData ownerRow, bool isPositive)
		{
			Dictionary<CubeCacheData,CubeCacheData> transferPair = new Dictionary<CubeCacheData, CubeCacheData> ();

			List<CubeCacheData> cubeCacheDatas = ownerRow.CubeCacheDatas;

			int count = cubeCacheDatas.Count;

			cubeCacheDatas.Map ((index, cubeCacheData)=>
				{
					int needChangeIndex = 0;
					
					if(isPositive)
					{
						needChangeIndex = index + intervalCount;

						if(needChangeIndex > (count-1))
						{
							needChangeIndex -= count;
						}
					}
					else
					{
						needChangeIndex = index - intervalCount;

						if(needChangeIndex < (0))
						{
							needChangeIndex += count;
						}
					}

					CubeCacheData needChangeData = cubeCacheDatas[needChangeIndex];
					transferPair.Add (needChangeData, cubeCacheData);
				});

			ProcessTransfer (gamePlayCubeWholeData.X_RotateRows, transferPair);
			ProcessTransfer (gamePlayCubeWholeData.Y_RotateRows, transferPair);
			ProcessTransfer (gamePlayCubeWholeData.Z_RotateRows, transferPair);
		}

		void ProcessTransfer(List<CubeRowData> cubeRowDataGroup, Dictionary<CubeCacheData,CubeCacheData> transferPair)
		{
			cubeRowDataGroup.ForEach (cubeRowData=>
				{
					List<CubeCacheData> cubeCacheDatas = cubeRowData.CubeCacheDatas;

					//for 是為了保留集合的參考
					for (int i = 0; i < cubeCacheDatas.Count; i++) 
					{
						CubeCacheData transferCubeCaheData = null;

						if(transferPair.TryGetValue(cubeCacheDatas[i], out transferCubeCaheData))
						{
							cubeCacheDatas[i] = transferCubeCaheData;
						}
					}

					CubeCacheData transferCenterCubeCaheData = null;

					if(transferPair.TryGetValue(cubeRowData.RowCenterPoint , out transferCenterCubeCaheData))
					{
						cubeRowData.RowCenterPoint = transferCenterCubeCaheData;
					}
				});
		}

		Quaternion GetDeltaQuaternion (RowRotateAxis axis, bool isPositive)
		{
			float scale = isPositive ? 1 : -1;

			float processDegree = 90 * scale;

			switch(axis)
			{
			case RowRotateAxis.X:
				{
					return Quaternion.Euler (processDegree, 0, 0);
				}

			case RowRotateAxis.Y:
				{
					return Quaternion.Euler (0, processDegree, 0);
				}

			case RowRotateAxis.Z:
				{
					return Quaternion.Euler (0, 0, processDegree);
				}
			}

			throw new Exception ("無對應旋轉設定");
		}

		List<CubeRowData> GetRotateRowsGroup (RowRotateAxis dir)
		{
			switch(dir)
			{
			case RowRotateAxis.X:
				{
					return gamePlayCubeWholeData.X_RotateRows;
				}

			case RowRotateAxis.Y:
				{
					return gamePlayCubeWholeData.Y_RotateRows;
				}

			case RowRotateAxis.Z:
				{
					return gamePlayCubeWholeData.Z_RotateRows;
				}
			}

			throw new Exception ("無對應所屬群組");
		}

		public void RotateWhole (Vector3 deltaEuler, float deltaTime)
		{
			Vector3 processDeltaEuler = deltaEuler * cubeEntitySetting.RotateSpeed * deltaTime;

			Quaternion deltaRot = Quaternion.Euler (processDeltaEuler);

			currentWholeRot = deltaRot * currentWholeRot;
			currentWholeEuler = currentWholeRot.eulerAngles;

			cubeCacheDatas.ForEach (cubeEntityDataGroup=>
				{
					cubeEntityDataGroup.SetWholeRot (currentWholeRot);
				});
		}

		void InitCubeEntityDatas (CubeBindData cubeTotalBindData)
		{
			cubeCacheDatas = new List<CubeCacheData> ();

			Dictionary<Transform,CubeCacheData> cubeCacheDataMappings = new Dictionary<Transform, CubeCacheData> ();

			cubeTotalBindData.CubeEntitys.ForEach (cubeEntity=>
				{
					CubeCacheData cubeCacheData = new CubeCacheData(centerPoint, cubeEntity);
					cubeCacheDatas.Add (cubeCacheData);
					cubeCacheDataMappings.Add (cubeEntity, cubeCacheData);
				});

			originCubeWholeData = new CubeWholeData (cubeTotalBindData, cubeCacheDataMappings);
			gamePlayCubeWholeData = new CubeWholeData (originCubeWholeData);
		}

		public bool Raycast (Vector3 mousePos, out RaycastHit hit)
		{
			Ray ray = mainCamera.ScreenPointToRay (mousePos);

			Vector3 beginPos = ray.origin;
			float lineLength = 10f;
			Vector3 endPos = beginPos + ray.direction * lineLength;

			Debug.DrawLine(beginPos, endPos, Color.red, 0.1f);

			return Physics.Raycast (ray, out hit);
		}

		public void Reset()
		{
			cubeCacheDatas.ForEach (cubeCacheData=>
				{
					cubeCacheData.Reset();
				});

			gamePlayCubeWholeData = new CubeWholeData (originCubeWholeData);
		}
	}
}