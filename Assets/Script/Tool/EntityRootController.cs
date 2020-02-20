using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kun.Tool
{
	public class EntityRootController<TEnum>
	{
		TEnum currentStatus;

		public virtual void Init ()
		{
			
		}

		public virtual void SwitchStatus (TEnum status)
		{
			currentStatus = status;
		}
	}
}
