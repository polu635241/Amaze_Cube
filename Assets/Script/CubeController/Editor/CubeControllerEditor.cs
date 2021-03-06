﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Kun.Tool;
using Kun.Data;
using Kun.HardwareInput;

namespace Kun.Controller
{
	[CustomEditor(typeof(CubeController))]
	public class CubeControllerEditor : SerializedObjectEditor<CubeController> 
	{
		Collider simulationTarget;

		bool isPositive = true;

		const string PositiveComment = "正向";
		const string NegativeComment = "反向";
		const float CenterInterval = 30f;

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			DrawRowRatateSimulation ();
		}

		void DrawRowRatateSimulation()
		{
			EditorTool.DrawInHorizontal (()=>
				{
					RowRotateAxis? currentFrameInputAxis = null;

					simulationTarget = EditorGUILayout.ObjectField (simulationTarget, typeof(Collider)) as Collider;

					GUILayout.FlexibleSpace ();

					EditorTool.DrawInVertical (()=>
						{
							EditorTool.DrawInHorizontal (()=>
								{
									GUILayout.FlexibleSpace ();
									
									DrawRowBtnGroup ("X", "仰角旋轉", ()=>
										{
											currentFrameInputAxis = RowRotateAxis.X;
										});

									GUILayout.FlexibleSpace ();
								});

							GUILayout.Space(CenterInterval);

							EditorTool.DrawInHorizontal (()=>
								{
									GUILayout.FlexibleSpace ();

									string comment = isPositive ? PositiveComment : NegativeComment;

									if(GUILayout.Button(comment, ToolBarButtonStyle , GUILayout.Width(buttonWidth)))
									{
										isPositive = !isPositive;
									}

									GUILayout.FlexibleSpace ();
								});

							GUILayout.Space(CenterInterval);

							EditorTool.DrawInHorizontal (()=>
								{
									DrawRowBtnGroup("Y", "漩渦旋轉", ()=>
										{
											currentFrameInputAxis = RowRotateAxis.Y;
										});

									GUILayout.FlexibleSpace ();

									DrawRowBtnGroup("Z", "水平旋轉", ()=>
										{
											currentFrameInputAxis = RowRotateAxis.Z;
										});
								});

							if(currentFrameInputAxis!=null)
							{
								if(EditorApplication.isPlaying)
								{
									if(simulationTarget!=null)
									{
										CubeEntityController cubeEntityController = runtimeScript.CubeEntityController;

										RowRotateAxis axis = currentFrameInputAxis.Value;

										int rowIndex;

										RowRatateCacheData rowRatateCacheData = cubeEntityController.GetRowRatateCacheData(simulationTarget, axis, isPositive, out rowIndex);

										runtimeScript.CubeFlowController.CubeFlowData.RowRatateCacheData = rowRatateCacheData;
									}
									else
									{
										Debug.LogError("模擬目標不得為空");
									}
								}
								else
								{
									Debug.LogError("請於播放後使用");
								}								
							}

						},boxSkin);
				});
		}

		void DrawRowBtnGroup (string btnName, string comment, Action callback)
		{
			EditorTool.DrawInVertical (()=>
				{
					EditorTool.DrawInHorizontal (()=>
						{
							if (GUILayout.Button (btnName, squareButtonLayoutOption))
							{
								callback.Invoke ();
							}
						});

					GUILayout.Label (comment);
				});
		}
	}
}