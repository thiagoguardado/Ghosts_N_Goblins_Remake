using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectScore))]
public class Coin : MonoBehaviour, IPlayerTouchable {

    public Transform coinSprite;

    public bool play = false;


    private void Update()
    {
        if (play)
        {
            WasTouchesByPlayer();
            play = false;
        }
    }

    public void WasTouchesByPlayer()
    {

        // add score to game controller
        GetComponent<ObjectScore>().IncrementGameScore(coinSprite.position);


        // destroy object
        Destroy(gameObject);

    }



}
