using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MetaScrip
{
public class JsonMetaNodeConverter : Newtonsoft.Json.Converters.CustomCreationConverter<MetaNode>
{
	private static JsonMetaNodeConverter _instance;

	public static JsonMetaNodeConverter instance
	{
		get
		{
			if (_instance == null)
				_instance = new JsonMetaNodeConverter();
			return _instance;
		}
	}
	
	private Dictionary<string, Type> _types;
	private JsonMetaNodeConverter()
	{
		_types = new Dictionary<string, Type>();
		
		var subtypes = ReflectionUtils.GetAllSubtypes<MetaNode>();
		foreach (var type in subtypes)
			_types.Add(type.Name, type);
	}
	
	public void RefreshTypes()
	{
		_types.Clear();
		
		var subtypes = ReflectionUtils.GetAllSubtypes<MetaNode>();
		foreach (var type in subtypes)
			_types.Add(type.Name, type);
	}
	
	public Dictionary<string, Type> MetaTypes => _types;

	public override MetaNode Create(Type objectType)
	{
		throw new NotImplementedException();
	}

	public MetaNode Create(Type objectType, JObject jObject)
	{
		var type = (string) jObject.Property("__type");
		
		if(_types.TryGetValue(type, out var TargetType))
		{
			return (MetaNode)Activator.CreateInstance(TargetType);
		}

		throw new ApplicationException(String.Format("The given vehicle type {0} is not supported!", type));
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		JObject jObject = JObject.Load(reader);

		var target = Create(objectType, jObject);

		serializer.Populate(jObject.CreateReader(), target);

		return target;
	}
}
}
