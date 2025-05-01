#if ENABLE_DATA_ASSET_SAMPLE
namespace DataAssetsPackage.Sample
{
    /// <summary>
    /// I would have used the SceneReference class from the Unity package, but it would create a dependency for the sample.
    /// </summary>
    public class ScenesDataAsset : DataAsset
    {
        public string MainSceneName;

        public string GameSceneName;
    }
}
#endif