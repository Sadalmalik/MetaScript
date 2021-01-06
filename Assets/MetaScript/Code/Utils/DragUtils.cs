using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MetaScrip
{
public class CustomDragData<T>
{
	public List<T> list;
	public int index;

	public T item => list[index];
}

public static class DragUtils
{
	public static readonly Color hilightColor = new Color(0.7f, 0.8f, 1.0f, 0.5f);

	public static void HandleDragNDrop<T>(
		Rect rect, string identifier,
		Func<T> StartDrag,
		Action<T> HandleDrop,
		Func<T, bool> CheckDrop = null)
		where T : class
	{
		var currentEvent     = Event.current;
		var currentEventType = currentEvent.type;

		if (currentEventType == EventType.DragExited)
			DragAndDrop.PrepareStartDrag();

		if (!rect.Contains(currentEvent.mousePosition))
			return;

		T dragData;

		switch (currentEventType)
		{
			case EventType.MouseDown:
				DragAndDrop.PrepareStartDrag(); // reset data

				dragData = StartDrag();

				if (dragData == null)
					throw new NullReferenceException("Can't drug null object!");

				DragAndDrop.SetGenericData(identifier, dragData);

				currentEvent.Use();
				break;
			case EventType.MouseDrag:
				// If drag was started here:
				dragData = DragAndDrop.GetGenericData(identifier) as T;

				if (dragData != null)
				{
					DragAndDrop.StartDrag("Dragging List Element");
					currentEvent.Use();
				}

				break;
			case EventType.DragUpdated:
				dragData = DragAndDrop.GetGenericData(identifier) as T;

				DragAndDrop.visualMode =
					dragData != null ||
					CheckDrop != null &&
					CheckDrop(dragData)
						? DragAndDropVisualMode.Link
						: DragAndDropVisualMode.Rejected;

				currentEvent.Use();
				break;
			case EventType.Repaint:
				if (DragAndDrop.visualMode == DragAndDropVisualMode.None ||
				    DragAndDrop.visualMode == DragAndDropVisualMode.Rejected)
					break;

				EditorGUI.DrawRect(rect, hilightColor);
				break;
			case EventType.DragPerform:
				DragAndDrop.AcceptDrag();

				dragData = DragAndDrop.GetGenericData(identifier) as T;

				HandleDrop(dragData);

				currentEvent.Use();
				break;
			case EventType.MouseUp:
				// Clean up, in case MouseDrag never occurred:
				DragAndDrop.PrepareStartDrag();
				break;
		}
	}
	
	public static void DrawReorderableList<T>(List<T> list) where T : MetaNode
	{
		var identifier = $"DragNDrop_{typeof(T)}";
		
		EditorGUILayout.BeginVertical();
		
		HashSet<T> toRemove=new HashSet<T>();
		for (int i = 0; i < list.Count; i++)
		{
			var index = i;
			var item = list[i];
			
			var storedColor = GUI.backgroundColor;
			GUI.backgroundColor = item.BGColor();
			EditorGUILayout.BeginHorizontal(item.style);
			GUI.backgroundColor = storedColor;

			Rect rect = CustomGUI.DragHangler();
			HandleDragNDrop(rect, identifier,
				()=> new CustomDragData<T>
				{
					list  = list,
					index = index
				},
				data =>
				{
					if (list == data.list &&
						index == data.index)
						return;
					var temp = data.item;
					data.list.RemoveAt(data.index);
					list.Insert(index, temp);
					GUI.changed = true;
					GUI.FocusControl(null);
				}
			);
			
			EditorGUILayout.BeginVertical();
			item.OnInspectorGUI();
			EditorGUILayout.EndVertical();

			if(CustomGUI.DelHandler())
				toRemove.Add(item);
			
			EditorGUILayout.EndHorizontal();
		}
		
		list.RemoveAll(item => toRemove.Contains(item));

		EditorGUILayout.EndVertical();
	}
}

public class CustomGUI
{
	public static GUIStyle draggingHandle = (GUIStyle) "RL DragHandle";
	
	public static GUILayoutOption width = GUILayout.Width(15);
	public static GUILayoutOption expand = GUILayout.ExpandHeight(true);
	public static GUILayoutOption height = GUILayout.Height(15);
	
	public static void DrawDragHandle(Rect rect)
	{
		if (Event.current.type != EventType.Repaint)
			return;

		var offset = rect.height / 2;
		rect = new Rect(
			rect.x, rect.y + offset - 2,
			rect.width, rect.height - offset + 2
		);

		draggingHandle.Draw(rect, false, false, false, false);
	}
	
	public static Rect DragHangler()
	{
		Rect rect = EditorGUILayout.GetControlRect(width, expand);
		DrawDragHandle(rect);
		return rect;
	}
	
	public static bool DelHandler()
	{
		return GUILayout.Button("X", width, height);
	}
}
}
