#if ENABLE_DATA_ASSETS_SAMPLE
using UnityEngine;

namespace DataAssetsPackage.Sample.Behaviours
{
    public class Sample : MonoBehaviour
    {
        private void Awake()
        {
            SettingsDataAsset settingsDataAsset = DataAssets.Get<SettingsDataAsset>();
        }
    }
}
#endif