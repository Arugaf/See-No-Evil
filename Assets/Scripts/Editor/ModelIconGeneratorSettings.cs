using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class ModelIconGeneratorSettings : ScriptableObject
{
    public List<GameObject> ToRender;
    public Vector3 RotationOffset;
    public string SavePath;
    [Min(17)] public int Resolution;
    public int RendererToUse;
}
