using UnityEngine;

[CreateAssetMenu(fileName = "weapon", menuName = "Player Weapon", order = 0)]
public class PlayerWeapon : ScriptableObject {

    public Sprite weaponSprite;
    public float initialVelocity;
    public int weaponDamage;

    public WeaponBehavior weaponPrefab;


    public void ShootWeapon(Vector3 SpawnPosition, LookingDirection direction)
    {
  
        var instantiated = Instantiate(weaponPrefab, SpawnPosition, Quaternion.identity);
        instantiated.damage = weaponDamage;
        instantiated.Shoot(initialVelocity, direction);

    }

}
