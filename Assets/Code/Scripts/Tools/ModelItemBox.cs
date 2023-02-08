using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelItemBox : MonoBehaviour
{
    Camera mainCamera => QuickReference.mainCamera;
    Transform sceneParent => QuickReference.sceneParent;

    [SerializeField] private Transform LinesObj;
    [SerializeField] private BoundsPointsDirScriptableAssets boundsDirs;

    [HideInInspector] public GameObject model;
    Bounds Bounds;

    public void SetModel(GameObject _model)
    {
        model = Instantiate(_model, sceneParent);
        model.transform.SetParent(transform);
        Bounds = model.GetComponent<Collider>().bounds;
        Vector3 pos = model.transform.position;
        pos.y = Bounds.size.y * 0.5f;
        model.transform.position = pos;

        model.SetActive(false);
    }

    #region Draw Line
    public void ShowModelsBounds()
    {
        DrawBoundsLine(Bounds);
    }

    private void DrawBoundsLine(Bounds bounds)
    {
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        float deltaX = Mathf.Abs(extents.x);
        float deltaY = Mathf.Abs(extents.y);
        float deltaZ = Mathf.Abs(extents.z);

        Vector3[] points = new Vector3[8];

        points[0] = center + new Vector3(-deltaX, deltaY + bounds.size.y * 0.5f, -deltaZ);        // 上前左（相对于中心点）
        points[1] = center + new Vector3(deltaX, deltaY + bounds.size.y * 0.5f, -deltaZ);         // 上前右
        points[2] = center + new Vector3(deltaX, deltaY + bounds.size.y * 0.5f, deltaZ);          // 上后右
        points[3] = center + new Vector3(-deltaX, deltaY + bounds.size.y * 0.5f, deltaZ);         // 上后左

        points[4] = center + new Vector3(-deltaX, -deltaY + bounds.size.y * 0.5f, -deltaZ);       // 下前左
        points[5] = center + new Vector3(deltaX, -deltaY + bounds.size.y * 0.5f, -deltaZ);        // 下前右
        points[6] = center + new Vector3(deltaX, -deltaY + bounds.size.y * 0.5f, deltaZ);         // 下后右
        points[7] = center + new Vector3(-deltaX, -deltaY + bounds.size.y * 0.5f, deltaZ);        // 下后左

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 p = points[i];
            Matrix4x4 m_q = Matrix4x4.Rotate(transform.rotation);
            Matrix4x4 m_t = Matrix4x4.Translate(transform.position);

            points[i] = (m_t * m_q).MultiplyPoint(p);
        }

        int index = 0;
        for (int i = 0; i < points.Length; i++)
        {
            Vector3[] dirs = Utility.CaculateBoundPointDir(i, points);
            for (int k = 0; k < dirs.Length; k++)
            {
                Vector3 startPos = points[i];
                Vector3 endPos = points[i] + (dirs[k] * boundsDirs.lineLength);
                // Debug.DrawLine(points[i], points[i] + (dirs[k] * boundsDirs.lineLength), boundsDirs.lineColor);
                LineRenderer line = LinesObj.GetChild(index).GetComponent<LineRenderer>();
                index++;
                line.material.color = boundsDirs.lineColor;
                line.startWidth = line.endWidth = boundsDirs.lineThickness;
                line.startColor = line.endColor = boundsDirs.lineColor;
                line.SetPositions(new Vector3[2] { startPos, endPos });
            }
        }
    }

    #endregion

    public void SetTransformDir(Vector2 mousePos)
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000))
        {
            var renderer = hitInfo.transform.GetComponent<Renderer>();
            if (renderer != null)
            {
                transform.DORotate(transform.localEulerAngles + (hitInfo.normal * -1), 0.1f);
                transform.position = hitInfo.point;
            }
        }

    }

    public float GetBoundsSizeY()
    {
        return Bounds.size.y;
    }

    public float GetBoundsSizeX()
    {
        return Bounds.size.x;
    }

}
