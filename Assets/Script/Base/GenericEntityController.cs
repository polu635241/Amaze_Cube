using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kun.Tool
{
	public class GenericEntityController : MonoBehaviour {

		protected virtual void Awake()
		{
			GetGenericComponents ();
		}

		GameObject m_go;

		public GameObject m_Go
		{
			get
			{
				return m_go;
			}
		}

		Transform m_transform;

		public Transform m_Transform
		{
			get 
			{ 
				return m_transform;
			}
		}

		Rigidbody m_rigidbody;

		public Rigidbody m_Rigidbody
		{
			get
			{
				return m_rigidbody;
			}
		}

		Collider m_collider;

		public Collider m_Collider
		{
			get
			{
				return m_collider;
			}
		}

		Animator m_anim;

		public Animator m_Anim
		{
			get
			{
				return m_anim;
			}
		}

		void GetGenericComponents()
		{
			m_go = this.gameObject;
			m_transform = m_go.transform;
			m_rigidbody = m_go.GetComponent<Rigidbody> ();
			m_collider = m_go.GetComponent<Collider> ();
			m_anim = m_go.GetComponent<Animator> ();
		}

		/// <summary>
		/// 傳入null代表使用原始值
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="z">The z coordinate.</param>
		public void SetPos (float? x, float? y, float? z = null)
		{
			Vector3 newPos = m_Transform.position;

			if (x != null) 
			{
				newPos.x = x.Value;
			}

			if (y != null) 
			{
				newPos.y = y.Value;
			}

			if (z != null) 
			{
				newPos.z = z.Value;
			}

			m_transform.position = newPos;
		}

		public void SetEntityLayer(string newLayerName)
		{
			m_go.layer = LayerMask.NameToLayer (newLayerName);
		}
	}
}
