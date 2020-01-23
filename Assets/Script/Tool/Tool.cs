using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kun.Tool
{
	public static class Tool
	{

		public static void SwitchItem<T> (List<T> list1, List<T> list2)
		{
			List<T> _list1 = new List<T> (list2);
			List<T> _list2 = new List<T> (list1);

			list1.Clear ();
			_list1.ForEach (item => list1.Add (item));

			list2.Clear ();
			_list2.ForEach (item => list2.Add (item));
		}
		/// <summary>
		/// 產生 帶顏色的字串
		/// </summary>
		/// <returns>The string color.</returns>
		/// <param name="inputStr">Input string.</param>
		/// <param name="inputColor">Input color.</param>
		public static string MixStringColor(string inputStr, Color inputColor)
		{
			string left = ColorUtility.ToHtmlStringRGB (inputColor).ToLower();

			return string.Format ("<color=#{0}>{1}</color>",left,inputStr);
		}

		static bool CheckIsSampleType<T>()
		{
			bool result = false;

			result = typeof(T).IsPrimitive || typeof(String).IsAssignableFrom (typeof(T));

			return result;
		}

		public static T DeepClone<T>(T source)
		{
			MemoryStream memoryStream = new MemoryStream ();
			BinaryFormatter binaryFormatter = new BinaryFormatter ();

			binaryFormatter.Serialize (memoryStream, source);
			memoryStream.Flush ();
			memoryStream.Position = 0;
			T result = (T)binaryFormatter.Deserialize (memoryStream);
			memoryStream.Dispose ();
			return result;
		}

		public static List<T> ToList<T>(this T[] srcArray)
		{
			List<T> result = new List<T> (srcArray);

			return result;
		}

		public static void Map<T> (this List<T> target, Action<int,T> mapCallback)
		{
			for (int i = 0; i < target.Count; i++) 
			{
				mapCallback (i, target [i]);
			}
		}

		public static void SetFieldValueByReflection (this object target, string fieldName, object value)
		{
			BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
			Type type = target.GetType ();
			FieldInfo fieldInfo = type.GetField (fieldName, flag);
			fieldInfo.SetValue (target, value);
		}

		public static void SetPropertyValueByReflection (this object target, string propertyName, object value)
		{
			BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
			Type type = target.GetType ();
			PropertyInfo field = type.GetProperty (propertyName, flag);
			field.SetValue (target, value, null);
		}

		public static void ProcessAddLayerMask(ref LayerMask layerMask, string modifyLayerName)
		{
			ProcessLayerMask (ref layerMask, modifyLayerName, true);
		}

		public static void ProcessRemoveLayerMask(ref LayerMask layerMask, string modifyLayerName)
		{
			ProcessLayerMask (ref layerMask, modifyLayerName, false);
		}

		public static void ProcessAddLayerMask(Camera camera, string modifyLayerName)
		{
			ProcessLayerMask (camera, modifyLayerName, true);
		}

		public static void ProcessRemoveLayerMask(Camera camera, string modifyLayerName)
		{
			ProcessLayerMask (camera, modifyLayerName, false);
		}

		static void ProcessLayerMask(ref LayerMask layerMask, string modifyLayerName, bool isAdd)
		{
			int layerMaskValue = layerMask.value;

			int modifyLayer = LayerMask.NameToLayer (modifyLayerName);

			if (modifyLayer == -1) 
			{
				//everyThing的layer是-1 同時 NameToLayer 找不到東西也會回傳-1
				UnityEngine.Debug.LogError ("this layer not exist -> " + modifyLayerName);
			}
			else
			{
				int modifyLayerValue = 1 << modifyLayer;

				//確定指定的layer有沒有被包含在layerMask裡
				bool isContain = (layerMaskValue & modifyLayerValue) == modifyLayerValue;

				// 新增 且 不包含			//移除 且 包含	
				if ((isAdd && !isContain)||(!isAdd&&isContain))
				{
					//反轉指定開關
					layerMaskValue = layerMaskValue ^ modifyLayerValue;
				}
			}

			layerMask.value = layerMaskValue;
		}

		static void ProcessLayerMask(Camera camera, string modifyLayerName, bool isAdd)
		{
			int layerMaskValue = camera.cullingMask;

			int modifyLayer = LayerMask.NameToLayer (modifyLayerName);

			if (modifyLayer == -1) 
			{
				//everyThing的layer是-1 同時 NameToLayer 找不到東西也會回傳-1
				UnityEngine.Debug.LogError ("this layer not exist -> " + modifyLayerName);
			}
			else
			{
				int modifyLayerValue = 1 << modifyLayer;

				//確定指定的layer有沒有被包含在layerMask裡
				bool isContain = (layerMaskValue & modifyLayerValue) == modifyLayerValue;

				// 新增 且 不包含			//移除 且 包含	
				if ((isAdd && !isContain)||(!isAdd&&isContain))
				{
					//反轉指定開關
					layerMaskValue = layerMaskValue ^ modifyLayerValue;
				}
			}

			camera.cullingMask = layerMaskValue;
		}

		/// <summary>
		/// 抓取最後一個物件
		/// </summary>
		/// <returns>The last.</returns>
		/// <param name="iList">I list.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T GetLast<T>(this IList<T> iList)
		{	
			int count = iList.Count;

			if (count == 0) 
			{
				throw new Exception ("長度為0 無法回傳");
			}
			else
			{
				return iList [count - 1];
			}
		}

		public static void ForEach<TKey,TValue>(this Dictionary<TKey,TValue> dict,Action<TValue> callback)
		{
			foreach (var item in dict) 
			{
				callback.Invoke (item.Value);
			}
		}

		public static void ForEach<TKey,TValue>(this Dictionary<TKey,TValue> dict,Action<KeyValuePair<TKey,TValue>> callback)
		{
			foreach (var item in dict) 
			{
				callback.Invoke (item);
			}
		}

		public static Vector3 Vector3Multiply(Vector3 a, Vector3 b)
		{
			return new Vector3 (a.x * b.x, a.y * b.y, a.z * b.z);
		}

		//傳入V3 只取x y來用
		public static Vector2 Vector2Multiply(Vector3 a, Vector3 b)
		{
			return new Vector2 (a.x * b.x, a.y * b.y);
		}

		public static Vector2 Vector2Multiply(Vector2 a, Vector2 b)
		{
			return new Vector2 (a.x * b.x, a.y * b.y);
		}
	}

	[Serializable]
	public class Range : IEnumerable<int>
	{
		List<int> values;

		public List<int> Values
		{
			get
			{
				return values;
			}
		}

		IEnumerator<int> IEnumerable<int>.GetEnumerator()
		{
			return values.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return values.GetEnumerator ();
		}

		public Range(int endPoint,int beginPoint=0)
		{
			int length = endPoint - beginPoint;

			values = new List<int>();

			for (int i = beginPoint; i < endPoint; i++) 
			{
				values.Add(i);
			}
		}

		public void ForEach(Action<int> trigger)
		{
			values.ForEach (trigger);
		}

		public int Count
		{
			get
			{
				return values.Count;
			}
		}

		public int this[int key]
		{
			get
			{
				return values [key];
			}
		}
	}

	[Serializable]
	public class RefKeyValuePair<TKey,TValue>
	{
		public TKey Key
		{
			get
			{
				return _key;
			}

			set
			{
				_key = value;
			}
		}

		[SerializeField][ReadOnly]
		TKey _key;

		public TValue Value
		{
			get
			{
				return _value;
			}

			set
			{
				_value = value;
			}
		}

		[SerializeField][ReadOnly]
		TValue _value;

		public RefKeyValuePair(TKey key,TValue value)
		{
			this._key = key;
			this._value = value;
		}

		public RefKeyValuePair()
		{

		}
	}
}
