using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleArmor : MonoBehaviour, IPlayerTouchable {


    public void WasTouchedByPlayer()
    {
        PlayerController.Instance.GetArmor();

        Destroy(gameObject);
    }

}
