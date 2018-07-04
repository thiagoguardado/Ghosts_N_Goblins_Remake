using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectScore))]
public class StaticCollectible : MonoBehaviour, IPlayerTouchable {

    public Transform spriteTransform;

    public void WasTouchedByPlayer()
    {

        // add score to game controller
        GetComponent<ObjectScore>().IncrementGameScore(spriteTransform.position);


        // destroy object
        Destroy(gameObject);

    }

}
