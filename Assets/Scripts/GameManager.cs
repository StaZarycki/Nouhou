using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject healthPanelObject;
    [SerializeField] private GameObject bombPanelObject;
    [SerializeField] private GameObject playerObject;

    private Image[] healthStarsObjects;
    private Image[] bombStarsObjects;
    private PlayerController playerController;

    public byte playerHealth = 4;
    public byte playerBombs = 4;
    public byte playerPower;


    private void Awake()
    {
        healthStarsObjects = healthPanelObject.GetComponentsInChildren<Image>();
        bombStarsObjects = bombPanelObject.GetComponentsInChildren<Image>();
        playerController = playerObject.GetComponent<PlayerController>();
        playerPower = playerController.GetPower();
    }

    private void Start()
    {
        UpdateHealth(playerHealth);
        UpdateBombs(playerBombs);
    }


    public void UpdateHealth(byte health)
    {
        playerHealth = health;

        // Set game over and return if health is 0
        if (playerHealth <= 0)
        {
            Debug.Log("Game Over!");
            return;
        }

        // Set all stars to invisible
        foreach(Image star in healthStarsObjects)
        {
            star.enabled = false;
        }

        // Set necessary stars to visible
        for (byte i = 0; i <= playerHealth; i++)
        {
            try
            {
                healthStarsObjects[i].enabled = true;
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.Log(e);
            }
        }
    }

    public void UpdateBombs(byte bombs)
    {
        playerBombs = bombs;

        if (playerBombs < 0)
        {
            playerBombs = 0;
            return;
        }

        // Set all stars to invisible
        foreach (Image star in bombStarsObjects)
        {
            star.enabled = false;
        }

        // Set necessary stars to visible
        for (byte i = 0; i <= playerBombs; i++)
        {
            try
            {
                bombStarsObjects[i].enabled = true;
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.Log(e);
            }
        }
    }
}
