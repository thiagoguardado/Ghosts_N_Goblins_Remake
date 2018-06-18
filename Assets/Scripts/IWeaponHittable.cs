using UnityEngine;

public interface IWeaponHittable {

    void Hit(int damageTaken, Vector2 hitPoint);

}
