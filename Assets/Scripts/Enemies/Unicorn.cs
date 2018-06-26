using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unicorn : Enemy {

    public Transform scoreAnchor;


	protected override void Kill()
	{
        // add score to game controller
        GetComponent<ObjectScore>().IncrementGameScore(scoreAnchor.position);

        // kill object
        Destroy(gameObject);
	}

}
