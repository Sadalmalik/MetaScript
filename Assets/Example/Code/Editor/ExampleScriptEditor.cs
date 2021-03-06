﻿using UnityEditor;

[CustomEditor(typeof(ExampleScript))]
public class ExampleScriptEditor : Editor
{
	private ExampleScript _script;

	void OnEnable()
	{
		_script = target as ExampleScript;
		_script.container.Load();
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.LabelField("Nodes:");

		_script.container.OnInspectorGUI();

		if (EditorGUI.EndChangeCheck())
		{
			_script.container.Save();
			EditorUtility.SetDirty(_script);
		}
	}
}