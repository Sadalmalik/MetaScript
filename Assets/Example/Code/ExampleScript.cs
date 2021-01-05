using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    public MetaContainer container;
    
    public void OnBeforeSerialize()
    {
        container.Save();
    }
    
    public void OnAfterDeserialize()
    {
        container.Load();
    }
}
