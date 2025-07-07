using UnityEditor;
using UnityEngine;

namespace DataAssetsPackage
{
    /// <summary>
    /// Used to initialize the DataAssets in runtime.
    /// </summary>
    public static class DataAssetsLoader
    {
        /// <summary>
        /// This method is called when the game starts, after all assemblies are loaded. It is called by Unity automatically.
        ///
        /// We can use this method to initialize the data assets and load them into memory using the already existing Preloaded Assets system
        /// which ensures that the DataAssets are already loaded in memory as they are loaded in the AfterAssembliesLoaded phase.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void InitializeOnLoad()
        {
            Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();
            
            foreach (Object asset in preloadedAssets)
            {
                // Type check as you can add any type of Unity asset to the preloaded assets.
                if (asset is not DataAsset dataAsset)
                {
                    continue;
                }

                DataAssets.Register(dataAsset);
                dataAsset.OnLoaded();
            }
        }
    }
}