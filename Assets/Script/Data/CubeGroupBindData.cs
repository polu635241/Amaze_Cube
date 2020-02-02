//#define DrawDebugLine
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	public class CubeGroupBindData : MonoBehaviour
	{
		public int GroupIndex
		{
			get
			{
				return groupIndex;
			}
		}

		[SerializeField]
		int groupIndex;

		public List<CubeRowBindData> HorizontalRows
		{
			get
			{
				return horizontalRows;
			}

			set
			{
				horizontalRows = value;
			}
		}

		[SerializeField]
		List<CubeRowBindData> horizontalRows = new List<CubeRowBindData> ();

		public List<CubeRowBindData> VerticalRows
		{
			get
			{
				return verticalRows;
			}

			set
			{
				verticalRows = value;
			}
		}

		[SerializeField]
		List<CubeRowBindData> verticalRows = new List<CubeRowBindData> ();

		#if UNITY_EDITOR && DrawDebugLine

		const float biggestWidth = 50f;

		void OnDrawGizmos ()
		{
			DrawCubeBindDataRow (horizontalRows, Color.blue);
			DrawCubeBindDataRow (verticalRows, Color.red);
		}

		void DrawCubeBindDataRow (List<CubeBindDataRow> rows, Color drawColor)
		{
			if (rows.Count > 0)
			{
				rows.ForEach (row=>
					{
						int rowItemCount = row.CubeEntitys.Count;

						if(rowItemCount > 0)
						{
							float rangeDeltaWidth = biggestWidth / rowItemCount;

							//最後一個點沒有下一個點了
							for (int rangeIndex = 0; rangeIndex < row.CubeEntitys.Count -1 ; rangeIndex++)
							{
								float rangeWidth = biggestWidth - (rangeDeltaWidth * rangeIndex);

								Transform currentPoint = row.CubeEntitys[rangeIndex];
								Transform nextPoint = row.CubeEntitys[rangeIndex +1];
								Color originColor = Gizmos.color;
								Gizmos.color = drawColor;
								Gizmos.DrawLine(currentPoint.position, nextPoint.position);
								Gizmos.color = originColor;
							}
						}
					});
			}
		}

		#endif
	}
}
