using UnityEngine;

namespace Kun.Tool
{
    public class PosTweenController : MonoBehaviour
    {
        [SerializeField]
        Transform valueTransform;

        public Vector3 BeginPos
        {
            get 
            {
                return beginPos;
            }
        }

        [SerializeField]
        Vector3 beginPos = Vector3.zero;

        public Vector3 EndPos
        {
            get 
            {
                return endPos;
            }
        }

        [SerializeField]
        Vector3 endPos = Vector3.zero;

        [SerializeField][ReadOnly][Header ("把差距值分成100等分")]
        Vector3 proportionDeltaPos;

        void Awake ()
        {
            Refresh ();
        }

        public void Refresh ()
        {
            proportionDeltaPos = (endPos - beginPos);

            SetValue (0f);
        }

        public void SetValue (float value)
        {
            if (value < 0)
            {
                value = 0;
            }

            if (value > 1)
            {
                value = 1;
            }

            Vector3 progressPos = beginPos + value * proportionDeltaPos;

            if (valueTransform != null)
            {
                valueTransform.localPosition = progressPos;
            }
        }
    }
}