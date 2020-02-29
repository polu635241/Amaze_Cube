using System;
using UnityEngine;

namespace Kun.Data
{
    public class CubeUnility
    {
        public static Quaternion GetDeltaQuaternion (RowRotateAxis axis, bool isPositive)
        {
            float scale = isPositive ? 1 : -1;

            float processDegree = 90 * scale;

            switch (axis)
            {
                case RowRotateAxis.X:
                    {
                        return Quaternion.Euler (processDegree, 0, 0);
                    }

                case RowRotateAxis.Y:
                    {
                        return Quaternion.Euler (0, processDegree, 0);
                    }

                case RowRotateAxis.Z:
                    {
                        return Quaternion.Euler (0, 0, processDegree);
                    }
            }

            throw new Exception ("無對應旋轉設定");
        }
    }
}