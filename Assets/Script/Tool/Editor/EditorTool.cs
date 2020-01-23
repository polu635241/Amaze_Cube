using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace Kun.Tool
{
	public static class EditorTool {

		public static float HorizontalIndentSpace = 14f;

		static float fieldNameWidth = 150f;
		static float fieldKeyWidth = 80f;
		static GUIStyle fieldNameGUIStyle
		{
			get
			{
				return EditorStyles.miniLabel;
			}
		}
		static GUIStyle titleNameGUIStyle
		{
			get
			{
				return EditorStyles.boldLabel;
			}
		}
		static BindingFlags checkEnumBindFlags = BindingFlags.Public | BindingFlags.Static;

		public static void DrawGeneralObject (object source,string fieldName)
		{
			GUILayout.Label (fieldName, titleNameGUIStyle, GUILayout.Width (fieldNameWidth));

			EditorGUI.indentLevel++;

			Type type = source.GetType ();

			if (!type.IsSerializable)
				throw new Exception("not system Serializable");

			List<FieldInfo> fieldInfos = new List<FieldInfo> (type.GetFields());

			fieldInfos.ForEach ((fieldInfo)=>
				{
					Type fieldType = fieldInfo.FieldType;

					if(fieldType.IsArray)
					{
						DrawArrayItem(fieldInfo,source);
					}
					else
					{
						if(fieldType.IsGenericType)
						{
							if(fieldType.IsList())
							{
								DrawListItem(fieldInfo,source);
							}
							else
							{
								Debug.LogError("尚未支援array以及list以外的容器");
							}
						}
						else
						{
							DrawSingleItem(fieldInfo,source);
						}
					}
				});

			EditorGUI.indentLevel--;
		}

		static void DrawListItem(FieldInfo fieldInfo,object source)
		{
			Type fieldType = fieldInfo.FieldType;

			object listObject = fieldInfo.GetValue(source);

			int count = Convert.ToInt32(fieldType.GetProperty("Count").GetValue(listObject));

			PropertyInfo propertyInfo = fieldType.GetProperty ("Item");

			for (int i = 0; i < count; i++) 
			{
				object item = propertyInfo.GetValue (listObject, new object[]{ i });

				DrawSingleRepeatedItme (item,(obj)=>
					{
						propertyInfo.SetValue(listObject,obj, new object[]{ i });
					});
			}
		}

		static void DrawArrayItem(FieldInfo fieldInfo,object source)
		{
			Type fieldType = fieldInfo.FieldType;

			Array arrayObject = (Array)fieldInfo.GetValue (source);

			for (int i = 0; i < arrayObject.Length; i++) 
			{
				object item = arrayObject.GetValue (i);

				DrawSingleRepeatedItme (item,(obj)=>
					{
						arrayObject.SetValue(obj,i);
					});
			}
		}


		static void DrawSingleRepeatedItme(object item,Action<object> onValueModify)
		{
			Type fieldType = item.GetType ();

			if (fieldType.IsArray || (fieldType.IsGenericType)) 
			{
				Debug.LogError ("尚未支援多重容器");
			} 
			else
			{
				if (fieldType.IsClass)
				{
					if (fieldType == typeof(String))
					{
						string oldValue = (string)item;

						//TODO 字串
						string newValue= EditorGUILayout.TextField (oldValue, GUILayout.Width (fieldKeyWidth));

						if(newValue!=oldValue)
						{
							onValueModify.Invoke (newValue);
						}
					}
					else
					{
						//TODO Custom Class
					}
				}
				else if(fieldType.IsPrimitive)
				{
					//TODO 字串以外的基礎型別
					if(fieldType==typeof(int))
					{
						int oldValue = (int)item;

						int newValue = EditorGUILayout.IntField ("", oldValue, GUILayout.Width (fieldKeyWidth));

						if(newValue!=oldValue)
						{
							onValueModify.Invoke (newValue);
						}
					}
					else if (fieldType == typeof(float))
					{
						float oldValue = (float)item;

						float newValue = EditorGUILayout.FloatField ("",oldValue, GUILayout.Width (fieldKeyWidth));

						if(newValue!=oldValue)
						{
							onValueModify.Invoke (newValue);
						}
					}
				}
				else
				{
					if(fieldType.IsEnum)
					{
						int oldValue = (int)item;

						FieldInfo[] fieldInfos = fieldType.GetFields (checkEnumBindFlags);

						string[] enumFieldNames = new string[fieldInfos.Length];

						for (int i = 0; i < fieldInfos.Length; i++) 
						{
							enumFieldNames [i] = fieldInfos [i].Name;
						}

						int newValue = EditorGUILayout.Popup (oldValue, enumFieldNames, GUILayout.Width (fieldKeyWidth));

						if(newValue!=oldValue)
						{
							onValueModify.Invoke (newValue);
						}
					}
					else
					{
						Debug.LogError ("尚未支援Sturct");
					}
				}
			}
		}

		static void DrawSingleItem (FieldInfo fieldInfo,object source)
		{
			string fieldName = fieldInfo.Name;
			Type fieldType = fieldInfo.FieldType;
			object fieldValue= fieldInfo.GetValue(source);

			if (fieldType.IsClass)
			{
				if (fieldType == typeof(String))
				{
					string oldValue = (string)fieldValue;

					//TODO 字串
					string newValue= DrawStringField(fieldName,oldValue);

					if(newValue!=oldValue)
					{
						fieldInfo.SetValue(source,newValue);
					}
				}
				else
				{
					//TODO Custom Class
				}
			}
			else if(fieldType.IsPrimitive)
			{
				//TODO 字串以外的基礎型別
				if(fieldType==typeof(int))
				{
					int oldValue = (int)fieldValue;

					int newValue = DrawIntField(fieldName,oldValue);

					if(newValue!=oldValue)
					{
						fieldInfo.SetValue(source,newValue);
					}
				}
				else if (fieldType == typeof(float))
				{
					float oldValue = (float)fieldValue;

					float newValue = DrawFloatField(fieldName,oldValue);

					if(newValue!=oldValue)
					{
						fieldInfo.SetValue(source,newValue);
					}
				}
			}
			else
			{
				if(fieldType.IsEnum)
				{
					int oldValue = (int)fieldValue;

					int newValue = DrawEnumField(fieldType,fieldName,oldValue);

					if(newValue!=oldValue)
					{
						fieldInfo.SetValue(source,newValue);
					}
				}
				else
				{
					Debug.LogError ("尚未支援Sturct");
				}
			}
		}

		static bool IsList(this Type type)
		{
			return type.GetGenericTypeDefinition () == typeof(List<>);
		}


		public static string DrawStringField (string fieldName, string fieldValue)
		{
			string result = "";

			Action drawInputPart = () => 
			{
				result = EditorGUILayout.TextField (fieldValue, GUILayout.Width (fieldKeyWidth));
			};

			DrawField (fieldName, drawInputPart);

			return result;
		}

		public static int DrawIntField (string fieldName, int fieldValue)
		{
			int result = 0;

			Action drawInputPart = () => 
			{
				result = EditorGUILayout.IntField ("",fieldValue, GUILayout.Width (fieldKeyWidth));
			};

			DrawField (fieldName, drawInputPart);

			return result;
		}

		public static float DrawFloatField (string fieldName, float fieldValue)
		{
			float result = 0f;

			Action drawInputPart = () => 
			{
				result = EditorGUILayout.FloatField ("",fieldValue, GUILayout.Width (fieldKeyWidth));
			};

			DrawField (fieldName, drawInputPart);

			return result;
		}

		public static string[] GetEnumFields(Type enumType)
		{
			FieldInfo[] fieldInfos = enumType.GetFields (checkEnumBindFlags);

			string[] enumFieldNames = new string[fieldInfos.Length];

			for (int i = 0; i < fieldInfos.Length; i++) 
			{
				enumFieldNames [i] = fieldInfos [i].Name;
			}

			return enumFieldNames;
		}

		public static int DrawEnumField (Type enumType,string fieldName, int selectValue)
		{
			string[] enumFieldNames = GetEnumFields (enumType);

			int result = 0;

			Action drawInputPart = () => 
			{
				result = EditorGUILayout.Popup(selectValue,enumFieldNames,GUILayout.Width(fieldKeyWidth));
			};

			DrawField (fieldName, drawInputPart);

			return result;
		}

		static void DrawField(string fieldName,Action callback)
		{
			string processField = fieldName;

			fieldNameGUIStyle.alignment = TextAnchor.MiddleLeft;

			GUILayout.BeginHorizontal ();
			{
				GUILayout.Label (processField, fieldNameGUIStyle, GUILayout.Width (fieldNameWidth));
				callback.Invoke ();
			}
			GUILayout.EndHorizontal ();
		}

		public class GeneralGUILayout
		{
			public static GUIStyle button = EditorStyles.miniButton;
			public static GUIStyle titleName = EditorStyles.label;
			public static GUIStyle fieldName = EditorStyles.miniLabel;
			public static float  classSpace = 10f;
		}

		public static void Foreach(this SerializedProperty arrayProperty,Action<SerializedProperty> loopAction)
		{
			int originSize = arrayProperty.arraySize;

			for (int i = 0; i < originSize; i++) 
			{
				SerializedProperty item = arrayProperty.GetArrayElementAtIndex (i);
				loopAction.Invoke (item);
			}
		}

		public static void Foreach(this SerializedProperty arrayProperty,Action<SerializedProperty,int> loopAction)
		{
			int originSize = arrayProperty.arraySize;

			for (int i = 0; i < originSize; i++) 
			{
				SerializedProperty item = arrayProperty.GetArrayElementAtIndex (i);
				loopAction.Invoke (item, i);
			}
		}


		public static SerializedProperty AddNewOne(this SerializedProperty arrayProperty)
		{
			int originSize = arrayProperty.arraySize;

			arrayProperty.InsertArrayElementAtIndex (originSize);

			return arrayProperty.GetArrayElementAtIndex (originSize);
		}

		public static SerializedProperty Find (this SerializedProperty arrayProperty, string queryFieldName, int value)
		{
			SerializedProperty findProperty = null;

			findProperty = FindInternal (arrayProperty,(item)=>
				{
					return item.FindPropertyRelative(queryFieldName).intValue == value;
				});

			return findProperty;
		}

		public static SerializedProperty Find (this SerializedProperty arrayProperty, string queryFieldName, float value)
		{
			SerializedProperty findProperty = null;

			findProperty = FindInternal (arrayProperty,(item)=>
				{
					return item.FindPropertyRelative(queryFieldName).floatValue == value;
				});

			return findProperty;
		}

		public static SerializedProperty Find (this SerializedProperty arrayProperty, string queryFieldName, string value)
		{
			SerializedProperty findProperty = null;

			findProperty = FindInternal (arrayProperty,(item)=>
				{
					return item.FindPropertyRelative(queryFieldName).stringValue == value;
				});

			return findProperty;
		}

		static SerializedProperty FindInternal(SerializedProperty arrayProperty,Func<SerializedProperty,bool> condition)
		{
			int originSize = arrayProperty.arraySize;

			SerializedProperty findProperty = null;

			for (int i = 0; i < originSize; i++) 
			{
				SerializedProperty item = arrayProperty.GetArrayElementAtIndex (i);

				if (condition.Invoke (item)) 
				{
					findProperty = item;
					break;
				}
			}

			return findProperty;
		}

		public static void DrawInHorizontal(Action body, GUIStyle style = null)
		{
			if (style == null)
			{
				EditorGUILayout.BeginHorizontal();
			}
			else
			{
				EditorGUILayout.BeginHorizontal(style);
			}

			body.Invoke();
			EditorGUILayout.EndHorizontal();
		}

		public static void DrawInReadOnly(Action body)
		{
			GUI.enabled = false;
			body.Invoke ();
			GUI.enabled = true;
		}

		public static void DrawInVertical(Action body, GUIStyle style = null)
		{
			if (style == null)
			{
				EditorGUILayout.BeginVertical();
			}
			else
			{
				EditorGUILayout.BeginVertical(style);
			}

			body.Invoke();
			EditorGUILayout.EndVertical();
		}

		public static void DrawInProperty (Rect position, SerializedProperty property, GUIContent label, Action body)
		{
			EditorGUI.BeginProperty (position, label, property);
			{
				body.Invoke ();
			}
			EditorGUI.EndProperty ();
		}

		public static void DrawInNoIndent (Action body)
		{
			int originIndent = EditorGUI.indentLevel;

			EditorGUI.indentLevel = 0;
			{
				body.Invoke ();
			}
			EditorGUI.indentLevel = originIndent;
		}

		public static void DrawInIndent (Action body)
		{
			EditorGUI.indentLevel++;
			{
				body.Invoke ();
			}
			EditorGUI.indentLevel--;
		}

		public static Vector2 DrawInScrollView (Vector2 scorllPosition, float scrollBarHeight, Action body)
		{
			Vector2 newPosition;

			newPosition = EditorGUILayout.BeginScrollView (scorllPosition, new GUIStyle (), GUILayout.Height (scrollBarHeight));
			{
				body.Invoke ();
			}
			EditorGUILayout.EndScrollView ();

			return newPosition;
		}
	}

}