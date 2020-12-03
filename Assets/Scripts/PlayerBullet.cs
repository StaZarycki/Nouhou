using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float animationSpeed;
    [SerializeField] float bulletSpeed;
    [SerializeField] Sprite[] bulletSprites = new Sprite[4];
    
    GameObject bullet;
    Transform bulletTransform;
    SpriteRenderer spriteRenderer;
    float animationTimer;

    void Awake()
    {
        bullet = gameObject;
        spriteRenderer = bullet.GetComponent<SpriteRenderer>();
        bulletTransform = bullet.GetComponent<Transform>();
    }

    void Start()
    {
        animationTimer = 0;
    }

    void Update()
    {
        rotate();
    }

    void FixedUpdate()
    {
        move();
    }

    void OnBecameInvisible()
    {
        die();
    }

    void rotate()
    {
        animationTimer += Time.deltaTime;

        if (animationTimer >= 1 / animationSpeed)
        {
            bulletTransform.Rotate(new Vector3(0, 0, -10));
            animationTimer = 0;
        }
    }

    void move()
    {
        bulletTransform.Translate(Vector2.up * bulletSpeed * Time.deltaTime, Space.World);
    }

    void die()
    {
        Destroy(this.gameObject);
    }

    public void SetImage(int spriteIndex)
    {
        spriteRenderer.sprite = bulletSprites[spriteIndex];
    }

}
