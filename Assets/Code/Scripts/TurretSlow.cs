using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TurretSlow : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField]
    private float targetingRange = 5f;

    [SerializeField]
    private float attackSpeed = 1 / 2f;

    [SerializeField]
    private float freezeTime = 1;
    private float timeUntilFire;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }

    private void Shoot()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            transform.position,
            targetingRange,
            transform.position,
            0f,
            enemyMask
        );
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                EnemyMovement enemyMovement = hit.transform.GetComponent<EnemyMovement>();
                enemyMovement.SetSpeed(0.25f);
                StartCoroutine(ResetEnemySpeed(enemyMovement));
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement enemyMovement)
    {
        yield return new WaitForSeconds(freezeTime);
        enemyMovement.ResetSpeed();
    }

    private void Update()
    {
        timeUntilFire += Time.deltaTime;
        if (timeUntilFire >= 1f / attackSpeed)
        {
            Shoot();
            timeUntilFire = 0;
        }
    }
}
