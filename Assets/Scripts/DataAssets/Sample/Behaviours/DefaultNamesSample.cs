#if ENABLE_DATA_ASSET_SAMPLE
using UnityEngine;

namespace DataAssetsPackage.Sample.Behaviours
{
    public class DefaultNamesSample : MonoBehaviour
    {
        private void Awake()
        {
            // You can see that in the first scene, in the first frame, the data asset is already loaded.
            LogDefaultNames();
        }

        private static void LogDefaultNames()
        {
            DefaultNamesDataAsset defaultNamesDataAsset = DataAssets.Get<DefaultNamesDataAsset>();

            if (defaultNamesDataAsset != null)
            {
                foreach (string defaultName in defaultNamesDataAsset.Names)
                {
                    Debug.Log(defaultName);
                }
            }
            else
            {
                Debug.LogError("DefaultNamesDataAsset is null");
            }
        }
    }
}
#endif