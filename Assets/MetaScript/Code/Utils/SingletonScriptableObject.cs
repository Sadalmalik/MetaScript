using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Abstract class for making reload-proof singletons out of ScriptableObjects
/// Returns the asset created on the editor, or null if there is none
/// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
/// </summary>
/// <typeparam name="T">Singleton type</typeparam>
public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
	static T _instance;
	
	public static T Instance
	{
		get
		{
			if (!_instance)
			{
				#if UNITY_EDITOR
				var type = typeof(T);
				Debug.Log($"[TEST] Try Load Asset {type.Name}");
				var paths = AssetDatabase.GetAllAssetPaths();
				var path = paths.FirstOrDefault(p=>p.EndsWith(type.Name+".asset"));
				Debug.Log($"[TEST] Founded Asset {path}");
				_instance = (T) AssetDatabase.LoadAssetAtPath(path, type);
				Debug.Log($"[TEST] Asset instance {_instance}");
				#else
				_instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
				#endif
			}
			return _instance;
		}
	}
}