using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField]
    private float bulletSpeed = 5f;
     [SerializeField]
    private int damage = 1;

    [SerializeField]
    private float lifeTime = 3f;
    private Transform target;
    private float timeSinceShot;

    void Start() { }

    void Update()
    {
        timeSinceShot += Time.deltaTime;
        if (timeSinceShot >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void FixedUpdate()
    {
        if (!target)
            return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<Health>().TakeDamage(damage);
        Destroy(gameObject);
    }
}
