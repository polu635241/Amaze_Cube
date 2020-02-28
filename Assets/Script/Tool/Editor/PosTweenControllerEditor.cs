using UnityEngine;
using UnityEditor;

namespace Kun.Tool
{
    [CustomEditor (typeof (PosTweenController))]
    public class PosTweenControllerEditor : SerializedObjectEditor<PosTweenController>
    {
        Vector3 oldBenginPos;
        Vector3 oldEndPos;

        protected override void OnEnable ()
        {
            base.OnEnable ();
            
            if (NullProxyPointCheck)
            {
                return;
            }

            oldBenginPos = GetCurrentBeginPos;
            oldEndPos = GetCurrentEndPos;

			if (!Application.isPlaying) 
			{
				ForceRefresh ();
			}
        }

        Vector3 GetCurrentBeginPos
        {
            get 
            {
                return runtimeScript.BeginPosProxy.position;
            }
        }

        Vector3 GetCurrentEndPos
        {
            get
            {
                return runtimeScript.EndPosProxy.position;
            }
        }

        bool NullProxyPointCheck
        {
            get 
            {
                if (runtimeScript.BeginPosProxy == null || runtimeScript.EndPosProxy == null)
                {
                    return true;
                }

                return false;
            }
        }

        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI ();

            float value = runtimeScript.Value;

            float newValue = EditorGUILayout.Slider (value, 0, 1);

			if (NullProxyPointCheck || Application.isPlaying)
            {
                return;
            }

            bool currentFrameNeedRefresh = false;

            Vector3 currentBeginPos = GetCurrentBeginPos;
            if (oldBenginPos != currentBeginPos)
            {
                currentFrameNeedRefresh = true;
                oldBenginPos = currentBeginPos;
            }

            Vector3 currentEndPos = GetCurrentEndPos;
            if (oldEndPos != currentEndPos)
            {
                currentFrameNeedRefresh = true;
                oldEndPos = currentEndPos;
            }

            if (currentFrameNeedRefresh)
            {
                ForceRefresh ();
            }

			if (newValue != value)
            {
                value = newValue;
                runtimeScript.SetValue (value);
            }
        }

        void ForceRefresh ()
        {
            runtimeScript.Refresh ();
        }
    }
}