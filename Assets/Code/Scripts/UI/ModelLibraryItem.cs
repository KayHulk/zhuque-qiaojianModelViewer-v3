using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModelLibraryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera mainCamera => QuickReference.mainCamera;
    Transform mainCanvas => QuickReference.mainCanvas;

    [HideInInspector] public GameObject itemModel;
    [SerializeField] private GameObject draggingPref;

    private DraggingItem draggingItem;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (draggingItem != null)
            Destroy(draggingItem.gameObject);

        draggingItem = Instantiate(draggingPref, mainCanvas).GetComponent<DraggingItem>();
        draggingItem.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        draggingItem.rawImage.texture = GetComponent<RawImage>().texture;
        draggingItem.prefName.text = transform.name;
        draggingItem.canvasGroup.alpha = 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingItem == null)
            return;
     
            draggingItem.GetComponent<RectTransform>().anchoredPosition3D = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingItem != null)
        {
            Destroy(draggingItem.gameObject);
        }
    }
}

