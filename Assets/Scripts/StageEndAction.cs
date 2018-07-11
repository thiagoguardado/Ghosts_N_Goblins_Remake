using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageEndAction : MonoBehaviour {

    // Use this for initialization
    void Awake()
    {
        LevelController.Instance.SetupLevelRoutine(LevelEnd);
    }

    protected abstract IEnumerator LevelEnd();
}
