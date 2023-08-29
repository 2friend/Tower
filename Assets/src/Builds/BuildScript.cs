using UnityEngine;
using UnityEditor;

public class BuildScript : MonoBehaviour
{
    public static void Build()
    {
        string[] scenes = { "Assets/Scenes/SampleScene.unity" };
        BuildPipeline.BuildPlayer(scenes, "BuildWin", BuildTarget.StandaloneWindows, BuildOptions.None);
    }
}