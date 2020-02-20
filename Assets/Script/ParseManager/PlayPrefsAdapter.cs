using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;
using Kun.Data;

namespace Kun.Tool
{
    [Serializable]
    public class PlayPrefsAdapter
    {
        const string BindGroupKey = "Amaze_Cube";

        public int SpeedScale
        {
            get 
            {
                return speedScalePlayPrefsAdapter.GetValue ();
            }

            set 
            {
                speedScalePlayPrefsAdapter.SetValue (value);
            }
        }

        private PlayPrefsIntAdapter speedScalePlayPrefsAdapter;


        private class PlayPrefsIntAdapter
        {
            public PlayPrefsIntAdapter (string bindGroupKey, string bindKey, int defaultValue = 0)
            {
                bindProcessKey = $"{bindGroupKey}_{bindKey}";

                value = PlayerPrefs.GetInt (bindProcessKey, defaultValue);
            }

            string bindProcessKey;

            int value;

            public int GetValue ()
            {
                return value;
            }

            public void SetValue (int value)
            {
                this.value = value;
                PlayerPrefs.SetInt (bindProcessKey, value);
            }
        }

        private class PlayPrefsFloatAdapter
        {
            public PlayPrefsFloatAdapter (string bindGroupKey, string bindKey, float defaultValue = 0f)
            {
                bindProcessKey = $"{bindGroupKey}_{bindKey}";
                value = PlayerPrefs.GetFloat (bindProcessKey, defaultValue);
            }

            string bindProcessKey;

            float value;

            public float GetValue ()
            {
                return value;
            }

            public void SetValue (float value)
            {
                this.value = value;
                PlayerPrefs.SetFloat (bindProcessKey, value);
            }
        }


        private class PlayPrefsStringAdapter
        {
            public PlayPrefsStringAdapter (string bindGroupKey, string bindKey, string defaultValue = "")
            {
                bindProcessKey = $"{bindGroupKey}_{bindKey}";

                value = PlayerPrefs.GetString (bindProcessKey, defaultValue);
            }

            string bindProcessKey;

            string value;

            public string GetValue ()
            {
                return value;
            }

            public void SetValue (string value)
            {
                this.value = value;
                PlayerPrefs.SetString (bindProcessKey, value);
            }
        }

        private class PlayPrefsBoolAdapter
        {
            public PlayPrefsBoolAdapter (string bindGroupKey, string bindKey, bool defaultValue = false)
            {
                bindProcessKey = $"{bindGroupKey}_{bindKey}";

                int processDefaultValue = GetProcessValue (defaultValue);

                value = PlayerPrefs.GetInt (bindProcessKey, processDefaultValue);
            }

            string bindProcessKey;

            int value;

            public bool GetValue ()
            {
                return GetProcessValue (value);
            }

            public void SetValue (bool value)
            {
                int processValue = GetProcessValue (value);

                this.value = processValue;
                PlayerPrefs.SetInt (bindProcessKey, processValue);
            }

            bool GetProcessValue (int value)
            {
                if (value == 1)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }

            int GetProcessValue (bool value)
            {
                if (value == true)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}
