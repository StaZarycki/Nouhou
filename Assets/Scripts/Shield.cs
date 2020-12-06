using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : PickUp
{
    public override void OnPlayerPickUp(Collider2D playerCollider)
    {
        base.OnPlayerPickUp(playerCollider);

        playerCollider.gameObject.GetComponent<PlayerController>().GainShield();
    }
}
