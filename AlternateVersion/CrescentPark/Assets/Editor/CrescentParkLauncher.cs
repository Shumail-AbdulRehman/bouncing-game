using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class CrescentParkLauncher
{
    public static void Run()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/ParkPhysicsEnvironment.unity");
        EditorApplication.delayCall += () =>
        {
            EditorApplication.ExecuteMenuItem("Window/General/Game");
            EditorApplication.isPlaying = true;
            Debug.Log("CRESCENT_PARK_PLAY_REQUESTED");
        };
    }
}
