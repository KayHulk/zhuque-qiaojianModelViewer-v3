using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ViewerPanel : MonoBehaviour
{
    public Button btn_Library;
    public RectTransform ModelLibraryPanel;

    private bool isLibraryOpen = false;
    private float tweeningDuration = 0.2f;

    [SerializeField] private RectTransform libContainer;
    [SerializeField] private RectTransform libItemPrefab;
    [SerializeField] private ModelLibraryDataScriptableAssets modelLibItemDatas;

    private void OnEnable()
    {
        btn_Library.onClick.AddListener(SetLibraryState);
    }

    private void OnDisable()
    {
        btn_Library.onClick.RemoveListener(SetLibraryState);
    }

    private void Start()
    {
        InitLibraryContent();
    }

    private void InitLibraryContent()
    {
        ClearLibraryContainer();
        for (int i = 0; i < modelLibItemDatas.ModelLibrayItems.Count; i++)
        {
            var data = modelLibItemDatas.ModelLibrayItems[i];
            RectTransform rect = Instantiate(libItemPrefab, libContainer);
            rect.name = data.Name;
            rect.GetComponent<RawImage>().texture = data.Cover;
            rect.GetComponent<ModelLibraryItem>().itemModel = data.Model;
            rect.GetComponent<ModelLibraryItem>().libratyRect = ModelLibraryPanel;
        }
    }

    private void ClearLibraryContainer()
    {
        int childCnt = libContainer.childCount;

        if (childCnt > 0)
            for (int i = childCnt - 1; i >= 0; i--)
            {
                //Debug.Log($"destroy 第{i}个:{libContainer.GetChild(i).name}");
                DestroyImmediate(libContainer.GetChild(i).gameObject);
            }
    }

    public void SetLibraryState()
    {
        btn_Library.interactable = false;

        Debug.Log($"isLibraryOpen:{isLibraryOpen}; {(isLibraryOpen ? 0 : ModelLibraryPanel.sizeDelta.x + 20)}");

        ModelLibraryPanel.DOAnchorPos3DX(!isLibraryOpen ? 0 : ModelLibraryPanel.sizeDelta.x + 50, tweeningDuration).OnComplete(() =>
        {
            isLibraryOpen = !isLibraryOpen;
            Debug.Log($"isLibraryOpen:{isLibraryOpen}");
            btn_Library.interactable = true;
        });
    }
}
