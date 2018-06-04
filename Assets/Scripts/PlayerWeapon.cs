using UnityEngine;

[CreateAssetMenu(fileName = "weapon", menuName = "Player Weapon", order = 0)]
public class PlayerWeapon : ScriptableObject {

    public Sprite weaponSprite;
    public float initialVelocity;
    public float weaponDamage;

    public WeaponBehavior weaponPrefab;

}
