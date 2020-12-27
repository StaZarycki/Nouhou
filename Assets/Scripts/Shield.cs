using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : PickUp
{
    public override void OnPlayerPickUp(Collider2D playerCollider, bool debugMode = false)
    {
        base.OnPlayerPickUp(playerCollider, debugMode);

        playerCollider.gameObject.GetComponent<PlayerController>().GainShield();
    }
}
