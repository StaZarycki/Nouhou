using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedGainMultiplier;

    private Vector2 movement = Vector2.down;

    private float animationTime;
    private float speedGain;

    private void Start()
    {
        // Set starting animation time for tan function
        animationTime = (Mathf.PI / 2) * 0.9f;
    }

    private void Update()
    {
        animateTan();
        DrawDebugLine();
    }

    private void FixedUpdate()
    {
        gameObject.transform.Translate(movement * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Die();
        }
    }


    private void animateTan()
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
}
