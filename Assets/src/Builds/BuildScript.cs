using UnityEngine;
using UnityEditor;

public class BuildScript : MonoBehaviour
{
    public static void Build()
    {
        string[] scenes = { "Assets/Scenes/SampleScene.unity" };
        BuildPipeline.BuildPlayer(scenes, "BuildWin", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }
    public static void BuildWebGL()
    {
        string[] scenes = { "Assets/Scenes/SampleScene.unity" };
        BuildPipeline.BuildPlayer(scenes, "BuildWebGL", BuildTarget.WebGL, BuildOptions.None);
    }
}