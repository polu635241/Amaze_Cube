using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

namespace Kun.Data
{
	public static class AssetKeys 
	{
		public const string MainCamera = "MainCamera";
		public const string Cube = "Cube";
		public const string CenterPoint = "CenterPoint";

		[BindConstIgnore]
		public static string RootFormat = "Root_{0}";

		public const string Root_1 = "Root_1";
		public const string Root_2 = "Root_2";
		public const string Root_3 = "Root_3";
		public const string Root_4 = "Root_4";
		public const string Root_5 = "Root_5";
		public const string Root_6 = "Root_6";

        public const string UIRoot = "UIRoot";
        public const string ResetBtn = "ResetBtn";
        public const string GameStartBtn = "GameStartBtn";
		public const string ApplicationQuitBtn = "ApplicationQuitBtn";
        public const string TimeText = "TimeText";

    }
}
