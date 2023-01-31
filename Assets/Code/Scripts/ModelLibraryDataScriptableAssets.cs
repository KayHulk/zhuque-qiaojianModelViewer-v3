using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ModelLibraryDataScriptableAsset", menuName = "ScriptableAssetObjects/ModelLibraryDataScriptableAsset")]
public class ModelLibraryDataScriptableAssets : ScriptableObject
{
    public List<ModelLibrayItemData> ModelLibrayItems;
}

[Serializable]
public class ModelLibrayItemData
{
    public string Name;
    public Texture Cover;
    public GameObject Model;
}
