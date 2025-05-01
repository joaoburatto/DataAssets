using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataAssetsPackage
{
    /// <summary>
    /// Used to provide assets to the DataAssets on runtime.
    /// </summary>
    public static class DataAssets
    {
        /// <summary>
        /// Provides access to the data assets in the project based on their type, as they will always be a singleton.
        /// </summary>
        private static readonly Dictionary<Type, DataAsset> DataAssetsDictionary = new Dictionary<Type, DataAsset>();

        /// <summary>
        /// Adds a data asset to the dictionary.
        /// </summary>
        public static void Register<T>(T dataAsset) where T : DataAsset
        {
            // No need to check if the data asset is already registered, as it will be registered only once.
            DataAssetsDictionary.Add(typeof(T), dataAsset);
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
}