using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    [SerializeField] private Sprite[] bulletSprites = new Sprite[4];

    [SerializeField] private float animationSpeed;
    
    private Transform bulletTransform;
    private SpriteRenderer spriteRenderer;

    private float animationTimer;


    private void Awake()
    {
        // Set initial values
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        bulletTransform = gameObject.GetComponent<Transform>();
        animationTimer = 0;
    }

    private void Update()
    {
        Rotate();
    }

    private void FixedUpdate()
    {
        Move(Vector2.up);
    }

    private void OnBecameInvisible()
    {
        Die();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
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

    public void SetImage(int spriteIndex)
    {
        spriteRenderer.sprite = bulletSprites[spriteIndex];
    }

}
