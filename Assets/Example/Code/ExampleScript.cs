using UnityEngine;
using MetaScrip;

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
