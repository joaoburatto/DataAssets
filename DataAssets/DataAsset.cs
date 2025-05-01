using UnityEngine;

namespace DataAssetsPackage
{
    /// <summary>
    /// "A data singleton", used to store data in a scriptable object, but easily accessible.
    /// </summary>
    ///

    /// When you create a class that inherits from this class, the DataAssetEditorHelper class will create the Scriptable Object asset instance of your
    /// class and put it under the folder that is specified on DataAssetConfig and add it to the preloaded assets list.
    ///
    /// Then you can simply use DataAssets.Get<TDataAsset>() to get the instance of your class.
    /// Like this:
    ///     DataAssets.Get<YourDataAssetClass>(); or DataAssets.Get<ScenesDataAssets>().MyScene;
    ///
    /// Good practices:
    ///     1. Don't add MEMORY HEAVY assets in DataAssets.
    ///         1.1 Gigantic lists/dictionaries or big strings might impact startup time.
    ///         1.2 If you want to add GameObjects or assets that are heavy, use Addressables via AssetReference a load them as needed.
    ///             Actually I'm not sure if heavy assets that are Unity assets are an issue, they might not be loaded with the DataAsset object.
    ///
    ///     2. Use these as a "global" READ ONLY data container, a way to store data that is not tied to a specific object and is more broad in uses.
    ///         2.1. For example, you can use this to store the project scene references, like an Addressable Scene and have only minimal memory usage for initialization.
    ///         2.2. You can use this to store the game settings, like the default game volume settings, default language.
    ///
    ///     3. Use this as a READ ONLY data provider, I don't recommend changing the data in runtime.
    ///         3.1 This is a recommendation for ScriptableObjects in general, but in this case,
    ///             it is even more important as these are loaded at startup and are never loaded again.
    ///
    ///     4. Don't call stuff from a DataAsset on another DataAsset.
    ///         4.1 I doubt this would ever happen, but if you do this, you might end up with a circular dependency and race conditions.
    public class DataAsset : ScriptableObject
    {
        /// <summary>
        /// Called by DataAssets when the data asset is loaded.
        /// </summary>
        public virtual void OnLoaded()
        {
            // You can comment this out, but it's nice to see which ones were loaded.
            Debug.Log($"[{nameof(DataAsset)}] Loaded DataAsset: {GetType().Name}");
        }
    }
}