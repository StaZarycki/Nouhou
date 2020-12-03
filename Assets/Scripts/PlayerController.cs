using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator playerAnimator;
    public GameObject bullet;

    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float slowSpeedMultiplier = 0.5f;
    [SerializeField] float fireIntervalTime = 0.1f;

    GameObject player;
    Vector2 moveDirection;
    float fireInterval;
    bool isZen;
    int shootType;

    private float minX;
    private float minY;
    private float maxX;
    private float maxY;

    private Vector2 spriteSize;

    void Start()
    {
        player = gameObject;
        spriteSize = new Vector2(
            player.GetComponent<SpriteRenderer>().sprite.bounds.size.x,
            player.GetComponent<SpriteRenderer>().sprite.bounds.size.y);

        moveDirection = Vector2.zero;
        fireInterval = 0;
        isZen = false;
        shootType = 0;

        calculateCameraBoundaries();
    }

    void Update()
    {
        // Draw line from player to moveDirection Vector
        Debug.DrawLine(player.transform.position, new Vector2(
            player.transform.position.x,
            player.transform.position.y) + moveDirection, Color.red);
    }

    void FixedUpdate()
    {
        getInputAndSetZen();
        getInputAndSetDirection();
        setShootType();
        getInputAndShoot();
        movePlayer(moveDirection);
    }

    void LateUpdate()
    {
        clampPlayerPosition();
    }



    void calculateCameraBoundaries()
    {
        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        minX = -horzExtent;
        maxX = horzExtent;
        minY = -vertExtent;
        maxY = vertExtent;
    }

    void getInputAndSetDirection()
    {
        moveDirection = Vector2.zero;

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

        moveDirection = Vector2.ClampMagnitude(moveDirection, 1);

        if (isZen)
            moveDirection *= slowSpeedMultiplier;
    }

    void getInputAndSetZen()
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
    
    void setShootType()
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

    void getInputAndShoot()
    {
        if (Input.GetKey(KeyCode.Z) && fireInterval <= 0)
        {
            fireInterval = fireIntervalTime;

            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.position = player.transform.position;
            newBullet.GetComponent<PlayerBullet>().SetImage(shootType);
        }

        fireInterval = Mathf.Max(0, fireInterval - Time.deltaTime);
    }

    void movePlayer(Vector2 moveDirection)
    {
        player.transform.Translate(moveDirection * movementSpeed * Time.deltaTime, Space.Self);
    }

    void clampPlayerPosition()
    {
        player.transform.position = new Vector2(
            Mathf.Clamp(player.transform.position.x, minX + spriteSize.x / 2, maxX - spriteSize.x / 2),
            Mathf.Clamp(player.transform.position.y, minY + spriteSize.y / 2, maxY - spriteSize.y / 2));
    }

}
