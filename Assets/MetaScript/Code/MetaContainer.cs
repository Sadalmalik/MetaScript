using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global

namespace MetaScrip
{
	[Serializable]
	public class MetaContainer
	{
		[TextArea(5, 15)]
		public string container;

		[NonSerialized]
		private List<MetaNode> _nodes;

		public List<MetaNode> nodes
		{
			get
			{
				if (_nodes == null)
					Load();
				return _nodes;
			}
			set
			{
				_nodes = value;
				Save();
			}
		}

		public void Load()
		{
			if (string.IsNullOrEmpty(container))
			{
				_nodes = new List<MetaNode>();
			}
			else
			{
				_nodes = JsonConvert.DeserializeObject<List<MetaNode>>(
					container,
					JsonMetaNodeConverter.instance);
			}
		}

		public void Save()
		{
			container = JsonConvert.SerializeObject(_nodes);
		}

#if UNITY_EDITOR
		public void OnInspectorGUI()
		{
			MetaNode.DrawMetaList(nodes);
		}
#endif
	}
}