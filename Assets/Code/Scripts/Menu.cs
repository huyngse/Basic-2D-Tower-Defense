using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField]
    private TextMeshProUGUI currencyUI;

    [SerializeField]
    private Animator animator;
    private bool isMenuOpen = false;

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        animator.SetBool("MenuOpen", isMenuOpen);
    }

    private void OnGUI()
    {
        if (currencyUI != null)
        {
            currencyUI.text = LevelManager.main.currency.ToString() + "$";
        }
    }

    public void SetSelected() { }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.main.SetHoveringState(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.main.SetHoveringState(true);
    }
}
