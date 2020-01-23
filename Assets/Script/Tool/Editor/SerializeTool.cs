using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kun.Tool
{
	[Serializable]
	public class SerializablePoint
	{
		public SerializableVector3 position = new SerializableVector3();
		public SerializableVector3 rotation = new SerializableVector3();

		public SerializablePoint(Transform convertTarget, bool local = false)
		{
			if (!local)
			{
				position = new SerializableVector3 (convertTarget.position);
				rotation = new SerializableVector3 (convertTarget.eulerAngles);
			}
			else
			{
				position = new SerializableVector3 (convertTarget.localPosition);
				rotation = new SerializableVector3 (convertTarget.localEulerAngles);
			}
		}

		public SerializablePoint(Point clonePoint)
		{
			position = new SerializableVector3(clonePoint.position.x, clonePoint.position.y, clonePoint.position.z);
			rotation = new SerializableVector3(clonePoint.rotation.x, clonePoint.rotation.y, clonePoint.rotation.z);
		}

		public SerializablePoint()
		{

		}

		public SerializablePoint GetClone()
		{
			SerializablePoint clonePoint = new SerializablePoint ();
			clonePoint.position = this.position.GetClone ();
			clonePoint.rotation = this.rotation.GetClone ();
			return clonePoint;
		}
	}

	public static class ConvertTool
	{
		public static SerializableVector3 ToSerializable(this Vector3 v3)
		{
			return new SerializableVector3 (v3);
		}

		public static SerializableQuaternion ToSerializable(this Quaternion q)
		{
			return new SerializableQuaternion (q);
		}
	}
	
    [Serializable]
    public class SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(Vector3 vector3)
        {
			this.x = vector3.x;
			this.y = vector3.y;
			this.z = vector3.z;
        }

		public SerializableVector3(float x,float y,float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static SerializableVector3 operator-(SerializableVector3 a,SerializableVector3 b)
		{
			return new SerializableVector3 (a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public static SerializableVector3 operator+(SerializableVector3 a,SerializableVector3 b)
		{
			return new SerializableVector3 (a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public SerializableVector3()
		{
			
		}

        public Vector3 GetValue()
        {
            return new Vector3(this.x, this.y, this.z);
        }

		public SerializableVector3 GetClone()
		{
			return new SerializableVector3 (this.x, this.y, this.z);
		}
    }

	[Serializable]
	public class Point
	{
		public Vector3 position = new Vector3 ();
		public Vector3 rotation = new Vector3 ();
	}

    [Serializable]
    public class SerializableQuaternion
    {
        public float x, y, z, w;

        public SerializableQuaternion(Quaternion quaternion)
        {
            this.x = quaternion.x;
            this.y = quaternion.y;
            this.z = quaternion.z;
            this.w = quaternion.w;
        }

        public Quaternion GetValue()
        {
            return new Quaternion(x, y, z, w);
        }
    }

	abstract class FileAdapter
	{
		public abstract string Serial (object obj);
		public abstract T Deserial<T> (string str);
		public abstract string SubFileName{get;}
	}

	class JsonAdapter:FileAdapter
	{
		public override string Serial (object obj)
		{
			return JsonUtility.ToJson (obj, true);
		}

		public override T Deserial<T> (string json)
		{
			return JsonUtility.FromJson<T> (json);
		}

		public override string SubFileName 
		{
			get 
			{
				return "json";
			}
		}
	}

	class XmlAdapter:FileAdapter
	{
		public override string Serial (object obj)
		{
			XmlSerializer ser = new XmlSerializer (obj.GetType ());
			StringBuilder sb = new StringBuilder ();
			StringWriter write = new StringWriter (sb);
			ser.Serialize (write, obj);
			return sb.ToString ();
		}

		public override T Deserial<T> (string xmlStr)
		{
			XmlDocument xdoc = new XmlDocument ();

			xdoc.LoadXml(xmlStr);
			XmlNodeReader reader = new XmlNodeReader(xdoc.DocumentElement);
			XmlSerializer ser = new XmlSerializer(typeof(T));
			object obj = ser.Deserialize(reader);
			return (T)obj;
		}

		public override string SubFileName 
		{
			get 
			{
				return "xml";
			}
		}
	}
}