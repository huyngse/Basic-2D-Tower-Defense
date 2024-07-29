using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isMouseOver = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        UIManager.main.SetHoveringState(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        UIManager.main.SetHoveringState(false);
        gameObject.SetActive(false);
    }
}
