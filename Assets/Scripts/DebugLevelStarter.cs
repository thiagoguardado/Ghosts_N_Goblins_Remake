using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLevelStarter : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {

        if (LevelController.Instance.InLevel)
        {
            Destroy(gameObject);
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                LevelController.Instance.StartLevel(true);
            }
        }


	}
}
