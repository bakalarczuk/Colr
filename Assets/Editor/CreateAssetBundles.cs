using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles/Android")]
    static void BuildAllAssetBundlesAndroid()
    {
        string assetBundleDirectory = "Assets/AssetBundles/Android";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
    }
    [MenuItem("Assets/Build AssetBundles/iOS")]
    static void BuildAllAssetBundlesiOS()
    {
        string assetBundleDirectory = "Assets/AssetBundles/iOS";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS);
    }
}