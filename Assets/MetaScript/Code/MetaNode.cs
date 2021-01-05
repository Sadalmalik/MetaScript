using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

// Editor setup
#if UNITY_EDITOR
using UnityEditor;

// ReSharper disable MemberCanBePrivate.Global
#endif

[Serializable]
public class MetaNode
{
	[JsonProperty("__type")] public string ObjectType => GetType().Name;

	[JsonIgnore] public GUIStyle style => MetaScriptSetup.Instance.defaultBox;
	public virtual Color BGColor() => new Color(0.2f,0.2f,0.2f,1f);

#if UNITY_EDITOR
	public virtual void OnInspectorGUI()
	{
		GUILayout.Label($"Inspector not implemented in type {GetType()}!");
	}
	
	public static void DrawMetaList<T>(List<T> nodes) where T : MetaNode
	{
		DragUtils.DrawReorderableList(nodes);
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Add"))
		{
			void AddItem(Type type)
			{
				var node = (T)Activator.CreateInstance(type);
				nodes.Add(node);
				GUI.changed = true;
			}
			
			MetaNodeContextMenu.ShowContextMenu<MetaNode>(AddItem);
		}
		EditorGUILayout.EndHorizontal();
	}
#endif
}
