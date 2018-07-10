using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPositionSpawner : MonoBehaviour {

    [System.Serializable]
    public class PositionsAndPrefabs {
        public string name;
        public Transform[] positions;
        public GameObject prefab;
    }

    public List<PositionsAndPrefabs> positionsToSpawn;


    void Awake()
    {

        for (int i = 0; i < positionsToSpawn.Count; i++)
        {
            for (int j = 0; j < positionsToSpawn[i].positions.Length; j++)
            {
                Instantiate(positionsToSpawn[i].prefab, positionsToSpawn[i].positions[j].position, Quaternion.identity, positionsToSpawn[i].positions[j]);
            }
        }

    }
}
