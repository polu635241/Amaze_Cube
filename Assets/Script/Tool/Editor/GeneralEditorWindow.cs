using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Kun.Tool
{
	public class GeneralEditorWindow : EditorWindow {

		public static float fieldNameWidth = 150f;
		public static float fieldKeyWidth = 80f;
		public static float buttonWidth = 100f;
		public static float defaultButtonHigh = 50f;
		public static float flatButtonHigh = 20f;

		public static float scriptFieldKeyWidth = 50f;

		public static Vector2 scrollPos = new Vector2();
		public static float scrollBarHeight = 500f;
		public static float selectScrollBarHeight = 250f;

		public static GUIStyle FieldNameGUIStyle
		{
			get
			{
				return EditorStyles.label;
			}
		}


		static GUIStyle richFieldNameGUIStyle;
		public static GUIStyle RichFieldNameGUIStyle
		{
			get
			{
				if (richFieldNameGUIStyle == null) 
				{
					richFieldNameGUIStyle = new GUIStyle (EditorStyles.label);
					richFieldNameGUIStyle.richText = true;
				}

				return richFieldNameGUIStyle;
			}
		}

		static GUIStyle orFieldNameGUIStyle = null;

		public static GUIStyle OrFieldNameGUIStyle
		{
			get
			{
				if (orFieldNameGUIStyle == null) 
				{
					orFieldNameGUIStyle = new GUIStyle (EditorStyles.label);
					orFieldNameGUIStyle.normal.textColor = Color.blue;
				}
				return orFieldNameGUIStyle;
			}
		}

		static GUIStyle andFieldNameGUIStyle = null;

		public static GUIStyle AndFieldNameGUIStyle
		{
			get
			{
				if (andFieldNameGUIStyle == null) 
				{
					andFieldNameGUIStyle = new GUIStyle (EditorStyles.label);
					andFieldNameGUIStyle.normal.textColor = Color.red;
				}
				return andFieldNameGUIStyle;
			}
		}

		GUIStyle worryFieldNameGUIStyle;

		protected GUIStyle WorryFieldNameGUIStyle
		{
			get
			{
				if (worryFieldNameGUIStyle == null) 
				{
					worryFieldNameGUIStyle = new GUIStyle (TitleNameGUIStyle);

					worryFieldNameGUIStyle.normal.textColor = Color.red;
					worryFieldNameGUIStyle.fontStyle = FontStyle.Bold;
				}

				return worryFieldNameGUIStyle;
			}
		}

		GUIStyle worryBigFieldNameGUIStyle;

		protected GUIStyle WorryBigFieldNameGUIStyle
		{
			get
			{
				if (worryBigFieldNameGUIStyle == null) 
				{
					worryBigFieldNameGUIStyle = new GUIStyle (TitleNameGUIStyle);

					worryBigFieldNameGUIStyle.normal.textColor = Color.red;
					worryBigFieldNameGUIStyle.fontStyle = FontStyle.Bold;
					worryBigFieldNameGUIStyle.fontSize = 15;
				}

				return worryBigFieldNameGUIStyle;
			}
		}

		GUIStyle tapBigFieldNameGUIStyle;

		protected GUIStyle TapBigFieldNameGUIStyle
		{
			get
			{
				if (tapBigFieldNameGUIStyle == null) 
				{
					tapBigFieldNameGUIStyle = new GUIStyle (TitleNameGUIStyle);

					tapBigFieldNameGUIStyle.normal.textColor = new Color (0.3137f, 0.6212f, 0.8867f);
					tapBigFieldNameGUIStyle.fontStyle = FontStyle.Bold;
					tapBigFieldNameGUIStyle.fontSize = 15;
				}

				return tapBigFieldNameGUIStyle;
			}
		}


		public static GUIStyle TitleNameGUIStyle
		{
			get
			{
				return EditorStyles.boldLabel;
			}
		}

		protected GUIStyle DefaultButtonGUIStyle
		{
			get
			{
				return EditorStyles.miniButton;
			}
		}

		public static GUIStyle FlatButtonGUIStyle
		{
			get
			{
				return EditorStyles.toolbarButton;
			}
		}

		protected GUIStyle RadioButtonGUIStyle
		{
			get
			{
				return EditorStyles.radioButton;
			}
		}


		public static GUIStyle BoxGUIStyle
		{
			get
			{
				return GUI.skin.box;
			}
		}


		static GUIStyle orBoxGUIStyle;
		public static GUIStyle OrBoxGUIStyle
		{
			get
			{
				if (orBoxGUIStyle == null) 
				{
					orBoxGUIStyle = new GUIStyle (BoxGUIStyle);
					orBoxGUIStyle.normal.background = MakeTex (600, 1, new Color (0.8392f, 0.9249f, 1, 1));
				}

				return orBoxGUIStyle;
			}
		}

		static GUIStyle andBoxGUIStyle;
		public static GUIStyle AndBoxGUIStyle
		{
			get
			{
				if (andBoxGUIStyle == null) 
				{
					andBoxGUIStyle = new GUIStyle (BoxGUIStyle);
					andBoxGUIStyle.normal.background = MakeTex (600, 1, Color.red);
				}

				return andBoxGUIStyle;
			}
		}

		static Texture2D MakeTex(int width, int height, Color col)
		{
			Color[] pix = new Color[width*height];

			for(int i = 0; i < pix.Length; i++)
				pix[i] = col;

			Texture2D result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();

			return result;
		}

		protected float areaInterval = 10f;

		protected float buttonHorzInterval = 30f;

		int originIndentLevel;

		protected void SetIndentLevelZero()
		{
			originIndentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
		}

		protected void RevertIndentLevel()
		{
			EditorGUI.indentLevel = originIndentLevel;
		}

		protected void DrawSerializablePoint(SerializedProperty rootProperty,string fieldName)
		{
			SerializedProperty serializablePointProperty = rootProperty.FindPropertyRelative (fieldName);

			EditorGUI.indentLevel--;
			bool openArea = EditorGUILayout.PropertyField (serializablePointProperty);
			EditorGUI.indentLevel++;

			if (openArea) 
			{
				EditorGUI.indentLevel++;
				DrawSerializableVector3 (serializablePointProperty, "position");
				DrawSerializableVector3 (serializablePointProperty, "rotation");
				EditorGUI.indentLevel--;
			}
		}

		protected void DrawSerializableVector3(SerializedProperty rootProperty,string fieldName)
		{
			SerializedProperty serializableVector3Property = rootProperty.FindPropertyRelative (fieldName);

			EditorGUI.indentLevel--;
			bool openArea = EditorGUILayout.PropertyField (serializableVector3Property);
			EditorGUI.indentLevel++;

			if (openArea) 
			{
				EditorGUI.indentLevel++;
				DrawCustomSerlizedField ("x", serializableVector3Property);
				DrawCustomSerlizedField ("y", serializableVector3Property);
				DrawCustomSerlizedField ("z", serializableVector3Property);
				EditorGUI.indentLevel--;
			}
		}

		protected void DrawSerializableQuaternion(SerializedProperty rootProperty,string fieldName)
		{
			SerializedProperty serializableQuaternionProperty = rootProperty.FindPropertyRelative (fieldName);

			EditorGUI.indentLevel--;
			bool openArea = EditorGUILayout.PropertyField (serializableQuaternionProperty);
			EditorGUI.indentLevel++;

			if (openArea) 
			{
				EditorGUI.indentLevel++;
				DrawCustomSerlizedField ("x", serializableQuaternionProperty);
				DrawCustomSerlizedField ("y", serializableQuaternionProperty);
				DrawCustomSerlizedField ("z", serializableQuaternionProperty);
				DrawCustomSerlizedField ("w", serializableQuaternionProperty);
				EditorGUI.indentLevel--;
			}
		}

		protected void DrawVariableField (string variableName, Action drawAndGetInput, float? overrideFieldWidth = null)
		{
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField (variableName, FieldNameGUIStyle, GUILayout.Width (fieldNameWidth));
				drawAndGetInput.Invoke ();
			}
			EditorGUILayout.EndHorizontal ();
		}

		protected void DrawCustomSerlizedField(string key,SerializedProperty rootProperty)
		{
			EditorGUILayout.BeginHorizontal ();
			{
				string KeyFieldName = key;

				EditorGUILayout.LabelField (KeyFieldName, GUILayout.Width (200));

				SerializedProperty findProperty = rootProperty.FindPropertyRelative (KeyFieldName);

				EditorGUILayout.PropertyField (findProperty, new GUIContent (""));
			}
			EditorGUILayout.EndHorizontal ();
		}

		protected class SlotTogglesCache
		{
			List<RefKeyValuePair<List<int>,bool>> cache = new List<RefKeyValuePair<List<int>, bool>>();

			//List是ref type 要用value type的比較法
			bool CheckIsSameKey(List<int> key1,List<int> key2)
			{
				if (key1.Count != key2.Count)
				{
					return false;
				}
				else
				{
					for (int i = 0; i < key2.Count; i++) 
					{
						if (key1 [i] != key2 [i])
							return false;
					}

					return true;
				}
			}


			public void Set(int key,bool value)
			{
				Set (new List<int>{ key }, value);
			}


			public void Set(List<int> key,bool value)
			{
				var finder = cache.Find (pair => CheckIsSameKey (pair.Key, key));

				if (finder == null) 
				{
					finder = new RefKeyValuePair<List<int>, bool> ();
					finder.Key = key;
					cache.Add (finder);
				}

				finder.Value = value;
			}

			public bool Get(int key)
			{
				return Get (new List<int>{ key });
			}


			public bool Get(List<int> key)
			{
				var finder = cache.Find (pair => CheckIsSameKey (pair.Key, key));

				if (finder == null) 
				{
					return false;
				}
				else
				{
					return finder.Value;
				}
			}

			public void Remove(int key)
			{
				Remove (new List<int>{ key });
			}

			public void Remove(List<int> compareKey)
			{
				//依據root刪除緩存 root符合就刪除 後面的不管ex 傳入0 -> 01 00 都要刪除
				cache.RemoveAll (pair => 
					{
						for (int i = 0; i < compareKey.Count; i++) 
						{
							return false;
						}
						return true;
					});

				//如果最後那位以外的位數不一樣的話 直接跳過 找出前幾位一樣最後那位比目標還大的
				var finderPairs = cache.FindAll (pair=>
					{
						for (int i = 0; i < compareKey.Count; i++) 
						{
							if(compareKey[i] != pair.Key[i])
							{
								if(i<compareKey.Count-1)
								{
									return false;
								}
								else
								{
									if(pair.Key[i]>compareKey[i])
									{
										return true;
									}
									else
									{
										return false;
									}
								}
							}
						}
						return true;
					});

				//剩下的以key的最後一位作依據 把對應的位數往前移一格
				finderPairs.ForEach (finderPair=>
					{
						finderPair.Key[compareKey.Count-1]-=1;
					});
			}
		}

		protected void SetSerializablePoint(SerializedProperty rootProperty,string fieldName,SerializablePoint point)
		{
			SerializedProperty pointProperty = rootProperty.FindPropertyRelative (fieldName);

			SetSerializableVector3 (pointProperty, "position",point.position);
			SetSerializableVector3 (pointProperty, "rotation", point.rotation);
		}

		protected void SetSerializableVector3(SerializedProperty rootProperty,string fieldName,SerializableVector3 v3)
		{
			SerializedProperty v3Property = rootProperty.FindPropertyRelative (fieldName);
			v3Property.FindPropertyRelative ("x").floatValue = v3.x;
			v3Property.FindPropertyRelative ("y").floatValue = v3.y;
			v3Property.FindPropertyRelative ("z").floatValue = v3.z;
		}

		protected SerializablePoint GetSerializablePoint(SerializedProperty rootProperty,string fieldName)
		{
			SerializablePoint point = new SerializablePoint ();

			SerializedProperty pointProperty = rootProperty.FindPropertyRelative (fieldName);

			point.position = GetSerializableVector3 (pointProperty, "position");
			point.rotation = GetSerializableVector3 (pointProperty, "rotation");

			return point;
		}

		SerializableVector3 GetSerializableVector3(SerializedProperty rootProperty,string fieldName)
		{
			SerializableVector3 v3 = new SerializableVector3();

			SerializedProperty v3Property = rootProperty.FindPropertyRelative (fieldName);

			v3.x = v3Property.FindPropertyRelative ("x").floatValue;
			v3.y = v3Property.FindPropertyRelative ("y").floatValue;
			v3.z = v3Property.FindPropertyRelative ("z").floatValue;

			return v3;
		}
			
		protected class RemoveAndAddCache
		{
			public bool hasAdd;
			public int? hasRemoveIndex;

			event Action addEvent;
			event Action<int> removeAtEvent;

			public RemoveAndAddCache(Action addEvent,Action<int> removeAtEvent)
			{
				this.addEvent = addEvent;
				this.removeAtEvent = removeAtEvent;
			}

			public void Flush()
			{
				if (hasAdd) 
				{
					if (addEvent != null)
						addEvent.Invoke ();
					
					hasAdd = false;
				}

				if (hasRemoveIndex!=null) 
				{
					if (removeAtEvent != null)
						removeAtEvent.Invoke (hasRemoveIndex.Value);
					
					hasRemoveIndex = null;
				}
			}
		}

		public static void InvokeInNoIndentLevel(Action innerAction)
		{
			int originIndentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			innerAction.Invoke ();
			EditorGUI.indentLevel = originIndentLevel;
		}

		#region Dialog Message

		/// <summary>
		/// 此關卡含有隨機群組 請問是否套用隨機
		/// </summary>
		protected Func<bool> RandomDialogCondition = () => 
		{
			return EditorUtility.DisplayDialog ("Title", "此關卡含有隨機群組 請問是否套用隨機", 
				"yes", "no");
		};

		/// <summary>
		/// 移除後不能還原 確定要移除嗎?
		/// </summary>
		protected Func<bool> RemoveDialogCondition = () => 
		{
			return EditorUtility.DisplayDialog ("Title", "移除後不能還原 確定要移除嗎?",
				"yes", "no");
		};

		/// <summary>
		/// 場景中尚未存儲的變更會遺失 確定繼續嗎?
		/// </summary>
		protected Func<bool> RefreshDialogCondition = () => 
		{
			return EditorUtility.DisplayDialog ("Title", "場景中尚未存儲的變更會遺失 確定繼續嗎?",
				"yes", "no");
		};

		#endregion
	}
}