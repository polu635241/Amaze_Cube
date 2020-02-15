using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	public class CubeWholeIndexData
	{	
		public CubeWholeIndexData (CubeWholeData cubeWholeData, List<CubeCacheData> allCubeEntityDatas)
		{
			x_RotateIndexRows = GetRotateIndexRows (cubeWholeData.X_RotateRows, allCubeEntityDatas);
			y_RotateIndexRows = GetRotateIndexRows (cubeWholeData.Y_RotateRows, allCubeEntityDatas);
			z_RotateIndexRows = GetRotateIndexRows (cubeWholeData.Z_RotateRows, allCubeEntityDatas);
		}

		public List<CubeRowIndexData> X_RotateIndexRows
		{
			get
			{
				return x_RotateIndexRows;
			}
		}
		
		[SerializeField]
		List<CubeRowIndexData> x_RotateIndexRows = new List<CubeRowIndexData> ();

		public List<CubeRowIndexData> Y_RotateIndexRows
		{
			get
			{
				return y_RotateIndexRows;
			}
		}

		[SerializeField]
		List<CubeRowIndexData> y_RotateIndexRows = new List<CubeRowIndexData> ();

		public List<CubeRowIndexData> Z_RotateIndexRows
		{
			get
			{
				return z_RotateIndexRows;
			}
		}

		[SerializeField]
		List<CubeRowIndexData> z_RotateIndexRows = new List<CubeRowIndexData> ();

		List<CubeRowIndexData> GetRotateIndexRows (List<CubeRowData> cubeRowDatas, List<CubeCacheData> allCubeEntityDatas)
		{
			List<CubeRowIndexData> rotateIndexRows = new List<CubeRowIndexData> ();

			cubeRowDatas.ForEach (cubeRowData=>
				{
					CubeRowIndexData cubeRowIndexData = new CubeRowIndexData (cubeRowData, allCubeEntityDatas);
					rotateIndexRows.Add (cubeRowIndexData);
				});

			return rotateIndexRows;
		}

		public CubeWholeData GetValue (List<CubeCacheData> allCubeEntityDatas)
		{
			List<CubeRowData> x_RowDatas = GetRotateRowValues (x_RotateIndexRows, allCubeEntityDatas);
			List<CubeRowData> y_RowDatas = GetRotateRowValues (y_RotateIndexRows, allCubeEntityDatas);
			List<CubeRowData> z_RowDatas = GetRotateRowValues (z_RotateIndexRows, allCubeEntityDatas);

			CubeWholeData cubeWholeData = new CubeWholeData (x_RowDatas, y_RowDatas, z_RowDatas);
			return cubeWholeData;
		}

		List<CubeRowData> GetRotateRowValues (List<CubeRowIndexData> cubeRowIndexDatas, List<CubeCacheData> allCubeEntityDatas)
		{
			List<CubeRowData> rotateRowDatas = new List<CubeRowData> ();

			cubeRowIndexDatas.ForEach (cubeRowIndexData=>
				{
					CubeRowData cubeRowData = cubeRowIndexData.GetValue (allCubeEntityDatas);
					rotateRowDatas.Add (cubeRowData);
				});

			return rotateRowDatas;
		}
	}
}
