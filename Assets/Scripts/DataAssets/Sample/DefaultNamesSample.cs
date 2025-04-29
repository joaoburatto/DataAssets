#if ENABLE_DATA_ASSET_SAMPLE
using System;
using UnityEngine;

namespace DefaultNamespace.Sample
{
    public class DefaultNamesSample : MonoBehaviour
    {
        private void Awake()
        {
            DefaultNamesDataAsset defaultNamesDataAsset = DataAssets.Get<DefaultNamesDataAsset>();
            
            if (defaultNamesDataAsset != null)
            {
                foreach (var name in defaultNamesDataAsset.Names)
                {
                    Debug.Log(name);
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