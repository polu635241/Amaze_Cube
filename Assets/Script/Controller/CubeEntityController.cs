using System;
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
		public CubeEntityController (CubeBindData cubeTotalBindData, CubeEntitySetting cubeEntitySetting)
		{
			this.centerPoint = cubeTotalBindData.CenterPoint;

			currentWholeRot = centerPoint.rotation;
			currentWholeEuler = centerPoint.eulerAngles;

			InitCubeEntityDatas (cubeTotalBindData);

			this.cubeEntitySetting = cubeEntitySetting;
		}

		const int intervalCount = 2;

		[SerializeField]
		Transform centerPoint;

		[SerializeField][ReadOnly]
		CubeEntitySetting cubeEntitySetting;

		[SerializeField][ReadOnly]
		Quaternion currentWholeRot;

		[SerializeField][ReadOnly]
		Vector3 currentWholeEuler;

		[SerializeField][ReadOnly]
		List<CubeCacheData> cubeCacheDatas = new List<CubeCacheData> ();

		[SerializeField]
		List<CubeRowData> x_RotateRows = new List<CubeRowData> ();

		[SerializeField]
		List<CubeRowData> y_RotateRows = new List<CubeRowData> ();

		[SerializeField]
		List<CubeRowData> z_RotateRows = new List<CubeRowData> ();

		public RowRatateCacheData GetRowRatateCacheData(Collider receiveColl, RowRotateDirection dir, bool isPositive)
		{
			List<CubeRowData> rotateRows = GetRotateRowsGroup (dir);

			CubeRowData ownerRow = rotateRows.Find (rotateRow=>
				{
					return rotateRow.CheckDataExist (receiveColl);
				});

			if (ownerRow != null) 
			{
				Quaternion deltaQuaterniotn = GetDeltaQuaternion (dir, isPositive);

				RowRatateCacheData rowRatateCacheData = new RowRatateCacheData (ownerRow, deltaQuaterniotn, isPositive);

				return rowRatateCacheData;
			}
			else
			{
				string name = receiveColl.gameObject.name;
				throw new UnityException ($"找不到所屬群組 name -> {name}, idr -> {dir}");
			}
		}

		void OnRowRotateFinish (CubeRowData ownerRow, bool isPositive)
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

			ProcessTransfer (x_RotateRows, transferPair);
			ProcessTransfer (y_RotateRows, transferPair);
			ProcessTransfer (z_RotateRows, transferPair);
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
				});
		}

		Quaternion GetDeltaQuaternion (RowRotateDirection dir, bool isPositive)
		{
			float scale = isPositive ? 1 : -1;

			float processDegree = 90 * scale;

			switch(dir)
			{
			case RowRotateDirection.X:
				{
					return Quaternion.Euler (processDegree, 0, 0);
				}

			case RowRotateDirection.Y:
				{
					return Quaternion.Euler (0, processDegree, 0);
				}

			case RowRotateDirection.Z:
				{
					return Quaternion.Euler (0, 0, processDegree);
				}
			}

			throw new Exception ("無對應旋轉設定");
		}

		List<CubeRowData> GetRotateRowsGroup (RowRotateDirection dir)
		{
			switch(dir)
			{
			case RowRotateDirection.X:
				{
					return x_RotateRows;
				}

			case RowRotateDirection.Y:
				{
					return y_RotateRows;
				}

			case RowRotateDirection.Z:
				{
					return z_RotateRows;
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

			x_RotateRows = GetEntityRows (cubeTotalBindData.X_RotateRows, cubeCacheDataMappings);
			y_RotateRows = GetEntityRows (cubeTotalBindData.Y_RotateRows, cubeCacheDataMappings);
			z_RotateRows = GetEntityRows (cubeTotalBindData.Z_RotateRows, cubeCacheDataMappings);
		}

		List<CubeRowData> GetEntityRows (List<CubeRowBindData> bindDataRows, Dictionary<Transform,CubeCacheData> cubeCacheDataMappings)
		{
			List<CubeRowData> entityRows = new List<CubeRowData> (); 

			if (bindDataRows != null) 
			{
				bindDataRows.ForEach (row=>
					{
						CubeRowData cubeEntityDataRow = row.GetEntityRow (cubeCacheDataMappings);

						entityRows.Add(cubeEntityDataRow);
					});
			}

			return entityRows;
		}
	}
}
