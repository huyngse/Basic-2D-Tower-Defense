using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    private int hitPoints = 2;

    [SerializeField]
    private int currencyWorth = 20;
    private bool isDestroyed = false;

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.increaseCurrency(currencyWorth);
            Destroy(gameObject);
        }
    }
}
