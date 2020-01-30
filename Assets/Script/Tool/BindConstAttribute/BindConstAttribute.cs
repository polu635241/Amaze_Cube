using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kun.Tool
{
	public class BindConstAttribute :  PropertyAttribute{

		public string[] enumSlots;

		public bool showFieldName;

		public BindConstAttribute (string typeStr, bool showFieldName = true, bool autoSort = true)
		{
			Type type = Type.GetType (typeStr);

			this.enumSlots = GetStaticParFromType (type, autoSort).ToArray ();
			this.showFieldName = showFieldName;
		}
			
		public BindConstAttribute (List<string> typeStrs, bool showFieldName = true, bool autoSort = true)
		{
			List<string> keys = new List<string> ();

			typeStrs.ForEach ((typeStr)=>
				{
					Type type = Type.GetType(typeStr);
					keys.AddRange(GetStaticParFromType (type, autoSort));
				});

			this.enumSlots = keys.ToArray ();
			this.showFieldName = showFieldName;
		}

		public BindConstAttribute (List<Type> types, bool showFieldName = true, bool autoSort = true)
		{
			List<string> keys = new List<string> ();

			types.ForEach ((type)=>
				{
					keys.AddRange(GetStaticParFromType (type, autoSort));
				});

			this.enumSlots = keys.ToArray ();
			this.showFieldName = showFieldName;
		}

		public BindConstAttribute (Type type, bool showFieldName = true, bool autoSort = true)
		{
			this.enumSlots = GetStaticParFromType (type, autoSort).ToArray ();
			this.showFieldName = showFieldName;
		}

		List<string> GetStaticParFromType(Type type,bool auto)
		{
			List<string> pars = new List<string> ();

			FieldInfo[] fieldInfos = type.GetFields (BindingFlags.Public | BindingFlags.Static);

			for (int i = 0; i < fieldInfos.Length; i++) 
			{
				FieldInfo fieldInfo = fieldInfos [i];
				//只要不具有此標籤的
				BindConstIgnore bindConstIgnore = fieldInfo.GetCustomAttribute<Kun.Tool.BindConstIgnore> ();

				if (bindConstIgnore == null) 
				{
					pars.Add (fieldInfo.GetValue (null).ToString ());
				}
			}

			pars.Sort ((a, b) => a.CompareTo (b));

			return pars;
		}
	}
}
