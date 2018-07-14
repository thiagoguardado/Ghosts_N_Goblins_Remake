using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageStartOrEndAction : MonoBehaviour {

    public enum RoutinePosition { Start, End }

    public RoutinePosition routinePosition;

    // Use this for initialization
    void Awake()
    {
        LevelController.Instance.SetupLevelRoutine(routinePosition, Routine);
    }

    protected abstract IEnumerator Routine();
}
