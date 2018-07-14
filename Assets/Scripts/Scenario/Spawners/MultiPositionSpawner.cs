using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPositionSpawner : MonoBehaviour {

    [System.Serializable]
    public class PositionsAndPrefabs {
        public string name;
        public Transform[] positions;
        public GameObject prefab;
        public bool isWinningCondition = false;
        public ObjectDestroyedTriggeredAction triggerWhenDefeated;
    }

    public List<PositionsAndPrefabs> positionsToSpawn;


    void Awake()
    {

        for (int i = 0; i < positionsToSpawn.Count; i++)
        {
            for (int j = 0; j < positionsToSpawn[i].positions.Length; j++)
            {
                GameObject go = Instantiate(positionsToSpawn[i].prefab, positionsToSpawn[i].positions[j].position, Quaternion.identity, positionsToSpawn[i].positions[j]);

                if (positionsToSpawn[i].isWinningCondition)
                {
                    go.AddComponent<WinningConditionEnemy>();
                }

                if (positionsToSpawn[i].triggerWhenDefeated != null)
                {
                    TriggerWhenDetroy twd = go.AddComponent<TriggerWhenDetroy>();
                    twd.SetupTrigger(positionsToSpawn[i].triggerWhenDefeated);
                }

            }
        }

    }
}
