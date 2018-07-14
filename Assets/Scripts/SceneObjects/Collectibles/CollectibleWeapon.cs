using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleWeapon : MonoBehaviour, IPlayerTouchable
{

    public PlayerWeapon playerWeapon;


    public void WasTouchedByPlayer()
    {
        PlayerController.Instance.ChangeWeapon(playerWeapon);

        Destroy(gameObject);
    }

}
