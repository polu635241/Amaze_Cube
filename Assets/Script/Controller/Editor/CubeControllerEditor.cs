using System;
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
		
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			DrawRowRatateSimulation ();
		}

		void DrawRowRatateSimulation()
		{
			EditorTool.DrawInHorizontal (()=>
				{
					RowRotateDirection? currentFrameInputDir = null;

					simulationTarget = EditorGUILayout.ObjectField (simulationTarget, typeof(Collider)) as Collider;

					GUILayout.FlexibleSpace ();

					EditorTool.DrawInVertical (()=>
						{
							EditorTool.DrawInHorizontal (()=>
								{
									GUILayout.FlexibleSpace ();

									if (GUILayout.Button ("Up", squareButtonLayoutOption))
									{
										currentFrameInputDir = RowRotateDirection.Up;
									}

									GUILayout.FlexibleSpace ();
								});

							EditorTool.DrawInHorizontal (()=>
								{
									GUILayout.FlexibleSpace ();

									if (GUILayout.Button ("Left", squareButtonLayoutOption))
									{
										currentFrameInputDir = RowRotateDirection.Left;
									}

									GUILayout.Space(squareButtonSize);

									if (GUILayout.Button ("Right", squareButtonLayoutOption))
									{
										currentFrameInputDir = RowRotateDirection.Right;
									}

									GUILayout.FlexibleSpace ();
								});


							EditorTool.DrawInHorizontal (()=>
								{
									GUILayout.FlexibleSpace ();

									if (GUILayout.Button ("Down", squareButtonLayoutOption))
									{
										currentFrameInputDir = RowRotateDirection.Down;
									}

									GUILayout.FlexibleSpace ();
								});

							if(currentFrameInputDir!=null)
							{
								if(EditorApplication.isPlaying)
								{
									if(simulationTarget!=null)
									{
										runtimeScript.CubeEntityController.RotateRow(simulationTarget,currentFrameInputDir.Value);
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
	}
}