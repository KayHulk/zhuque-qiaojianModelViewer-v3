using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModelLibraryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera mainCamera => QuickReference.mainCamera;
    Transform mainCanvas => QuickReference.mainCanvas;
    Transform sceneParent => QuickReference.sceneParent;

    [HideInInspector] public GameObject itemModel;
    [HideInInspector] public RectTransform libratyRect;
    [SerializeField] private GameObject draggingUIPref;
    [SerializeField] private GameObject dragging3DPref;

    private DraggingItem draggingUIItem;
    private ModelItemBox dragging3DItem;

    public void OnBeginDrag(PointerEventData eventData)
    {
        InitDraggingUI();
        InitDragging3D();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingUIItem == null)
            return;
        
        if (RectTransformUtility.RectangleContainsScreenPoint(libratyRect, Input.mousePosition))
        {
            if (!draggingUIItem.gameObject.activeSelf) draggingUIItem.gameObject.SetActive(true);
            if (dragging3DItem.gameObject.activeSelf) dragging3DItem.gameObject.SetActive(false);

            draggingUIItem.GetComponent<RectTransform>().anchoredPosition3D = Input.mousePosition;
        }
        else
        {
            if (draggingUIItem.gameObject.activeSelf) draggingUIItem.gameObject.SetActive(false);
            if (!dragging3DItem.gameObject.activeSelf) dragging3DItem.gameObject.SetActive(true);

            Vector3 screenPos = mainCamera.WorldToScreenPoint(dragging3DItem.transform.position);
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = screenPos.z;
            Vector3 wordPos = mainCamera.ScreenToWorldPoint(mousePos);

            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            int layer = LayerMask.NameToLayer("Scene");
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000, (1 << layer)))
            {
                var renderer = hitInfo.transform.GetComponent<Renderer>();

                if (renderer != null)
                {
                    Debug.Log($"renderer:{renderer.name}, 3d:{dragging3DItem.name}");
                    //dragging3DItem.transform.localEulerAngles = dragging3DItem.transform.localEulerAngles + (hitInfo.normal * -1);

                    Vector3 v = dragging3DItem.transform.up;
                    dragging3DItem.transform.up = hitInfo.normal;

                    wordPos = hitInfo.point;
                    
                }
            }
            else
            {
                dragging3DItem.transform.localEulerAngles = Vector3.zero;
                wordPos.y = dragging3DItem.GetBoundsSizeY() * 0.5f;
            }

            dragging3DItem.transform.position = wordPos;
            dragging3DItem.ShowModelsBounds();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingUIItem != null)
        {
            Destroy(draggingUIItem.gameObject);
        }
        if (dragging3DItem != null)
        {
            Destroy(dragging3DItem.gameObject);
            // TODO:将对应 3D Object 放置到对应位置上
        }
    }

    private void InitDraggingUI()
    {
        if (draggingUIItem != null)
            Destroy(draggingUIItem.gameObject);

        draggingUIItem = Instantiate(draggingUIPref, mainCanvas).GetComponent<DraggingItem>();
        draggingUIItem.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        draggingUIItem.rawImage.texture = GetComponent<RawImage>().texture;
        draggingUIItem.prefName.text = transform.name;
        draggingUIItem.canvasGroup.alpha = 0.5f;

        draggingUIItem.gameObject.SetActive(true);
    }

    private void InitDragging3D()
    {
        if (dragging3DItem != null)
            Destroy(dragging3DItem.gameObject);

        dragging3DItem = Instantiate(dragging3DPref, sceneParent).GetComponent<ModelItemBox>();
        dragging3DItem.SetModel(itemModel);

        dragging3DItem.gameObject.SetActive(false);
    }
}

