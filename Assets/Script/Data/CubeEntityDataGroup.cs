using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	[Serializable]
	public class CubeEntityDataGroup
	{
		CubeEntityDataGroupStyle groupStyle;

		public CubeEntityDataGroup (int info, List<CubeEntityDataRow> horizontalRows, List<CubeEntityDataRow> verticalRows)
		{
			this.info = info;
			this.horizontalRows = new List<CubeEntityDataRow> (horizontalRows);
			this.verticalRows = new List<CubeEntityDataRow> (verticalRows);

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

		public List<CubeEntityDataRow> HorizontalRows
		{
			get
			{
				return horizontalRows;
			}
		}

		List<CubeEntityDataRow> horizontalRows;

		public List<CubeEntityDataRow> VerticalRows
		{
			get
			{
				return verticalRows;
			}
		}

		List<CubeEntityDataRow> verticalRows;

		[NonSerialized]
		CubeEntityDataGroup rightGroupData;

		[NonSerialized]
		CubeEntityDataGroup leftGroupData;

		[NonSerialized]
		CubeEntityDataGroup upGroupData;

		[NonSerialized]
		CubeEntityDataGroup downGroupData;

		public void SetWholeRot (Quaternion wholeRot)
		{
			if (groupStyle == CubeEntityDataGroupStyle.Vetical)
			{
				verticalRows.ForEach (row=>
					{
						row.CubeEntityDatas.ForEach(data=>
							{
								data.SetWholeRot(wholeRot);
							});
					});
			}
			else
			{
				horizontalRows.ForEach (row=>
					{
						row.CubeEntityDatas.ForEach(data=>
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
