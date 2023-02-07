using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggingItem : MonoBehaviour
{
    [HideInInspector] public GameObject itemModel;
    public RawImage rawImage;
    public CanvasGroup canvasGroup;
    public Text prefName;
}
