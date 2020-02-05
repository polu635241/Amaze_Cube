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

		public void RotateRow(Collider receiveColl, RowRotateDirection dir)
		{
//			CubeGroupData ownerGroupData = null;
//			CubeRowData ownerRowData = null;
//
//			ownerGroupData = cubeEntityDataGroups.Find (cubeEntityDataGrou=>
//				{
//					return cubeEntityDataGrou.CheckDataExist(receiveColl,dir, out ownerRowData);
//				});
//
//			if (ownerGroupData != null) 
//			{
//				Debug.LogError ("--------->");
//				Debug.Log ("dir" + dir.ToString ());
//			}
//			else
//			{
//				Debug.LogError ("搜尋不到對應的群組");
//			}
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
