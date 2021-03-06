﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Animator playerAnimator;
    public GameObject bullet;
    public GameObject gameManagerObject;

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float slowSpeedMultiplier = 0.5f;
    [SerializeField] private float fireIntervalTime = 0.1f;

    private Vector2 moveDirection;
    private Vector2 playerSpriteSize;
    private AudioClip shootAudioClip;
    private GameManager gameManager;

    private bool isZen;
    private bool isVulnerable;
    private byte shootType;
    private float fireInterval;
    private byte power;

    private float minX;
    private float minY;
    private float maxX;
    private float maxY;


    private void Awake()
    {
        // Set initial values
        moveDirection = Vector2.zero;
        gameManager = gameManagerObject.GetComponent<GameManager>();
        shootAudioClip = Resources.Load<AudioClip>("Sounds/plst00");
        isZen = false;
        isVulnerable = true;
        shootType = 0;
        fireInterval = 0;
        power = 4;
    }


    private void Start()
    {
        CalculateCameraBoundaries();
        SetPlayerPositionToCenter();
    }

    private void Update()
    {
        // Get input
        GetInputAndSetZen();
        GetInputAndSetDirection();
        GetInputAndShoot();

        // Draw line from player to moveDirection Vector
        Debug.DrawLine(gameObject.transform.position, new Vector2(
            gameObject.transform.position.x,
            gameObject.transform.position.y) + moveDirection, Color.red);

        // Restart level on R
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer(moveDirection);
    }

    private void LateUpdate()
    {
        ClampPlayerPosition();
    }


    private void SetPlayerPositionToCenter()
    {
        transform.position = new Vector2(
            (minX + maxX) / 2,
            minY + 1);
    }

    private void CalculateCameraBoundaries()
    {
        // Get player bounds
        SpriteRenderer playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerSpriteSize = new Vector2(
            playerSpriteRenderer.sprite.bounds.size.x,
            playerSpriteRenderer.sprite.bounds.size.y);

        // Get window bounds
        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        // Set max and min (based on background image size and window resolution)
        minX = -horzExtent + horzExtent * (32f/640f)*2;
        maxX = horzExtent - horzExtent * (224f/640f)*2;
        minY = -vertExtent + vertExtent * (16f/480f)*2;
        maxY = vertExtent - vertExtent * (16f/480f)*2;
    }

    private void GetInputAndSetZen()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isZen = true;
        }
        else
        {
            isZen = false;
        }
    }

    private void GetInputAndSetDirection()
    {
        // Set vector to zero to prevent moving without pressing any key
        moveDirection = Vector2.zero;

        // I'm not really proud of this part, but well
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection += Vector2.left;
            playerAnimator.SetBool("MoveLeft", true);

            if (Input.GetKey(KeyCode.RightArrow))
            {
                playerAnimator.SetBool("MoveLeft", false);
            }
        }
        else
        {
            playerAnimator.SetBool("MoveLeft", false);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection += Vector2.right;
            playerAnimator.SetBool("MoveRight", true);

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                playerAnimator.SetBool("MoveRight", false);
            }
        }
        else
        {
            playerAnimator.SetBool("MoveRight", false);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection += Vector2.down;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection += Vector2.up;
        }

        // Set maximum speed to 1
        moveDirection = Vector2.ClampMagnitude(moveDirection, 1);

        // Slow down if in zen mode
        if (isZen)
            moveDirection *= slowSpeedMultiplier;
    }

    private void GetInputAndShoot()
    {
        if (Input.GetKey(KeyCode.Z) && fireInterval <= 0)
        {
            // Reset interval
            fireInterval = fireIntervalTime;

            // Play sound
            AudioSource.PlayClipAtPoint(shootAudioClip, transform.position);

            // Create new bullet/s and shoot (based on shootType)
            GameObject[] newBullets = new GameObject[6];

            for (byte i = 0; i < shootType + 1; i++)
            {
                newBullets[i] = Instantiate(bullet);
                newBullets[i].transform.position = gameObject.transform.position;
                newBullets[i].GetComponent<PlayerBullet>().SetImage(isZen ? 1 : 0);
            }

            // Transform bullets to make shapes
            switch (shootType)
            {
                case 1:
                    newBullets[0].transform.Translate(Vector2.left * 0.2f, Space.Self);
                    newBullets[1].transform.Translate(Vector2.right * 0.2f, Space.Self);
                    break;
                case 2:
                    newBullets[0].transform.Translate(Vector2.left * 0.3f, Space.Self);
                    newBullets[1].transform.Translate(Vector2.right * 0.3f, Space.Self);
                    break;
                case 3:
                    newBullets[0].transform.Translate(new Vector2(0.2f, 0.2f), Space.Self);
                    newBullets[1].transform.Translate(new Vector2(0.2f, -0.2f), Space.Self);
                    newBullets[2].transform.Translate(new Vector2(-0.2f, 0.2f), Space.Self);
                    newBullets[3].transform.Translate(new Vector2(-0.2f, -0.2f), Space.Self);
                    break;
                case 4:
                    newBullets[0].transform.Translate(new Vector2(0.25f, 0.25f), Space.Self);
                    newBullets[1].transform.Translate(new Vector2(0.25f, -0.25f), Space.Self);
                    newBullets[2].transform.Translate(new Vector2(-0.25f, 0.25f), Space.Self);
                    newBullets[3].transform.Translate(new Vector2(-0.25f, -0.25f), Space.Self);
                    break;
                case 5:
                    newBullets[0].transform.Translate(new Vector2(-0.2f, 0.25f), Space.Self);
                    newBullets[1].transform.Translate(new Vector2(-0.2f, 0), Space.Self);
                    newBullets[2].transform.Translate(new Vector2(-0.2f, -0.25f), Space.Self);
                    newBullets[3].transform.Translate(new Vector2(0.2f, 0.25f), Space.Self);
                    newBullets[4].transform.Translate(new Vector2(0.2f, 0), Space.Self);
                    newBullets[5].transform.Translate(new Vector2(0.2f, -0.25f), Space.Self);
                    break;
                default:
                    break;
            }
        }

        // Update interval
        fireInterval = Mathf.Max(0, fireInterval - Time.deltaTime);
    }
    
    private void SetShootType()
    {
        shootType = (byte) Mathf.Ceil(power / 20);
        Debug.Log("Current power: " + power);
        Debug.Log("Shoot type: " + shootType);
    }

    private void MovePlayer(Vector2 moveDirection)
    {
        gameObject.transform.Translate(moveDirection * movementSpeed * Time.deltaTime, Space.Self);
    }

    private void ClampPlayerPosition()
    {
        gameObject.transform.position = new Vector2(
            Mathf.Clamp(gameObject.transform.position.x, minX + playerSpriteSize.x / 2, maxX - playerSpriteSize.x / 2),
            Mathf.Clamp(gameObject.transform.position.y, minY + playerSpriteSize.y / 2, maxY - playerSpriteSize.y / 2));
    }

    public void AddMorePower(byte powerToAdd)
    {
        power += powerToAdd;
        SetShootType();
    }

    public void GainShield()
    {
        // TODO: Gain shield
        StartCoroutine("ShieldTimer");
    }

    public void TakeDamage()
    {
        if (isVulnerable)
        {
            // Deal damage when is vulnerable
            // TODO: Make player disappear for a moment and respawn
            gameManager.UpdateHealth(Convert.ToByte(Mathf.Max(gameManager.playerHealth - 1, 0)));
        }
    }

    public byte GetPower()
    {
        return power;
    }


    private IEnumerator ShieldTimer()
    {
        Debug.Log("Start of ShieldTimer()");
        isVulnerable = false;
        yield return new WaitForSeconds(5);
        isVulnerable = true;
        Debug.Log("Shield lost!");
    }

}
