using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MetaScrip
{
	public static class ReflectionUtils
	{
		public static List<Type> GetAllSubtypes<T>()
		{
			var targetType = typeof(T);
			var types      = new List<Type>();

			foreach (var domain_assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				var assembly_types = domain_assembly
				                    .GetTypes()
				                    .Where(type => type.IsSubclassOf(targetType) && !type.IsAbstract);
				types.AddRange(assembly_types);
			}

			var test = string.Join("\n\t", types.Select(t => t.Name));

			Debug.Log($"Types:\n\t{test}");

			return types;
		}
	}
}