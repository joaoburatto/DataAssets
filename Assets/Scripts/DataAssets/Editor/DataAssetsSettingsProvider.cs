#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DataAssetsPackage.Editor
{
    /// <summary>
    /// Used to display the data assets in the DataAssets in the Project Settings window.
    /// </summary>
    public class DataAssetsSettingsProvider : SettingsProvider
    {
        private readonly Dictionary<DataAsset, bool> _dataAssetsFoldouts = new Dictionary<DataAsset, bool>();

        private int _currentPage = 0;
        private int _totalPages = 0;
        
        private List<DataAsset> _dataAssets = new List<DataAsset>();

        private const int PageDisplayCount = 12;
        
        private DataAssetsSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            LoadDataAssets();
        }

        private void LoadDataAssets()
        {
            string[] guids = AssetDatabase.FindAssets("t:DataAsset");
            
            _dataAssets = guids
                .Select(guid => AssetDatabase.LoadAssetAtPath<DataAsset>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(asset => asset != null)
                .ToList();
        }

        public override void OnGUI(string searchContext)
        {
            if (_dataAssets == null)
            {
                LoadDataAssets();
            }

            DataAssetConfig dataAssetConfig = AssetDatabase.FindAssets("t:DataAssetConfig")
                .Select(guid => AssetDatabase.LoadAssetAtPath<DataAssetConfig>(AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault();
            
            if (dataAssetConfig == null)
            {
                EditorGUILayout.HelpBox("DataAssetConfig not found. Please create one.", MessageType.Error);
                return;
            }
            
            EditorGUILayout.Space();
            
            int elementsCount = DrawPageSelection(out int startIndex);

            if (elementsCount == 0)
            {
                EditorGUILayout.HelpBox("No DataAssets found.", MessageType.Info);
                return;
            }

            for (int i = startIndex; i < startIndex + PageDisplayCount && i < elementsCount; i++)
            {
                DataAsset asset = _dataAssets[i];
                DrawDataAssetFoldout(asset);
            }
        }

        private int DrawPageSelection(out int startIndex)
        {
            int elementsCount = _dataAssets.Count;
            
            _totalPages = (int)Math.Ceiling((double)elementsCount / PageDisplayCount);
            
            _currentPage = Mathf.Clamp(_currentPage, 0, _totalPages - 1);
            
            if (_totalPages == 0)
            {
                startIndex = 0;
                return 0;
            }
            
            startIndex = _currentPage * PageDisplayCount;
            
            if (elementsCount > PageDisplayCount)
            {
                EditorGUILayout.BeginHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Page Number");
                string pageNumber = EditorGUILayout.TextField((_currentPage + 1).ToString());
                if (int.TryParse(pageNumber, out int page))
                {
                    _currentPage = Mathf.Clamp(page - 1, 0, _totalPages - 1);
                }
                
                EditorGUILayout.EndHorizontal();
                
                if (GUILayout.Button("Previous"))
                {
                    _currentPage--;
                }
                
                if (GUILayout.Button("Next"))
                {
                    _currentPage++;
                }
                
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.LabelField($"Page {_currentPage + 1} of {_totalPages}");
            }
            
            EditorGUILayout.Space();
            return elementsCount;
        }

        private void DrawDataAssetFoldout(DataAsset asset)
        {
            string assetName = asset.name;
            assetName = assetName.Replace(nameof(DataAsset), string.Empty);
                
            assetName = Regex.Replace(assetName, "([a-z])([A-Z])", "$1 $2");

            if (!_dataAssetsFoldouts.ContainsKey(asset))
            {
                _dataAssetsFoldouts.Add(asset, false);
            }
                
            _dataAssetsFoldouts[asset] = EditorGUI.BeginFoldoutHeaderGroup(
                EditorGUILayout.GetControlRect(),
                _dataAssetsFoldouts[asset],
                $"{assetName}"
            );
                
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (_dataAssetsFoldouts[asset])
            {
                EditorGUILayout.Space();
                
                if (asset == null)
                {
                    return;
                }

                using (new EditorGUILayout.VerticalScope("box"))
                {
                    UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(asset);
                    if (editor != null)
                    {
                        editor.OnInspectorGUI();
                    }
                }   
            }
        }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new DataAssetsSettingsProvider("Data Assets", SettingsScope.Project);
        }
    }
}
#endif