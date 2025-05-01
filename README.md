# DataAssets Package

The **DataAssets Package** provides a system for managing and accessing global data in Unity using `ScriptableObject` instances. It ensures that data assets are automatically created, validated, and preloaded for runtime use.

## Features
- Automatically creates and validates `DataAsset` instances.
- Stores data assets in a user-specified folder.
- Preloads data assets for runtime access.
- Provides a simple API for accessing data assets globally.

## Initialization
- **Editor**: The `DataAssetEditorHelper` ensures all `DataAsset` instances are created and validated when the project changes.
- **Runtime**: The `DataAssetsLoader` preloads all `DataAsset` instances at startup and registers them for global access.

---

## How to Use

### 1. Configure the DataAssets Folder
Find the `DataAssetConfig` object in your project. This object specifies the folder where `DataAsset` instances will be stored. Change settings to taste.

### 2. Create a `DataAsset` Class
Create a class that inherits from `DataAsset`. After recompilation `DataAssetEditorHelper` will create the ScriptableObject asset in the folder specified in the config asset.

```csharp
using DataAssetsPackage;

public class GameSettingsDataAsset : DataAsset
{
    public float defaultVolume;
    public string defaultLanguage;
}
```

### 3. Access Data Assets in Code
Use the `DataAssets.Get<T>()` method to access your data assets globally.

```csharp
using DataAssetsPackage;
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    public void Start()
    {
        PrintSettings();
    }

    public void PrintSettings()
    {
        GameSettingsDataAsset settings = DataAssets.Get<GameSettingsDataAsset>();
        Debug.Log($"Default Volume: {settings.defaultVolume}");
        Debug.Log($"Default Language: {settings.defaultLanguage}");
    }
}
```

### 4. Project Settings Window Integration
The `DataAssetsSettingsProvider` provides an editor interface for editing data assets in the Project Settings. Access it in the **Project Settings** Window under **Data Assets**, you can also edit the **ScriptableObject** asset on the inpector as you would with any other **ScriptableObject**.

![{6EF2DC17-26E8-4BBE-9D7E-8FC640498409}](https://github.com/user-attachments/assets/2c629d61-ac4d-4b94-93cd-281c6c227653)

---

## Best Practices
### 1. Don't add MEMORY HEAVY assets in DataAssets.
- Gigantic lists/dictionaries or big strings (as in a giant JSON text) might impact startup time.
- If you want to add GameObjects or assets that are heavy, use Addressables via AssetReference a load them as needed.
- I'm not sure if heavy assets that are Unity assets are an issue, they might not be loaded in memory with the DataAsset object. Figure it out.
### 2. Use these as a "global" READ ONLY data container, a way to store data that is not tied to a specific object and is more broad in uses.
- For example, you can use this to store the project scene references, like an Addressable Scene and have only minimal memory usage for initialization.
- You can use this to store the game settings, like the default game volume settings, default language.
### 3. Use this as a READ ONLY data provider, I don't recommend changing the data in runtime.
- This is a recommendation for ScriptableObjects in general, but in this case, it is even more important as these are loaded at startup and are never loaded again.
### 4. Don't call stuff from a DataAsset on another DataAsset.
- I doubt this would ever happen, but if you do this, you might end up with a circular dependency and race conditions.
### 5. Use the DataAsset postfix for your DataAssets
- Totally optional.
