using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	public class CubeWholeData
	{	
		public CubeWholeData (CubeBindData cubeTotalBindData, Dictionary<Transform,CubeCacheData> cubeCacheDataMappings)
		{
			x_RotateRows = GetEntityRows (cubeTotalBindData.X_RotateRows, cubeCacheDataMappings);
			y_RotateRows = GetEntityRows (cubeTotalBindData.Y_RotateRows, cubeCacheDataMappings);
			z_RotateRows = GetEntityRows (cubeTotalBindData.Z_RotateRows, cubeCacheDataMappings);
		}

		public CubeWholeData (List<CubeRowData> x_RotateRows, List<CubeRowData> y_RotateRows, List<CubeRowData> z_RotateRows)
		{
			this.x_RotateRows = x_RotateRows;
			this.y_RotateRows = y_RotateRows;
			this.z_RotateRows = z_RotateRows;
		}

		public List<CubeRowData> X_RotateRows
		{
			get
			{
				return x_RotateRows;
			}
		}
		
		[SerializeField]
		List<CubeRowData> x_RotateRows = new List<CubeRowData> ();

		public List<CubeRowData> Y_RotateRows
		{
			get
			{
				return y_RotateRows;
			}
		}

		[SerializeField]
		List<CubeRowData> y_RotateRows = new List<CubeRowData> ();

		public List<CubeRowData> Z_RotateRows
		{
			get
			{
				return z_RotateRows;
			}
		}

		[SerializeField]
		List<CubeRowData> z_RotateRows = new List<CubeRowData> ();


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

		List<CubeRowData> GetCloneRows (List<CubeRowData> sourceRows)
		{
			List<CubeRowData> cloneRows = new List<CubeRowData> ();

			sourceRows.ForEach (sourceRow=>
				{
					CubeRowData cloneRowData= new CubeRowData();
					cloneRowData.SetUp (sourceRow);
					cloneRows.Add (cloneRowData);
				});

			return cloneRows;
		}

	}
}
