﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private float maxSpeed;

    private Vector2 movement = Vector2.down;
    private AudioClip pickUpAudioClip;

    private float speedGainMultiplier;
    private float animationTime;
    private float speedGain;

    private void Awake()
    {
        // Set initial values
        pickUpAudioClip = Resources.Load<AudioClip>("Sounds/item00");
        speedGainMultiplier = 10;
    }

    public virtual void Start()
    {
        // Set starting animation time for tan function
        animationTime = (Mathf.PI / 2) * 0.9f;
    }

    private void Update()
    {
        AnimateTan();
        DrawDebugLine();
    }

    private void OnBecameInvisible()
    {
        Die();
    }

    private void FixedUpdate()
    {
        gameObject.transform.Translate(movement * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Activate pickup and destroy on collision with player
        if (collision.CompareTag("Player"))
        {
            OnPlayerPickUp(collision);
            Die();
        }
    }



    private void AnimateTan()
    {
        // Make a tan of time function for animation and clamp it to maxSpeed
        animationTime = Mathf.Min(animationTime + Time.time * 0.01f, Mathf.PI * 0.99f);
        speedGain = Mathf.Tan((animationTime - Mathf.PI / 2)) * speedGainMultiplier;
        movement = Vector2.ClampMagnitude(Vector2.down * speedGain, maxSpeed);
    }

    private void DrawDebugLine()
    {
        // Draw line from PowerUp to movement Vector
        Debug.DrawLine(gameObject.transform.position, new Vector2(
            gameObject.transform.position.x,
            gameObject.transform.position.y) + movement, new Color(0.8f, 0.5f, 0.5f));
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public virtual void OnPlayerPickUp(Collider2D playerCollider, bool debugMode = false)
    {
        if (debugMode)
            Debug.Log(gameObject.name + ": picked up");

        AudioSource.PlayClipAtPoint(pickUpAudioClip, transform.position);
    }
}
