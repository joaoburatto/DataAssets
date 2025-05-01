#if UNITY_EDITOR
using UnityEngine;

namespace DataAssetsPackage
{ 
    // You can move the asset of this class to another place if you want.
    
    /// <summary>
    /// Configures the data assets in the project.
    /// </summary>
    [CreateAssetMenu(fileName = "DataAssetConfig", menuName = "DataAssets/DataAssetConfig", order = 1)]
    public class DataAssetConfig : ScriptableObject
    {
        /// <summary>
        /// Path where created DataAssets will be placed when automatically created by DataAssetEditorHelper.
        /// </summary>
        public string DataAssetsPath = "Assets/DataAssets";
    }
}
#endif