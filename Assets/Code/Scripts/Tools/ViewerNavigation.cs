using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using Lean.Common;
using UnityEngine.UIElements;
using DG.Tweening;
using DG.Tweening.Core.Easing;

public class ViewerNavigation : MonoBehaviour
{
    Camera mainCamera => QuickReference.mainCamera;
    LeanTouch leanTouch => QuickReference.leanTouch;
    Transform sceneParent => QuickReference.sceneParent;

    private LeanFingerTap _leanFingerTap;
    private LeanFingerTap leanFingerTap
    {
        get
        {
            if(_leanFingerTap == null && leanTouch != null)
            {
                _leanFingerTap = leanTouch.GetComponent<LeanFingerTap>();
            }
            return _leanFingerTap;
        }
    }

    private LeanMaintainDistance _leanMaintainDistance = null;
    private LeanMaintainDistance leanMaintainDistance
    {
        get
        {
            if (_leanMaintainDistance == null) _leanMaintainDistance = mainCamera.GetComponent<LeanMaintainDistance>();
            return _leanMaintainDistance;
        }
    }

    private LeanPitchYaw _leanPitchYaw = null;
    private LeanPitchYaw leanPitchYaw
    {
        get
        {
            if (_leanPitchYaw == null) _leanPitchYaw = cameraPivot.GetComponentInParent<LeanPitchYaw>();
            return _leanPitchYaw;
        }
    }

    private Transform _cameraPivot = null;
    private Transform cameraPivot
    {
        get
        {
            if (_cameraPivot == null) _cameraPivot = mainCamera.transform.parent;
            return _cameraPivot;
        }
    }

    private float defaultClampMaxValue = 20.0f;
    private Vector2 defaultPitchYawValue = Vector2.zero;

    private void OnEnable()
    {
        if (leanFingerTap != null) leanFingerTap.OnFinger.AddListener(FingerTapHandler);
    }

    private void OnDisable()
    {
        if (leanFingerTap != null) leanFingerTap.OnFinger.RemoveListener(FingerTapHandler);
    }

    private void FingerTapHandler(LeanFinger finger)
    {
        if (finger.TapCount == 2)
        {
            RayDetect(finger.ScreenPosition);
        }
    }

    private void RayDetect(Vector3 mousePos)
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000))
        {
            var renderer = hitInfo.transform.GetComponent<Renderer>();
            
            if (renderer != null)
            {
                Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.red, 10f);
                FocusScreenPoint(hitInfo.point, hitInfo.normal);
            }
        }
        else
        {
            CenterWholeModel();
        }
    }

    private void FocusScreenPoint(Vector3 hitPoint, Vector3 hitPointNormal)
    {
        Debug.Log($"FocusScreenPoint");
        Vector3 dir = (hitPointNormal * -1).normalized;
        Vector3 pos = (hitPointNormal * -8) + hitPoint;

        leanPitchYaw.RotateToPosition(pos);
        cameraPivot.DOMove(hitPoint,0.3f);
        leanMaintainDistance.Distance = 8;
    }

    private void CenterWholeModel()
    {
        Debug.Log($"CenterWholeModel");
        var bounds = GetWholeSceneBounds();
        var camDis = CalculateCameraPreferredDistance(bounds);

        cameraPivot.DOMove(bounds.center, 0.3f);
        leanMaintainDistance.Distance = camDis;
    }

    private float CalculateCameraPreferredDistance(Bounds bounds)
    {
        Vector3 boundsCenter = bounds.center;
        Vector3 boundsExtents = bounds.extents;

        float deltaX = Mathf.Abs(boundsExtents.x);
        float deltaY = Mathf.Abs(boundsExtents.y);
        float deltaZ = Mathf.Abs(boundsExtents.z);

        Vector3[] boundsPoints = new Vector3[8];

        boundsPoints[0] = boundsCenter + new Vector3(-deltaX, deltaY, -deltaZ);        // 上前左（相对于中心点）
        boundsPoints[1] = boundsCenter + new Vector3(deltaX, deltaY, -deltaZ);         // 上前右
        boundsPoints[2] = boundsCenter + new Vector3(deltaX, deltaY, deltaZ);          // 上后右
        boundsPoints[3] = boundsCenter + new Vector3(-deltaX, deltaY, deltaZ);         // 上后左

        boundsPoints[4] = boundsCenter + new Vector3(-deltaX, -deltaY, -deltaZ);       // 下前左
        boundsPoints[5] = boundsCenter + new Vector3(deltaX, -deltaY, -deltaZ);        // 下前右
        boundsPoints[6] = boundsCenter + new Vector3(deltaX, -deltaY, deltaZ);         // 下后右
        boundsPoints[7] = boundsCenter + new Vector3(-deltaX, -deltaY, deltaZ);        // 下后左


        float d1 = Vector3.Distance(boundsPoints[0], boundsPoints[1]);
        float d2 = Vector3.Distance(boundsPoints[0], boundsPoints[3]);
        float d3 = Vector3.Distance(boundsPoints[0], boundsPoints[4]);
        float cameraDis = Mathf.Max(d1, d2, d3) * (60/mainCamera.fieldOfView*1.2f);

        float newMax = cameraDis * (60 / mainCamera.fieldOfView * 2);
        leanMaintainDistance.ClampMax = defaultClampMaxValue < newMax ? newMax : defaultClampMaxValue;

        return cameraDis;
    }

    private Bounds GetWholeSceneBounds()
    {
        var renderes = sceneParent.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds();
        for (int i = 0; i < renderes.Length; i++)
        {
            if (i == 0)
            {
                bounds = renderes[i].bounds;
            }
            else
            {
                bounds.Encapsulate(renderes[i].bounds);
            }
        }
        return bounds;
    }
}
