using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ProjectBuilder 
	: MonoBehaviour 
{
    // 実行ファイルを置くディレクトリ
    const string EXE_DIRECTORY = "../../Executables/";

    // 作成するファイル名
    const string FILE_NAME = "MobileVRJam";

    // 上記のディレクトリ内で、apkを置く場所
    const string ANDROID_DIRECTORY = "Android/";

    // アンドロイドアプリの拡張子
    const string ANDROID_EXTENSION = ".apk";

    class BuildSettings
    {
        public string Name = "";
        public string NameToAdd = "";
        public string FileExtension = "";
        public bool IsVRMode = false;
        public string DirectoryToAdd = "";
        public BuildTarget TargetPlatform = BuildTarget.Android;
        public BuildOptions BuildOption = BuildOptions.None;
    }

    [MenuItem( "Tools/Build/Build All")]
    public static void BuildAll()
    {
        //// 現在の設定を控える
        //BuildTarget previousPlatform = EditorUserBuildSettings.activeBuildTarget;
        string defaultProductName = PlayerSettings.productName;
        string defaultBundleID = PlayerSettings.bundleIdentifier;
        bool vrMode = UnityEngine.VR.VRSettings.enabled;

        // パスを書き出す
        Debug.Log("Build start! - path = " + System.IO.Directory.GetCurrentDirectory() );

        // ビルド対象シーンリスト
        string[] sceneList = GetEnabledScenes();

        // ビルド設定
        List<BuildSettings> settings = CreateAllBuildSettings();
        int settingCount = settings.Count;

        for (int i = 0; i < settingCount; ++i)
        {
            BuildPlayer(settings[i], sceneList, defaultProductName, defaultBundleID, vrMode);
        }

        // 元に戻す
        //EditorUserBuildSettings.SwitchActiveBuildTarget(prevPlatform);
    }

    [MenuItem("Tools/Build/Build GearVR")]
    public static void BuildGearVR()
    {
        BuildSingleAndroidTarget(true);
    }

    [MenuItem("Tools/Build/Build Android")]
    public static void BuildAndroid()
    {
        BuildSingleAndroidTarget(false);
    }

    private static void BuildSingleAndroidTarget(bool isGearVR)
    {
        string defaultProductName = PlayerSettings.productName;
        string defaultBundleID = PlayerSettings.bundleIdentifier;
        bool vrMode = UnityEngine.VR.VRSettings.enabled;

        BuildSettings settings;
        if (isGearVR)
        {
            settings = CreateGearVRSetting();
        }
        else
        {
            settings = CreateAndroidSetting();
        }

        settings.BuildOption = BuildOptions.AutoRunPlayer;
        BuildPlayer(settings, GetEnabledScenes(), defaultProductName, defaultBundleID, vrMode);
    }

    private static void BuildPlayer(BuildSettings setting, string[] sceneList, 
                                    string productName, string bundleID, bool isVRMode
                                    )
    {
        string path = EXE_DIRECTORY + setting.DirectoryToAdd + productName + setting.NameToAdd + setting.FileExtension;
        Debug.Log("Starting Build [" + setting.Name + "] : path = " + path);

        SetPlayerSettings(productName + setting.NameToAdd, bundleID + setting.NameToAdd, setting.IsVRMode);

        // 実行
        string errorMessage = BuildPipeline.BuildPlayer
        (
            sceneList,              //!< ビルド対象シーンリスト
            path,                   //!< 出力先
            setting.TargetPlatform, //!< ビルド対象プラットフォーム
            setting.BuildOption     //!< ビルドオプション
        );

        // 結果出力
        if (!string.IsNullOrEmpty(errorMessage))
        {
            Debug.LogError("[Error!] " + errorMessage);
        }
        else
        {
            Debug.Log("[Success!]");
        }

        // 元に戻す
        SetPlayerSettings(productName, bundleID, isVRMode);
    }

    private static string[] GetEnabledScenes()
    {
        List<string> sceneList = new List<string>();

        // "Scenes In Build"に登録されているシーンリストを取得
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene scene in scenes)
        {
            // チェックがついているか確認
            if (scene.enabled)
            {
                // リストに加える
                sceneList.Add(scene.path);
            }
        }

        // 配列にして返す
        return sceneList.ToArray();
    }

    private static void SetPlayerSettings(string productName, string bundleID, bool vrMode)
    {
        PlayerSettings.productName = productName;
        PlayerSettings.bundleIdentifier = bundleID;
        PlayerSettings.virtualRealitySupported = vrMode;
        PlayerSettings.Android.bundleVersionCode++;
    }

    private static List<BuildSettings> CreateAllBuildSettings()
    {
        List<BuildSettings> settings = new List<BuildSettings>();

        // アンドロイド/cardboard
        BuildSettings android = CreateAndroidSetting();
        settings.Add(android);

        // GearVR設定
        BuildSettings gearVr = CreateGearVRSetting();
        settings.Add(gearVr);

        return settings;
    }

    private static BuildSettings CreateAndroidSetting()
    {
        BuildSettings android = new BuildSettings();
        android.Name = "Android";
        android.DirectoryToAdd = ANDROID_DIRECTORY;
        android.FileExtension = ANDROID_EXTENSION;
        android.IsVRMode = false;
        android.NameToAdd = "";
        android.BuildOption = BuildOptions.AutoRunPlayer;
        return android;
    }

    private static BuildSettings CreateGearVRSetting()
    {
        const string GEAR_VR_NAME = "_Gear";
        BuildSettings gearVr = new BuildSettings();
        gearVr.Name = "Gear VR";
        gearVr.DirectoryToAdd = ANDROID_DIRECTORY;
        gearVr.FileExtension = ANDROID_EXTENSION;
        gearVr.IsVRMode = true;
        gearVr.NameToAdd = GEAR_VR_NAME;
        gearVr.BuildOption = BuildOptions.AutoRunPlayer;
        return gearVr;
    }
}
