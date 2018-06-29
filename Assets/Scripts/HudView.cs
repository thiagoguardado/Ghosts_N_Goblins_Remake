using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudView : MonoBehaviour {

    
    public Text lifes;
    public Image weapon;


    private void OnEnable()
    {
        GameEvents.PlayerPickedWeapon += ChangeWeapon;
    }

    private void OnDisable()
    {
        GameEvents.PlayerPickedWeapon -= ChangeWeapon;
    }

    private void Start()
    {
        // initial setup
        ChangeWeapon();

    }


    // change current weapon being displayed
    private void ChangeWeapon()
    {
        weapon.sprite = PlayerController.Instance.CurrentWeapon.weaponSprite;
    }


}
