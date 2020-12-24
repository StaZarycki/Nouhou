using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage();
            Die();
        }
    }


    public virtual void Move(Vector2 direction)
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime, Space.World);
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
