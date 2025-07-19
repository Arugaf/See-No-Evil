using UnityEngine;
using System.Collections.Generic;

public class ModelIconGeneratorSettings : ScriptableObject
{
    public List<GameObject> ToRender;
    public Vector3 RotationOffset;
    public string SavePath;
    public int Resolution;
    public Color BackColor = new Color(0, 0, 0, 0);
}
