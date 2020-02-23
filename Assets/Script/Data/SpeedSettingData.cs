using System;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
    [Serializable]
    public class SpeedSettingData
    {
        public List<float> Speeds
        {
            get
            {
                return speeds;
            }
        }

        [SerializeField][ReadOnly]
        List<float> speeds;

        public int DefaultIndex
        {
            get
            {
                return defaultIndex;
            }
        }

        [SerializeField][ReadOnly]
        int defaultIndex;

        public float GetValue (int index)
        {
            return Speeds[index];
        }
    }
}