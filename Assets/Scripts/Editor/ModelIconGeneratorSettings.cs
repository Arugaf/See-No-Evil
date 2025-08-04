using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Editor/ModelIconGeneratorSettings")]
public class ModelIconGeneratorSettings : ScriptableObject
{
    public List<GameObject> ToRender;
    public Vector3 RotationOffset;
    public string SavePath;
    [Min(17)] public int Resolution;
    public int RendererToUse;
    [Min(1.0f)] public float SpacingFactor = 1.0f;
}
