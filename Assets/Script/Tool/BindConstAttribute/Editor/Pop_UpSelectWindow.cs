//#define debugMode
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Kun.Tool
{
	public class Pop_UpSelectWindow : GeneralEditorWindow 
	{
		public static void ShowWindow (SerializedProperty valueProperty, string[] originItems)
		{
			Pop_UpSelectWindow customSelectWindow = GetWindow<Pop_UpSelectWindow> ();

			customSelectWindow.Init (valueProperty, originItems);
			customSelectWindow.Show ();
		}
		Texture2D selectIcon;

		string cacheSearch;

		List<string> originItems = new List<string> ();
		List<string> processItems = new List<string> ();

		SerializedProperty valueProperty;

		string currentValue;
		int currentIndex;

		GUIStyle normalStyle;

		GUIStyle selectedStyle;

		GUIContent searchContent;

		void Init (SerializedProperty valueProperty, string[] originItems)
		{
			this.valueProperty = valueProperty;
			
			selectIcon = new Texture2D (1, 1);
			Color origanColor = new Color (1, 0.517701f, 0, 1);
			selectIcon.SetPixels (new Color[]{ origanColor });
			selectIcon.Apply ();
			searchContent = EditorGUIUtility.IconContent("ViewToolZoom");
			currentValue = valueProperty.stringValue;
			this.originItems = new List<string> (originItems);

			currentIndex = this.originItems.FindIndex (item => item == currentValue);

			processItems = new List<string> (this.originItems);

			normalStyle = new GUIStyle (EditorStyles.label);

			selectedStyle = new GUIStyle (normalStyle);

			selectedStyle.normal.background = selectIcon;

			selectedStyle.normal.textColor = Color.black;

			scrollPos = Vector2.zero;
		}

		void OnGUI()
		{
			if (setClose) 
			{
				return;
			}
			
			Event currentEvent = Event.current;

			DrawSearchField ();

			ProcessKeyEvent (currentEvent);

			#if debugMode
			EditorGUILayout.LabelField ($"currentEvent -> {currentEvent}");
			EditorGUILayout.LabelField ($"currentIndex -> {currentIndex}");
			#endif

			scrollPos = EditorTool.DrawInScrollView (scrollPos, selectScrollBarHeight,()=>
				{
					processItems.Map ((index,item)=>
						{
							DrawItem(index, item, currentEvent);
						});
				});

			if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow != this)
			{
				setClose = true;
			}

			if (setClose) 
			{
				Close ();
			}
		}

		void DrawSearchField ()
		{
			EditorTool.DrawInHorizontal (()=>
				{
					EditorGUILayout.LabelField ("Search : ", GUILayout.Width (70), GUILayout.Height(flatButtonHigh));
					cacheSearch = EditorGUILayout.TextField (cacheSearch, EditorStyles.textField, GUILayout.Height(flatButtonHigh));

					if(GUILayout.Button(searchContent, GUILayout.Width(buttonWidth), GUILayout.Height(flatButtonHigh)))
					{
						if (!string.IsNullOrEmpty (cacheSearch)) 
						{
							string processSearch = cacheSearch.ToLower ();

							processItems = originItems.FindAll (item => item.ToLower ().Contains (processSearch));
						}
						else
						{
							processItems = new List<string> (originItems);
						}

						currentIndex = processItems.IndexOf (currentValue);

					}
				});
		}

		void ProcessKeyEvent (Event currentEvent)
		{
			if (currentEvent.type == EventType.KeyDown) 
			{
				if (currentEvent.keyCode == KeyCode.UpArrow) 
				{
					if (currentIndex > 0) 
					{
						currentIndex--;

						currentValue = originItems [currentIndex];
					}

					currentEvent.Use();
				}

				if (currentEvent.keyCode == KeyCode.DownArrow) 
				{

					if (currentIndex < processItems.Count - 1)
					{
						currentIndex++;

						currentValue = originItems [currentIndex];
					}

					currentEvent.Use();
				}

				if (currentEvent.keyCode == KeyCode.Return) 
				{
					Flush ();

					currentEvent.Use();
				}
			}
		}

		bool setClose;

		void DrawItem (int index, string item, Event currentEvent)
		{
			GUIStyle guiStyle;

			if (index == currentIndex)
			{
				guiStyle = selectedStyle;
			}
			else
			{
				guiStyle = normalStyle;
			}
			
			EditorGUILayout.LabelField (item, guiStyle, GUILayout.Height(flatButtonHigh));

			Rect rect = GUILayoutUtility.GetLastRect ();

			if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0) 
			{
				if (rect.Contains (currentEvent.mousePosition)) 
				{
					currentIndex = index;
					currentValue = item;
					currentEvent.Use ();

					if (currentEvent.clickCount >= 2)
					{
						Flush ();
					}
				}
			}
		}

		void Flush()
		{	
			valueProperty.stringValue = currentValue;
			valueProperty.serializedObject.ApplyModifiedProperties ();
			setClose = true;
		}
	}
}