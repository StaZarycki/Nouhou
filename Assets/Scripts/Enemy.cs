using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private byte health = 1;
    [SerializeField] private float spawnTime = 1;
    [SerializeField] private GameObject bulletObject;
    [SerializeField] private GameObject powerUpObject;
    [SerializeField] private Destination[] destinations;

    private short destinationIndex = 0;
    private bool canBeDestroyed;
    private AudioClip deathAudioClip;
    private Destination currentDestination;


    private void Awake()
    {
        // Set initial values
        canBeDestroyed = false;
        deathAudioClip = Resources.Load<AudioClip>("Sounds/enep00");
        currentDestination = destinations[destinationIndex];
    }

    private void Start()
    {
        // Spawn object at spawnTime
        gameObject.SetActive(false);
        Invoke("Spawn", spawnTime);
    }

    private void Update()
    {
        // Logic for moving to destination from destinations array
        // First check if distance to current point is less than tolerance
        // If so, change destination to next target (or to the first one if it's at the end of an array)
        // Also, invoke Attack if checked
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
        // To keep object unkillable too soon
        canBeDestroyed = true;
    }

    private void OnBecameInvisible()
    {
        // Destroy object if it's away
        if (canBeDestroyed)
            Destroy(gameObject);
    }



    private void Spawn()
    {
        gameObject.SetActive(true);
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
        float _luckyRoll = Random.Range(0f, 1f);

        // 30% chance to create power up
        if (_luckyRoll <= 0.3f)
            Instantiate(powerUpObject).transform.position = transform.position;

        AudioSource.PlayClipAtPoint(deathAudioClip, transform.position);
        Destroy(gameObject);
    }

    private void Attack()
    {
        Instantiate(bulletObject).transform.position = transform.position;
    }

    private void UpdateHealth(byte _health)
    {
        health = _health;
        if (health == 0)
            Die();
    }

    public void TakeDamage(byte damage)
    {
        UpdateHealth(System.Convert.ToByte(Mathf.Max(health - damage, 0)));
    }

}
