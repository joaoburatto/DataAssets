using UnityEngine;

namespace DefaultNamespace
{ 
    // You can move this the asset of this class to another place if you want
    
    /// <summary>
    /// Configures the data assets in the project.
    /// </summary>
    [CreateAssetMenu(fileName = "DataAssetConfig", menuName = "DataAssets/DataAssetConfig", order = 1)]
    public class DataAssetConfig : ScriptableObject
    {
        public string DataAssetsPath = "Assets/DataAssets";
        
        // public bool GenerateDataAssetsCode = true;
        // public string GeneratedDataAssetsCodePath = "Assets/Scripts/DataAssets/DataAsset.Generated.cs";
    }
}