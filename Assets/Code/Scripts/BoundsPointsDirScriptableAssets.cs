using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BoundsPointsDirScriptableAsset", menuName = "ScriptableAssetObjects/BoundsPointsDirScriptableAsset")]
public class BoundsPointsDirScriptableAssets : ScriptableObject
{
    public float lineLength = 0.3f;
    public float lineThickness = 0.009f;
    public Color lineColor = Color.yellow;
    public List<PointDirections> BoundsPoints;
}

[Serializable]
public class PointDirections
{
    public List<Vector3> Dir;
}
