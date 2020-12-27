using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : PickUp
{
    [SerializeField] private bool isBig;

    public override void Start()
    {
        base.Start();

        // Make bigger if isBig (duh)
        if (isBig)
        {
            gameObject.transform.localScale = new Vector2(1.5f, 1.5f);
        }
    }

    public override void OnPlayerPickUp(Collider2D playerCollider, bool debugMode = false)
    {
        base.OnPlayerPickUp(playerCollider, debugMode);

        // Add power to player
        playerCollider.gameObject.GetComponent<PlayerController>().AddMorePower(isBig ? (byte) 10 : (byte) 1);
    }
}
