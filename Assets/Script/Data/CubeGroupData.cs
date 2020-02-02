using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class CubeGroupData
	{
		CubeEntityDataGroupStyle groupStyle;

		public CubeGroupData (int info, List<CubeRowData> horizontalRows, List<CubeRowData> verticalRows)
		{
			this.info = info;
			this.horizontalRows = new List<CubeRowData> (horizontalRows);
			this.verticalRows = new List<CubeRowData> (verticalRows);

			bool hasHorizontal;
			bool hasVetical;

			if (horizontalRows.Count > 0) 
			{
				hasHorizontal = true;
			}
			else
			{
				hasHorizontal = false;
			}

			if (verticalRows.Count > 0) 
			{
				hasVetical = true;
			}
			else
			{
				hasVetical = false;
			}

			if (hasHorizontal) 
			{
				if (!hasVetical) 
				{
					groupStyle = CubeEntityDataGroupStyle.Horizontal;
				}
				else
				{
					groupStyle = CubeEntityDataGroupStyle.CrossPoint;
				}
			}
			else
			{
				groupStyle = CubeEntityDataGroupStyle.Vetical;
			}
		}
		
		[SerializeField][ReadOnly][Header("對應骰子的1~6對應的面數")]
		int info;

		public List<CubeRowData> HorizontalRows
		{
			get
			{
				return horizontalRows;
			}
		}

        [SerializeField]
		List<CubeRowData> horizontalRows;

		public List<CubeRowData> VerticalRows
		{
			get
			{
				return verticalRows;
			}
		}

        [SerializeField]
        List<CubeRowData> verticalRows;

		[NonSerialized]
		CubeGroupData rightGroupData;

		[NonSerialized]
		CubeGroupData leftGroupData;

		[NonSerialized]
		CubeGroupData upGroupData;

		[NonSerialized]
		CubeGroupData downGroupData;

		public void SetWholeRot (Quaternion wholeRot)
		{
			if (groupStyle == CubeEntityDataGroupStyle.Vetical)
			{
				verticalRows.ForEach (row=>
					{
						row.CubeCacheDatas.ForEach(data=>
							{
								data.SetWholeRot(wholeRot);
							});
					});
			}
			else
			{
				horizontalRows.ForEach (row=>
					{
						row.CubeCacheDatas.ForEach(data=>
							{
								data.SetWholeRot(wholeRot);
							});
					});
			}
		}
	}

	enum CubeEntityDataGroupStyle
	{
		Horizontal,Vetical,CrossPoint
	}
}
