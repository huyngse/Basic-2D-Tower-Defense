using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform turretRotationPoint;

    [SerializeField]
    private LayerMask enemyMask;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform firingPoint;

    [SerializeField]
    private GameObject upgradeUI;

    [SerializeField]
    private Button upgradeButton;

    [Header("Attributes")]
    [SerializeField]
    private float targetingRange = 5f;

    [SerializeField]
    private float rotationSpeed = 200f;

    [SerializeField]
    private float bps = 1f;

    [SerializeField]
    private int baseUpgradeCost = 120;
    private Transform target;
    private float timeUntilFire;
    private int level = 1;
    private float baseBps;
    private float baseTargetingRange;

    private void RotateTowardsTarget()
    {
        float angle =
            Mathf.Atan2(
                target.position.y - transform.position.y,
                target.position.x - transform.position.x
            ) * Mathf.Rad2Deg
            - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(
            turretRotationPoint.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void Shoot()
    {
        GameObject bulletOjb = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletOjb.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }

    private void Start()
    {
        baseBps = bps;
        baseTargetingRange = targetingRange;
        upgradeButton.onClick.AddListener(Upgrade);
    }

    private void Update()
    {
        timeUntilFire += Time.deltaTime;
        if (target == null)
        {
            FindTarget();
            return;
        }
        RotateTowardsTarget();
        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0;
            }
        }
    }

    private void FindTarget()
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
            target = hits[0].transform;
        }
    }

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    private int CalculateUpgradeCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private float CalculateBps()
    {
        return baseBps * Mathf.Pow(level, 0.5f);
    }

    private float CalculateTargetingRange()
    {
        return baseTargetingRange * Mathf.Pow(level, 0.3f);
    }

    public void Upgrade()
    {
        if (LevelManager.main.spendCurrency(CalculateUpgradeCost()))
        {
            level++;
            bps = CalculateBps();
            targetingRange = CalculateTargetingRange();
            CloseUpgradeUI();
        }
    }
}
