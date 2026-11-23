using UnityEditor;
using UnityEngine;

public class BuildScript
{
    public static void BuildAndroid()
    {
        string[] scenes = { "Assets/Scenes/Main.unity" };
        string outputPath = "Builds/LittleKitten.apk";
        
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = outputPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };
        
        BuildPipeline.BuildPlayer(options);
        Debug.Log($"Build complete: {outputPath}");
    }
}
