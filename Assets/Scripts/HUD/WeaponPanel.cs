using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour {

    public Image weapon;

    private void OnEnable()
    {
        GameEvents.Player.PlayerPickedWeapon += ChangeWeapon;
    }

    private void OnDisable()
    {
        GameEvents.Player.PlayerPickedWeapon -= ChangeWeapon;
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
