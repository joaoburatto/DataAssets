#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DataAssetsPackage.Editor
{
    /// <summary>
    /// Used to initialize, maintain and validate the data assets in the editor.
    /// </summary>
    [InitializeOnLoad]
    public static class DataAssetEditorHelper
    {
        static DataAssetEditorHelper()
        {
            EditorApplication.projectChanged += HandleProjectChanged;
        
            ValidateDataAssets();
        }

        private static void HandleProjectChanged()
        {
            ValidateDataAssets();
        }

        private static void ValidateDataAssets()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }

            DataAssetConfig dataAssetConfig = GetDataAssetConfig();

            ValidateDataAssetsFolder(dataAssetConfig.DataAssetsPath);
            ValidateDataAssets(GetAllDataAssetTypes(), dataAssetConfig);
        }

        private static void ValidateDataAssets(List<Type> dataAssetTypes, DataAssetConfig dataAssetConfig)
        {
            List<DataAsset> existingDataAssets = GetExistingDataAssets();
            Dictionary<Type, DataAsset> dataAssets = CreateDataAssetsDict(existingDataAssets);

            CreateMissingDataAssets(dataAssetTypes, dataAssets, dataAssetConfig);
            ValidatePreloadedAssets(dataAssets);
        }

        private static void ValidateDataAssetsFolder(string dataAssetsPath)
        {
            if (!AssetDatabase.IsValidFolder(dataAssetsPath))
            {
                // create the DataAssets folder
                AssetDatabase.CreateFolder("Assets", "DataAssets");
                Debug.Log("Created DataAssets folder.");
            }
        }

        private static Dictionary<Type, DataAsset> CreateDataAssetsDict(List<DataAsset> existingDataAssets)
        {
            // create a Dictionary of existing DataAssets and their types
            Dictionary<Type, DataAsset> dataAssetsDict = existingDataAssets
                .GroupBy(dataAsset => dataAsset.GetType())
                .ToDictionary(group => group.Key, group => group.First());
            return dataAssetsDict;
        }

        private static void ValidatePreloadedAssets(Dictionary<Type, DataAsset> dataAssetsDict)
        {
            Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();
            List<Object> newPreloadedAssets = preloadedAssets.ToList();
        
            newPreloadedAssets.RemoveAll(asset => asset == null);
        
            foreach (KeyValuePair<Type, DataAsset> typeDataAssetPair in dataAssetsDict)
            {
                Type dataAssetType = typeDataAssetPair.Key;
                DataAsset dataAsset = typeDataAssetPair.Value;

                // check if the DataAsset is in the preloaded assets
                if (!preloadedAssets.Contains(dataAsset))
                {
                    // add the DataAsset to the preloaded assets
                    newPreloadedAssets.Add(dataAsset);
                
                    Debug.Log($"Added {dataAssetType.Name} to preloaded assets.");
                }
            }
        
            PlayerSettings.SetPreloadedAssets(newPreloadedAssets.ToArray());
        }

        private static void CreateMissingDataAssets(List<Type> dataAssetTypes, Dictionary<Type, DataAsset> existingDataAssetsDict,
            DataAssetConfig dataAssetConfig)
        {
            bool createdNewDataAssets = false;
        
            // add the missing DataAssets types with a null value
            foreach (Type dataAssetInheritorType in dataAssetTypes)
            {
                if (!existingDataAssetsDict.ContainsKey(dataAssetInheritorType))
                {
                    // create the missing data asset on data asset path
                
                    string dataAssetPath = $"{dataAssetConfig.DataAssetsPath}/{dataAssetInheritorType.Name}.asset";
                    DataAsset dataAsset = ScriptableObject.CreateInstance(dataAssetInheritorType) as DataAsset;
                
                    existingDataAssetsDict.Add(dataAssetInheritorType, dataAsset);
                    AssetDatabase.CreateAsset(dataAsset, dataAssetPath);
                
                    createdNewDataAssets = true;
                }
            }
        
            if (createdNewDataAssets)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private static List<DataAsset> GetExistingDataAssets()
        {
            // get all existing DataAssets
            List<DataAsset> existingDataAssets = AssetDatabase.FindAssets("t:DataAsset")
                .Select(guid => AssetDatabase.LoadAssetAtPath<DataAsset>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToList();
        
            return existingDataAssets;
        }

        private static DataAssetConfig GetDataAssetConfig()
        {
            // get the DataAssetConfig scriptable object
            DataAssetConfig dataAssetConfig = AssetDatabase.FindAssets("t:DataAssetConfig")
                .Select(guid => AssetDatabase.LoadAssetAtPath<DataAssetConfig>(AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault();

            if (dataAssetConfig == null)
            {
                dataAssetConfig = ScriptableObject.CreateInstance(typeof(DataAssetConfig)) as DataAssetConfig;
                AssetDatabase.CreateAsset(dataAssetConfig!, "Assets/DataAssetsConfig.asset");
            }

            return dataAssetConfig;
        }

        private static List<Type> GetAllDataAssetTypes()
        {
            // get all classes that inherit from DataAsset
            Type dataAssetType = typeof(DataAsset);
            List<Type> dataAssetTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(dataAssetType) && !type.IsAbstract)
                .ToList();
        
            return dataAssetTypes;
        }
    }
}
#endif