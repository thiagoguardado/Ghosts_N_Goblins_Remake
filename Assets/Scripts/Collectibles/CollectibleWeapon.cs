using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleWeapon : MonoBehaviour, IPlayerTouchable
{

    public PlayerWeapon playerWeapon;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = playerWeapon.weaponSprite;
    }

    public void WasTouchedByPlayer()
    {
        PlayerController.Instance.ChangeWeapon(playerWeapon);

        Destroy(gameObject);
    }

}
