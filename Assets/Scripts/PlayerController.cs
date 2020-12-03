using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator playerAnimator;
    public GameObject bullet;

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float slowSpeedMultiplier = 0.5f;
    [SerializeField] private float fireIntervalTime = 0.1f;

    private Vector2 moveDirection;
    private Vector2 playerSpriteSize;

    private bool isZen;
    private int shootType;
    private float fireInterval;

    private float minX;
    private float minY;
    private float maxX;
    private float maxY;


    private void Start()
    {
        // Set initial values
        moveDirection = Vector2.zero;
        fireInterval = 0;
        shootType = 0;
        isZen = false;

        CalculateCameraBoundaries();
    }

    private void Update()
    {
        // Draw line from player to moveDirection Vector
        Debug.DrawLine(gameObject.transform.position, new Vector2(
            gameObject.transform.position.x,
            gameObject.transform.position.y) + moveDirection, Color.red);
    }

    private void FixedUpdate()
    {
        GetInputAndSetZen();
        GetInputAndSetDirection();
        SetShootType();
        GetInputAndShoot();
        MovePlayer(moveDirection);
    }

    private void LateUpdate()
    {
        ClampPlayerPosition();
    }



    private void CalculateCameraBoundaries()
    {
        // Get player bounds
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerSpriteSize = new Vector2(
            spriteRenderer.sprite.bounds.size.x,
            spriteRenderer.sprite.bounds.size.y);

        // Get window bounds
        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        // Set max and min
        minX = -horzExtent;
        maxX = horzExtent;
        minY = -vertExtent;
        maxY = vertExtent;
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
    
    private void SetShootType()
    {
        if (isZen)
        {
            shootType = 1;
        }
        else
        {
            shootType = 0;
        }
    }

    private void GetInputAndShoot()
    {
        if (Input.GetKey(KeyCode.Z) && fireInterval <= 0)
        {
            // Reset interval
            fireInterval = fireIntervalTime;

            // Create new bullet and shoot
            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.position = gameObject.transform.position;
            newBullet.GetComponent<PlayerBullet>().SetImage(shootType);
        }

        // Update interval
        fireInterval = Mathf.Max(0, fireInterval - Time.deltaTime);
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

}
