using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform startPoint;
    public Transform[] path;

    [Header("Attributes")]
    [SerializeField]
    public int currency = 100;

    private void Awake()
    {
        main = this;
    }

    public void increaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool spendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough money");
            return false;
        }
    }
}
