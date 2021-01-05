using System;
using UnityEngine;

// Editor setup
#if UNITY_EDITOR
using UnityEditor;

// ReSharper disable MemberCanBePrivate.Global
#endif

[Serializable]
public class CommentNode : ExampleNode
{
	public override Color BGColor() => new Color(0.2f,0.3f,0.5f,1f);
	
	public string note;

#if UNITY_EDITOR
	public override void OnInspectorGUI()
	{
		note = EditorGUILayout.TextArea(note);
	}
#endif
}