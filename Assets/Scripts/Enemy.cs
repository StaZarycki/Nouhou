using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private byte health = 1;
    [SerializeField] private GameObject bulletObject;
    [SerializeField] private Destination[] destinations;

    private short destinationIndex = 0;
    private bool canBeDestroyed;
    private Destination currentDestination;


    private void Awake()
    {
        // Set initial values
        canBeDestroyed = false;
        currentDestination = destinations[destinationIndex];
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, currentDestination.Position) < currentDestination.Tolerance)
        {
            if (currentDestination.AttackOnFinish)
                Invoke("Attack", 0);
            if (destinations.Length > destinationIndex + 1)
            {
                destinationIndex += 1;
                currentDestination = destinations[destinationIndex];
            }
            else
            {
                destinationIndex = 0;
                currentDestination = destinations[destinationIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        MoveToDestination(currentDestination.Position, currentDestination.Speed);
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



    private void MoveToDestination(Vector3 destination, float speed)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            speed * 0.1f * Vector3.Distance(transform.position, destination));
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
        Instantiate(bulletObject).transform.position = transform.position;
    }

    public void TakeDamage(byte damage)
    {
        health -= damage;
        UpdateHealth();
    }

}
