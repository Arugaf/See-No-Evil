using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Rendering.Universal;
#if UNITY_EDITOR
[CustomEditor(typeof(ModelIconGeneratorSettings), true)]
public class ModelIconGeneratorSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw default properties first
        DrawDefaultInspector();

        // Button to locate and assign all relevant assets
        if (GUILayout.Button("Set in icon generator"))
        {
            ModelIconGenerator g = EditorWindow.GetWindow<ModelIconGenerator>("Model Icon Generator");
            g.Settings = (ModelIconGeneratorSettings)target;
            g.Show();
        }
    }
}
#endif
public class ModelIconGenerator : EditorWindow
{
    public ModelIconGeneratorSettings Settings;

    [MenuItem("Tools/Generate Model Icon")]
    public static void ShowWindow() => GetWindow<ModelIconGenerator>("Model Icon Generator");

    void OnGUI()
    {
        GUILayout.Label("Model Icon Settings", EditorStyles.boldLabel);
        Settings = (ModelIconGeneratorSettings)EditorGUILayout.ObjectField(Settings, typeof(ModelIconGeneratorSettings), false);

        if (GUILayout.Button("Create settings"))
        {
            var path = UnityEditor.EditorUtility.SaveFilePanelInProject(
                "Save ModelIconSettings",
                "ModelIconSettings",
                "asset",
                string.Empty);

            if (!string.IsNullOrEmpty(path))
            {

                var newSettings = CreateInstance<ModelIconGeneratorSettings>();
                UnityEditor.AssetDatabase.CreateAsset(newSettings, path);
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = newSettings;
            }
        }

        if (GUILayout.Button("Generate Icon") && Settings != null)
        {
            foreach (GameObject gm in Settings.ToRender)
            {
                GenerateIcon(gm);
            }
        }
    }

    void GenerateIcon(GameObject prefab)
    {
        int resolution = Settings.Resolution * 4;
        // Create temporary environment
        GameObject tempParent = new GameObject("TempIconRenderer");
        // At the end of the world.
        tempParent.transform.position = new Vector3(100, -100, 100);
        SceneVisibilityManager.instance.Hide(tempParent, true);

        // Setup camera
        Camera renderCam = new GameObject("RenderCamera").AddComponent<Camera>();
        var dat = renderCam.gameObject.AddComponent<UniversalAdditionalCameraData>();
        dat.renderPostProcessing = false;
        dat.SetRenderer(Settings.RendererToUse);
        renderCam.transform.SetParent(tempParent.transform);
        renderCam.backgroundColor = new Color(0, 0, 0, 0);
        renderCam.clearFlags = CameraClearFlags.SolidColor;
        renderCam.orthographic = true;
        renderCam.orthographicSize = 1.5f;
        renderCam.nearClipPlane = 0.1f;
        renderCam.farClipPlane = 10f;
        renderCam.allowHDR = false;
        renderCam.forceIntoRenderTexture = true;
        // Instantiate model
        GameObject modelInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        modelInstance.transform.position = tempParent.transform.position;
        modelInstance.transform.SetParent(tempParent.transform);
        modelInstance.transform.rotation = Quaternion.Euler(Settings.RotationOffset);

        // Position model and camera
        Bounds bounds = CalculateBounds(modelInstance);
        renderCam.transform.position = bounds.center - Vector3.forward * 5f;
        renderCam.orthographicSize = Mathf.Max(bounds.extents.x, bounds.extents.y) * 1.2f;

        // Create render texture
        RenderTexture rt = RenderTexture.GetTemporary(resolution, resolution, 24, RenderTextureFormat.ARGBFloat);
        renderCam.targetTexture = rt;
        renderCam.Render();

        // Process texture
        Texture2D icon = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false);
        RenderTexture.active = rt;
        icon.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);
        icon.filterMode = FilterMode.Point;
        icon.Apply();
        icon = ResizeTexture(icon, resolution / 4, resolution / 4);
        // Save results
        SaveIcon(icon, prefab.name);
        // Cleanup
        RenderTexture.active = null;
        renderCam.targetTexture = null;
        DestroyImmediate(tempParent);
        RenderTexture.ReleaseTemporary(rt);
    }
    Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight, 24, RenderTextureFormat.ARGBFloat);
        RenderTexture.active = rt;

        // Blit with bilinear filtering
        Graphics.Blit(source, rt);
        Texture2D result = new Texture2D(newWidth, newHeight, TextureFormat.ARGB32, false);
        result.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        result.Apply();
        result.filterMode = FilterMode.Point; 
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);
        return result;
    }

    Bounds CalculateBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds();

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }
        return bounds;
    }

    void SaveIcon(Texture2D icon, string prefix)
    {
        byte[] bytes = icon.EncodeToPNG();
        string path = $"{Settings.SavePath}/{prefix}_Icon.png";

        Directory.CreateDirectory(Settings.SavePath);
        File.WriteAllBytes(path, bytes);
        AssetDatabase.Refresh();

        // Configure texture importer
        string assetPath = path.Substring(path.IndexOf("Assets"));
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.alphaSource = TextureImporterAlphaSource.FromInput;
            importer.SaveAndReimport();
        }

        Debug.Log($"Saved icon to: {assetPath}");
    }
}