using System.Collections.Generic;
using UnityEngine;

// Editor setup
#if UNITY_EDITOR
using UnityEditor;

// ReSharper disable MemberCanBePrivate.Global
#endif

public class IfNode : ExampleNode
{
	public override Color BGColor() => new Color(0.6f,0.4f,0.1f,1f);
	
	public string condition;
	public List<MetaNode> nodes = new List<MetaNode>();

#if UNITY_EDITOR
	public override void OnInspectorGUI()
	{
		condition = EditorGUILayout.TextField("Condition:", condition);
		EditorGUILayout.LabelField("Subnodes:");
		MetaNode.DrawMetaList(nodes);
	}
#endif
}