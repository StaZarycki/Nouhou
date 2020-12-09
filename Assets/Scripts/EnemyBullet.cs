using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage();
            Die();
        }
    }


    private void Move()
    {
        transform.Translate(Vector2.down * bulletSpeed * Time.deltaTime, Space.World);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
