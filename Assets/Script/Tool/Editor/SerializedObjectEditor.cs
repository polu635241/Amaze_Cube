using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Kun.Tool
{
	public abstract class SerializedObjectEditor<T> : Editor where T:MonoBehaviour 
	{
		protected T runtimeScript;

		GUIStyle biggerFontSizeBtnGUIStyle;

		protected GUIStyle BiggerFontSizeBtnGUIStyle
		{
			get
			{
				if (biggerFontSizeBtnGUIStyle == null) 
				{
					biggerFontSizeBtnGUIStyle = new GUIStyle (EditorStyles.miniButton);
					biggerFontSizeBtnGUIStyle.fontSize = 15;
				}

				return biggerFontSizeBtnGUIStyle;
			}
		}

		protected GUIStyle buttonGUIStyle
		{
			get
			{
				return EditorStyles.miniButton;
			}
		}

		protected const float buttonHeight = 50f;

		protected GUIStyle fieldNameGUIStyle
		{
			get
			{
				return EditorStyles.miniLabel;
			}
		}

		GUILayoutOption[] _squareButtonLayoutOption;

		protected float squareButtonSize = 50;

		protected GUILayoutOption[] squareButtonLayoutOption
		{
			get
			{
				if (_squareButtonLayoutOption == null) 
				{
					_squareButtonLayoutOption = new GUILayoutOption[]
					{
						GUILayout.Width(squareButtonSize),
						GUILayout.Height(squareButtonSize)
					};
				}

				return _squareButtonLayoutOption;
			}
		}

		protected GUIStyle titleNameGUIStyle
		{
			get
			{
				return EditorStyles.label;
			}
		}

		protected GUIStyle boxSkin
		{
			get
			{
				return GUI.skin.box;
			}
		}

		protected const float fieldNameWidth = 70f;

		protected virtual void OnEnable()
		{
			runtimeScript = (T)target;
		}

		protected void DrawVariableField (string variableName, Action drawAndGetInput, float? overrideFieldWidth = null)
		{
			EditorTool.DrawInHorizontal (() => 
			{
				EditorGUILayout.LabelField (variableName, fieldNameGUIStyle, GUILayout.Width (fieldNameWidth));
				drawAndGetInput.Invoke ();
			});
		}
	}	
}