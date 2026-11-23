using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build.Reporting;
using TMPro;

public class SceneSetup : EditorWindow
{
 [MenuItem("LittleKitten/Setup Main Scene")]
 public static void SetupScene()
 {
 // Force new empty scene
 EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

 // Run auto-setup
 AutoSetupUI.Setup();

 // PetStats asset
 var statsAsset = ScriptableObject.CreateInstance<PetStats>();
 AssetDatabase.CreateAsset(statsAsset, "Assets/Resources/PetStats.asset");
 AssetDatabase.SaveAssets();

 // GameManager
 var gmGO = new GameObject("GameManager");
 var gm = gmGO.AddComponent<GameManager>();

 // Find UI elements and assign
 var pet = Object.FindFirstObjectByType<PetVisual>();
 var stats = Object.FindObjectsByType<StatBar>(FindObjectsInactive.Include, FindObjectsSortMode.None);
 var buttons = Object.FindFirstObjectByType<ActionButtons>();

 if (pet) gm.petVisual = pet;
 if (stats.Length > 0) gm.statBars = stats;
 if (buttons) gm.actionButtons = buttons;

 // Save scene
 EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), "Assets/Scenes/Main.unity");
 AssetDatabase.SaveAssets();
 Debug.Log("Main scene created at Assets/Scenes/Main.unity");
 }

 public static void BuildAndroid()
 {
 // Configure Android Player Settings
 PlayerSettings.productName = "Little Kitten";
 PlayerSettings.applicationIdentifier = "com.littlekitten.virtualpet";
 PlayerSettings.bundleVersion = "1.0.0";
 PlayerSettings.Android.bundleVersionCode = 1;
 PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
 PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel34;
 PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
 PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

 // App Icon
 var iconPath = "Assets/Textures/AppIcon.png";
 if (System.IO.File.Exists(iconPath))
 {
 var icons = new Texture2D[5];
 icons[0] = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
 icons[1] = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
 icons[2] = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
 icons[3] = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
 icons[4] = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
 PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, icons);
 }

 // Always recreate scene to ensure clean state
 SetupScene();

 var buildPlayerOptions = new BuildPlayerOptions
 {
 scenes = new[] { "Assets/Scenes/Main.unity" },
 locationPathName = "build/LittleKitten.apk",
 target = BuildTarget.Android,
 options = BuildOptions.None
 };

 var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
 if (report.summary.result == BuildResult.Succeeded)
 {
 Debug.Log($"Build succeeded: {report.summary.outputPath}");
 }
 else
 {
 Debug.LogError($"Build failed: {report.summary.totalErrors} errors");
 foreach (var step in report.steps)
 {
 foreach (var msg in step.messages)
 {
 if (msg.type == LogType.Error)
 Debug.LogError($"{msg.content}");
 }
 }
 }
}
}
