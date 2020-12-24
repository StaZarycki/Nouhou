using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector2 destination;

    [SerializeField] private byte health = 1;
    [SerializeField] private float speed = 0.1f;

    private bool canBeDestroyed;


    private void Awake()
    {
        // Set initial values
        canBeDestroyed = false;
    }

    private void FixedUpdate()
    {
        MoveToDestination();
    }

    private void OnBecameVisible()
    {
        canBeDestroyed = true;
    }

    private void OnBecameInvisible()
    {
        if (canBeDestroyed)
            Die();
    }



    private void MoveToDestination()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            speed * Mathf.Abs(transform.position.magnitude - destination.magnitude));
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void UpdateHealth()
    {
        if (health <= 0)
            Die();
    }

    private void Attack()
    {
        // TODO: Attack (duh)
    }

    public void TakeDamage(byte damage)
    {
        health -= damage;
        UpdateHealth();
    }

}
