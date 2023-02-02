using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickReference : MonoBehaviour
{
    private static LeanTouch _leanTouch = null;
    public static LeanTouch leanTouch
    {
        get
        {
            if (_leanTouch == null) _leanTouch = GameObject.FindObjectOfType<LeanTouch>();
            return _leanTouch;
        }
    }

    private static Camera _mainCamera = null;
    public static Camera mainCamera
    {
        get
        {
            if (_mainCamera == null) _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            return _mainCamera;
        }
    }

    private static Transform _sceneParent = null;
    public static Transform sceneParent
    {
        get
        {
            if (_sceneParent == null) _sceneParent = GameObject.FindGameObjectWithTag("Scene").transform;
            return _sceneParent;
        }
    }
}
