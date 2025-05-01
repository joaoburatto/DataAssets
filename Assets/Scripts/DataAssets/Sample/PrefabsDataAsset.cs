#if ENABLE_DATA_ASSET_SAMPLE
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DataAssetsPackage.Sample
{
    public class PrefabsDataAsset : DataAsset
    {
        public GameObject MyPrefab;
        
        [FormerlySerializedAs("Prefabs")]
        public List<GameObject> EnemyPrefabs = new List<GameObject>
        {
            null,
            null,
            null
        };
        
        public Sprite[] EnemySprites = new Sprite[3];
    }
}
#endif