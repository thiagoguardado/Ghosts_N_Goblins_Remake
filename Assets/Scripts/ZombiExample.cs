using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombiExample : Enemy, IWeaponHittable
{
    public Text heltText;


    protected override void Update()
    {
        base.Update();


        heltText.text = "Health: " + health.ToString();

    }

}
