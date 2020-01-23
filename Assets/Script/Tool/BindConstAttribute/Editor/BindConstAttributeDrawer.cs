using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Kun.Tool
{
	[CustomPropertyDrawer(typeof(BindConstAttribute))]
	public class BindConstAttributeDrawer :  PropertyDrawer{

		float GetAbs(string fieldName)
		{
			int fieldNameLength = fieldName.Length;

			//先看有幾個字
			float length = fieldNameLength*8f;

			//再加上一個固定長度
			length += 50;

			return length;
		}

		const float lineHeight = 16f;

		const float lineSpaceHeight = 4f;

		Vector2 fieldSize = new Vector2 (200, lineHeight);

		BindConstAttribute myAttribute
		{
			get
			{
				return (BindConstAttribute)attribute;
			}
		}


		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			//搜尋框的下移
			return base.GetPropertyHeight (property, label) + lineHeight + lineSpaceHeight;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			EditorTool.DrawInProperty (position, property, label,()=>
				{
					EditorTool.DrawInNoIndent(()=>
						{
							string fieldName = property.name;

							// Struct 可以直接給值 不用管refence
							Rect fieldNameRect;
							Rect filedValueRect;

							fieldNameRect = filedValueRect = position;

							float fieldNameUseX = GetAbs (fieldName);

							//只偏移X 沒畫變數名稱的話 不用做偏移
							if(myAttribute.showFieldName)
							{
								filedValueRect.center += new Vector2 (fieldNameUseX, 0);
							}

							filedValueRect.size = fieldSize;

							Event e = Event.current;

							if (e.type == EventType.MouseDown &&e.button == 0 && filedValueRect.Contains(e.mousePosition))
							{
								Pop_UpSelectWindow.ShowWindow(property,myAttribute.enumSlots);
							}

							if(myAttribute.showFieldName)
							{
								EditorGUI.LabelField (fieldNameRect, fieldName);
							}
							//假的輸入框 其實是靠彈出式畫面
							EditorGUI.TextField(filedValueRect,"");
							EditorGUI.LabelField(filedValueRect, property.stringValue);

						});
				});
		}
	}
}
