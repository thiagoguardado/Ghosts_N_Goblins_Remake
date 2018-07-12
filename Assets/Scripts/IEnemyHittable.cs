using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyHittable {

    void Hit(int hitDamage, Vector3 objectPosition);

}
