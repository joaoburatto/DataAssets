using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


/// <summary>
/// Used to initialize the data assets in runtime and provide access to them.
/// </summary>
public abstract partial class DataAssets
{
    private static readonly Dictionary<Type, DataAsset> DataAssetsDictionary = new Dictionary<Type, DataAsset>();
        
    /// <summary>
    /// Initializes the data assets in runtime to provide access to them.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void InitializeOnLoad()
    {
        Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();
            
        foreach (Object asset in preloadedAssets)
        {
            if (asset is not DataAsset dataAsset)
            {
                continue;
            }

            DataAssetsDictionary.Add(asset.GetType(), dataAsset);

            dataAsset.OnLoaded();
        }
    }

    /// <summary>
    /// Provides access to the data asset of type T.
    ///
    /// It is inferred that the data asset will never be null and will always be loaded, unless something weird happens.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Get<T>() where T : DataAsset
    {
        if (DataAssetsDictionary.TryGetValue(typeof(T), out DataAsset dataAsset))
        {
            return (T)dataAsset;
        }

        Debug.LogError($"Data asset of type {typeof(T)} not found.");
        return null;
    }
}