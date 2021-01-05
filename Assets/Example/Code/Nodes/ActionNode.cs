using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Editor setup
#if UNITY_EDITOR
using UnityEditor;

// ReSharper disable MemberCanBePrivate.Global
#endif

public class ActionNode : ExampleNode
{
	public string actionDescription;
	
#if UNITY_EDITOR
	public override void OnInspectorGUI()
	{
		actionDescription = EditorGUILayout.TextField("Action:", actionDescription);
	}
#endif
}
