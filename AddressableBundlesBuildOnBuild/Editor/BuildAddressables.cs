using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using System.Collections;

class BuildAddressablesProcessor
{
    /// <summary>
    /// Run a clean build before export.
    /// </summary>
    
    // PreBuild callbacks are not usable for building asset bundles + addressables. Unity throws following error.
    // InvalidOperationException: Cannot build asset bundles while a build is in progress.
    // This workaround gets prebuild hooks to call assetbundle generation methods that does not depend on callbacks
    static public void PreExport()
    {
        Debug.Log("BuildAddressablesProcessor.PreExport start");
        AddressableAssetSettings.CleanPlayerContent(
            AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
        AddressableAssetSettings.BuildPlayerContent();
        Debug.Log("BuildAddressablesProcessor.PreExport done");
    }

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayerHandler);
    }

    private static void BuildPlayerHandler(BuildPlayerOptions options)
    {
        if (EditorUtility.DisplayDialog("Build with Addressables",
            "Do you want to build a clean addressables before export?",
            "Build with Addressables", "Skip"))
        {
            PreExport();
        }
        BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(options);
    }

}
