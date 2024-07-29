using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Tower[] towers;
    private int selectedTower = 0;
    public static BuildManager main;

    private void Awake()
    {
        main = this;
    }

    public Tower GetSelectedTower()
    {
        return towers[selectedTower];
    }

    public void SetSelectedTower(int selectedTower) { 
        this.selectedTower = selectedTower;
    }
}
