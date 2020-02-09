using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Kun.Tool
{
	public class SerializedObjectEditor_FullOverride<T> : SerializedObjectEditor<T>  where T : UnityEngine.Object
	{
		const float beginIntervalSpace = 10f;
		const float scriptFieldKeyWidth = 50f;
		protected const float classIntervalSpace = 5f;

		public override void OnInspectorGUI ()
		{
			GUILayout.Space (beginIntervalSpace);

			DrawScriptField ();

			GUILayout.Space (classIntervalSpace);
		}

		void DrawScriptField()
		{
			DrawVariableField ("Script : ", () => 
			{		
				MonoScript script = null;

				if(target is MonoBehaviour)
				{
					MonoBehaviour monoScript = runtimeScript as MonoBehaviour;
					script = MonoScript.FromMonoBehaviour (monoScript);
				}

				if(target is ScriptableObject)
				{
					ScriptableObject scriptableObjectScript = runtimeScript as ScriptableObject;
					script = MonoScript.FromScriptableObject (scriptableObjectScript);
				}

				if(script !=null)
				{
					EditorTool.DrawInReadOnly(()=>
					{
						EditorGUILayout.ObjectField (script, typeof(MonoScript), false);
					});
				}

			}, scriptFieldKeyWidth);
			GUI.enabled = true;
		}
	}	
}