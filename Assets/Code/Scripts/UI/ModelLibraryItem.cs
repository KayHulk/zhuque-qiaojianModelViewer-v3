using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModelLibraryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera mainCamera => QuickReference.mainCamera;
    Transform mainCanvas => QuickReference.mainCanvas;

    public GameObject itemModel;
    public CanvasGroup canvasGroup;
    public GameObject line;
    public Text prefName;

    private ModelLibraryItem draggingItem;

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingItem = Instantiate(GetComponent<RectTransform>(), transform.parent).GetComponent<ModelLibraryItem>();
        draggingItem.transform.SetParent(mainCanvas);
        draggingItem.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        draggingItem.prefName.gameObject.SetActive(true);
        draggingItem.prefName.text = transform.name;
        draggingItem.canvasGroup.alpha = 0.5f;
        draggingItem.line.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingItem != null)
        {
            Camera overlay = mainCamera.transform.Find("OverlayCamera").GetComponent<Camera>();
            Debug.Log($"dP:{draggingItem.transform.position}, cP:{(Input.mousePosition)}");
            draggingItem.GetComponent<RectTransform>().anchoredPosition3D = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingItem != null)
        {
            Destroy(draggingItem.gameObject);
        }
    }
}

