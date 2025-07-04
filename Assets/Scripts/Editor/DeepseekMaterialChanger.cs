using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class MaterialReplacementTool : EditorWindow
{
    private Material templateMaterial;
    private string targetPath = "Assets/";
    private readonly Dictionary<Material, Material> materialMap = new Dictionary<Material, Material>();

    [MenuItem("Tools/Material Replacement Tool")]
    public static void ShowWindow()
    {
        GetWindow<MaterialReplacementTool>("Material Replacer");
    }

    void OnGUI()
    {
        GUILayout.Label("Material Replacement Settings", EditorStyles.boldLabel);
        
        templateMaterial = (Material)EditorGUILayout.ObjectField(
            "Template Material", 
            templateMaterial, 
            typeof(Material), 
            false
        );

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Save Path", targetPath);
        if (GUILayout.Button("Browse", GUILayout.Width(80)))
        {
            string path = EditorUtility.SaveFolderPanel("Select Material Folder", "Assets", "");
            if (!string.IsNullOrEmpty(path))
            {
                targetPath = "Assets" + path.Substring(Application.dataPath.Length);
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        
        GUI.enabled = templateMaterial != null;
        if (GUILayout.Button("Process Selected Objects"))
        {
            ProcessMaterials();
        }
        GUI.enabled = true;

        if (GUILayout.Button("Reset All Materials"))
        {
            ResetMaterials();
        }

        EditorGUILayout.HelpBox(
            "1. Assign template material\n" +
            "2. Set save path (default: Assets/)\n" +
            "3. Select GameObjects in hierarchy\n" +
            "4. Click Process button", 
            MessageType.Info
        );
    }

    void ProcessMaterials()
    {
        if (Selection.gameObjects.Length == 0)
        {
            Debug.LogWarning("No GameObjects selected!");
            return;
        }

        materialMap.Clear();
        HashSet<Material> processedMaterials = new HashSet<Material>();

        foreach (GameObject go in Selection.gameObjects)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.sharedMaterials;
                //bool materialsChanged = false;

                for (int i = 0; i < materials.Length; i++)
                {
                    Material originalMat = materials[i];
                    if (originalMat == null) continue;

                    if (!processedMaterials.Contains(originalMat))
                    {
                        CreateReplacementMaterial(originalMat);
                        processedMaterials.Add(originalMat);
                        //if (newMaterial != null)
                        //{
                        //    materialMap[originalMat] = newMaterial;
                           
                        //}
                    }

                    //if (materialMap.TryGetValue(originalMat, out Material replacement))
                    //{
                    //    materials[i] = replacement;
                    //    materialsChanged = true;
                    //}
                }

                //if (materialsChanged)
                //{
                //    Undo.RecordObject(renderer, "Material Replacement");
                //    renderer.sharedMaterials = materials;
                //}
            }
        }

        Debug.Log($"Processed {processedMaterials.Count} materials");
    }

    Material CreateReplacementMaterial(Material original)
    {
        // Create new material instance
        Material newMaterial = new Material(templateMaterial)
        {
            name = $"{original.name}_Replacement"
        };

        // Copy main texture
        if (original.HasProperty("_MainTex"))
        {
            Texture mainTex = original.GetTexture("_MainTex");
            if (mainTex != null)
            {
                newMaterial.SetTexture("_MainTex", mainTex);
            }
        }

        // Save as asset
        string safeName = Path.GetInvalidFileNameChars().Aggregate(newMaterial.name, 
            (current, c) => current.Replace(c.ToString(), "_"));
        
        string fullPath = Path.Combine(targetPath, $"{safeName}.mat");
        AssetDatabase.CreateAsset(newMaterial, AssetDatabase.GenerateUniqueAssetPath(fullPath));
        
        return newMaterial;
    }

    void ResetMaterials()
    {
        materialMap.Clear();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Material cache cleared");
    }
}