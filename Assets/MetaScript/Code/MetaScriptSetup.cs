﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MetaScrip
{
	[CreateAssetMenu(menuName = "Meta Script/Setup", fileName = "MetaScriptSetup")]
	public class MetaScriptSetup : SingletonScriptableObject<MetaScriptSetup>
	{
		public GUIStyle defaultBox;
	}
}