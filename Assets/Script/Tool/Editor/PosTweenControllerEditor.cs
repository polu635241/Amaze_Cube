using UnityEngine;
using UnityEditor;

namespace Kun.Tool
{
    [CustomEditor (typeof (PosTweenController))]
    public class PosTweenControllerEditor : SerializedObjectEditor<PosTweenController>
    {
        float value;

        Vector3 oldBenginPos;
        Vector3 oldEndPos;

        protected override void OnEnable ()
        {
            base.OnEnable ();
            oldBenginPos = runtimeScript.BeginPos;
            oldEndPos = runtimeScript.EndPos;
        }

        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI ();

            bool currentFrameNeedRefresh = false;

            if (oldBenginPos != runtimeScript.BeginPos)
            {
                currentFrameNeedRefresh = true;
            }

            if (oldEndPos != runtimeScript.EndPos)
            {
                currentFrameNeedRefresh = true;
            }

            if (currentFrameNeedRefresh)
            {
                ForceRefresh ();
            }

            float newValue = EditorGUILayout.Slider (value, 0, 1);

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