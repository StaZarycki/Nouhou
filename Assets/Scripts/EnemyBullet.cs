using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void FixedUpdate()
    {
        Move(Vector2.down);
    }

}
