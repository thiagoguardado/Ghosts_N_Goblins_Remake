using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VFXSpriteAnimation : MonoBehaviour {

    public enum InstantiationPoint
    {
        WeaponHit,
        EnemyTransform
    }

    public InstantiationPoint instantiationPoint;

    public void Destroy()
    {
        Destroy(gameObject);
    }


}
