using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Color hoverColor;

    private GameObject towerObj;
    private Turret turret;
    private Color startColor;

    private void Start()
    {
        startColor = spriteRenderer.color;
    }

    private void OnMouseEnter()
    {
        if (UIManager.main.IsHoveringUI()) return;
        spriteRenderer.color = hoverColor;
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = startColor;
    }

    private void OnMouseDown()
    {
        if (UIManager.main.IsHoveringUI()) return;
        if (towerObj != null && turret != null){
            turret.OpenUpgradeUI();
            return;
        }
        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (LevelManager.main.spendCurrency(towerToBuild.cost))
        {
            towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
            turret = towerObj.transform.GetComponent<Turret>();
        }
    }
}
