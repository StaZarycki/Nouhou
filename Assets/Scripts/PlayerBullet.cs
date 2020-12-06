using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private Sprite[] bulletSprites = new Sprite[4];

    [SerializeField] private float animationSpeed;
    [SerializeField] private float bulletSpeed;
    
    private GameObject bullet;
    private Transform bulletTransform;
    private SpriteRenderer spriteRenderer;

    private float animationTimer;


    private void Awake()
    {
        // Set initial values
        bullet = gameObject;
        spriteRenderer = bullet.GetComponent<SpriteRenderer>();
        bulletTransform = bullet.GetComponent<Transform>();
    }

    private void Start()
    {
        animationTimer = 0;
    }

    private void Update()
    {
        Rotate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnBecameInvisible()
    {
        Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(1);
            Die();
        }
    }



    private void Rotate()
    {
        // Set timer for animation
        animationTimer += Time.deltaTime;

        if (animationTimer >= 1 / animationSpeed)
        {
            bulletTransform.Rotate(new Vector3(0, 0, -10));
            animationTimer = 0;
        }
    }

    private void Move()
    {
        bulletTransform.Translate(Vector2.up * bulletSpeed * Time.deltaTime, Space.World);
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    public void SetImage(int spriteIndex)
    {
        spriteRenderer.sprite = bulletSprites[spriteIndex];
    }

}
