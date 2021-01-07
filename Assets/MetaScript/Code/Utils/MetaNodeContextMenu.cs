using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MetaScrip
{
	public static class MetaNodeContextMenu
	{
		private static readonly List<string> _tempPath = new List<string>();

		public static void ShowContextMenu<T>(Action<Type> selectCallback) where T : MetaNode
		{
			JsonMetaNodeConverter.instance.RefreshTypes();

			var baseType = typeof(T);
			var types    = JsonMetaNodeConverter.instance.MetaTypes;

			GenericMenu menu = new GenericMenu();

			foreach (var pair in types)
			{
				var meta = pair.Value;
				if (meta == baseType)
					continue;

				_tempPath.Clear();
				while (meta != baseType)
				{
					_tempPath.Add(meta.Name);
					meta = meta.BaseType;
				}

				_tempPath.Reverse();

				var path = string.Join("/", _tempPath);
				Debug.Log($"Add menuitem {path}");

				menu.AddItem(
					new GUIContent(path),
					false,
					t => selectCallback?.Invoke((Type) t),
					pair.Value);
			}

			menu.ShowAsContext();
		}
	}
}