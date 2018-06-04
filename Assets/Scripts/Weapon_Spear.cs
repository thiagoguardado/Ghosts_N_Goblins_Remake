using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Spear : WeaponBehavior {

    private float speed;

    public override void Shoot(float shootSpeed)
    {
        speed = shootSpeed;
    }

    protected override void CollidedWith(GameObject go)
    {
        Debug.Log(go.tag);
    }

    protected override void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }


}
