using UnityEngine;
 
/// <summary>
/// "A data singleton", used to store data in a scriptable object, but easily accessible.
/// </summary>
public class DataAsset : ScriptableObject
{
    public virtual void OnLoaded()
    {
        Debug.Log($"[{nameof(DataAssets)}] Loaded DataAsset: {GetType().Name}");
    }
}