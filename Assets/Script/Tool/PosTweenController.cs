using UnityEngine;

namespace Kun.Tool
{
    public class PosTweenController : MonoBehaviour
    {
        [SerializeField]
        RectTransform valueTransform;

        public Transform BeginPosProxy
        {
            get 
            {
                return beginPosProxy;
            }
        }

        [SerializeField]
        Transform beginPosProxy;

        [SerializeField][ReadOnly]
        Vector3 beginPos = Vector3.zero;

        public Transform EndPosProxy
        {
            get 
            {
                return endPosProxy;
            }
        }

        [SerializeField]
        Transform endPosProxy;

        public Vector3 EndPos
        {
            get
            {
                return endPos;
            }
        }

        [SerializeField][ReadOnly]
        Vector3 endPos = Vector3.zero;


        [SerializeField][ReadOnly][Header ("把差距值分成100等分")]
        Vector3 proportionDeltaPos;

        public Camera RectCamera
        {
            set 
            {
                rectCamera = value;
            }
        }

        [SerializeField][Header("繪畫出的Camera overlay的話設定為null")]
        Camera rectCamera;

        [SerializeField]
        Vector3 fixPos;

        public float Value
        {
            get 
            {
                return value;
            }
        }

        float value;

        void Awake ()
        {
            Refresh ();
        }

        /// <summary>
        /// 滑鼠點下去的時候 會記下滑鼠與物件座標的delta當作補正值 保證點到物件的邊緣時 物件不會瞬間飛到滑鼠的點上
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="setFixPos"></param>
        /// <returns></returns>
        public bool CheckContatin (Vector3 screenPos ,out Vector3 rectPos)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle (valueTransform, screenPos, null, out rectPos))
            {
                if (RectTransformUtility.RectangleContainsScreenPoint (valueTransform, rectPos))
                {
                    return true;
                }
            }
            return false;
        }

        [SerializeField]
        bool debug;

        [SerializeField][ReadOnly]
        bool inDrag;

        void Update ()
        {
            if (!debug)
                return;

            Vector3 mousePos = Input.mousePosition;

            if (!inDrag)
            {
                value += Time.deltaTime * 0.1f;
                SetValue (value);

                if (Input.GetMouseButton (0))
                {
                    Vector3 rectPos;

                    if (CheckContatin (mousePos, out rectPos))
                    {
                        SetFixPos (rectPos);

                        inDrag = true;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButton (0))
                {
                    float progress;

                    AttachPoint (mousePos, out progress);
                }
                else
                {
                    inDrag = false;
                }
            }
        }

        public void SetFixPos (Vector3 rectPos)
        {
            fixPos = rectPos - valueTransform.position;
        }

        public void AttachPoint (Vector3 screenPos, out float progress)
        {
            Vector3 rectPos;

            Vector3 processScreenPos = screenPos - fixPos;

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle (valueTransform, processScreenPos, rectCamera, out rectPos))
            {
                // 拖動物件的時候 應該要用X來回推Y 縱向離開物件碰撞時還是可以拖動 橫向就看range (0,1)

                float unclampedProgress = (rectPos.x - beginPos.x) / (endPos.x - beginPos.x);

                progress = Mathf.Clamp (unclampedProgress, 0, 1);

                SetValue (progress);
            }
            else
            {
                Debug.LogError ("not in same rect");
                progress = -1;
            }
        }


        public void Refresh ()
        {
            proportionDeltaPos = (endPos - beginPos);

            beginPos = beginPosProxy.position;
            endPos = endPosProxy.position;

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
                valueTransform.position = progressPos;
            }

            this.value = value;
        }
    }
}