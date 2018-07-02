using UnityEngine;

[CreateAssetMenu(fileName = "weapon", menuName = "Player Weapon", order = 0)]
public class PlayerWeapon : ScriptableObject {

    public Sprite weaponSprite;
    public float initialVelocity;
    public int weaponDamage;
    public int maxWeaponsOnScreen = 3;

    public WeaponBehavior weaponPrefab;


    public WeaponBehavior ShootWeapon(Vector3 SpawnPosition, LookingDirection direction, PlayerController owner)
    {
        WeaponBehavior instantiated = Instantiate(weaponPrefab, SpawnPosition, Quaternion.identity);
        instantiated.damage = weaponDamage;
        instantiated.Init(initialVelocity, direction, owner);
        return instantiated;
    }

}
