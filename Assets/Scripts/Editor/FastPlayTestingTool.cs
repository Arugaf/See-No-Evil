#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
public class FastPlayTestWindow : EditorWindow
{
    void OnGUI()
    {
        // Use the Object Picker to select the start SceneAsset
        EditorSceneManager.playModeStartScene = (SceneAsset)EditorGUILayout.ObjectField(new GUIContent("Start Scene"), EditorSceneManager.playModeStartScene, typeof(SceneAsset), false);

        // Or set the start Scene from code
        var scenePath = "Assets\\Scenes\\IntroScene.unity";
        if (GUILayout.Button("Set start Scene: " + scenePath))
            SetPlayModeStartScene(scenePath);
        if (GUILayout.Button("Disable start scene"))
            RemovePlayModeStartScene();
    }

    void SetPlayModeStartScene(string scenePath)
    {
        SceneAsset myWantedStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        if (myWantedStartScene != null)
            EditorSceneManager.playModeStartScene = myWantedStartScene;
        else
            Debug.Log("Could not find Scene " + scenePath);
    }
    void RemovePlayModeStartScene()
    {
        EditorSceneManager.playModeStartScene = null;
    }


    [MenuItem("Tools/PlayTest")]
    static void Open()
    {
        GetWindow<FastPlayTestWindow>();
    }
}
#endif