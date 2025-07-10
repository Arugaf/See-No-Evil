#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Registries
{
    [CustomEditor(typeof(IdentifiableRegistry<>), true)]
    public class IdentifiableRegistryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw default properties first
            DrawDefaultInspector();

            // Button to locate and assign all relevant assets
            if (GUILayout.Button("Locate All Assets"))
            {
                LocateAndAssignAssets();
            }
        }

        private void LocateAndAssignAssets()
        {
            // Get the closed generic type from the target
            Type registryType = target.GetType();
            Type elementType = GetRegistryElementType(registryType);

            if (elementType == null)
            {
                Debug.LogError("Could not determine element type for registry.");
                return;
            }

            // Find all assets of the element type
            List<ScriptableObject> assets = FindAllAssets(elementType);

            // Assign found assets to the registry
            AssignAssetsToRegistry(assets, elementType);
        }

        private Type GetRegistryElementType(Type registryType)
        {
            // Traverse base types to find the concrete IdentifiableRegistry<T>
            while (registryType != null)
            {
                if (registryType.IsGenericType &&
                    registryType.GetGenericTypeDefinition() == typeof(IdentifiableRegistry<>))
                {
                    return registryType.GetGenericArguments()[0];
                }
                registryType = registryType.BaseType;
            }
            return null;
        }

        private List<ScriptableObject> FindAllAssets(Type assetType)
        {
            string[] guids = AssetDatabase.FindAssets($"t:{assetType.Name}");
            return guids.Select(guid =>
                AssetDatabase.LoadAssetAtPath(
                    AssetDatabase.GUIDToAssetPath(guid),
                    assetType
                ) as ScriptableObject
            ).Where(asset => asset != null).ToList();
        }

        private void AssignAssetsToRegistry(List<ScriptableObject> assets, Type elementType)
        {
            SerializedObject serializedTarget = new SerializedObject(target);
            SerializedProperty valuesProp = serializedTarget.FindProperty("<Values>k__BackingField");

            if (valuesProp == null)
            {
                Debug.LogError("Could not find 'Values' property in registry.");
                return;
            }

            // Convert list to appropriate array type
            Array array = Array.CreateInstance(elementType, assets.Count);
            for (int i = 0; i < assets.Count; i++)
            {
                array.SetValue(assets[i], i);
            }

            // Update serialized property
            valuesProp.ClearArray();
            for (int i = 0; i < assets.Count; i++)
            {
                valuesProp.InsertArrayElementAtIndex(i);
                SerializedProperty elementProp = valuesProp.GetArrayElementAtIndex(i);
                elementProp.objectReferenceValue = assets[i];
            }

            serializedTarget.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
            Debug.Log($"Assigned {assets.Count} assets to registry.");
        }
    }
}
#endif