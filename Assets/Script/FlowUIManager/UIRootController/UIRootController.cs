using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Data;

namespace Kun.Tool
{
	public class UIRootController : EntityRootController<GameFlowUIStatus> 
	{
		protected GameObject bindGO;
		
		public UIRootController (GameObject bindGO)
		{
			this.bindGO = bindGO;
		}
	}

}