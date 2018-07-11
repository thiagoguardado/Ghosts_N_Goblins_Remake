using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class WinningConditionEnemy : MonoBehaviour {

    private void Awake()
    {
        LevelController.Instance.AddEnemyToWinningCOnditionList(GetComponent<Enemy>());
    }

    private void OnDestroy()
    {
        LevelController.Instance.RemoveEnemyToWinningCOnditionList(GetComponent<Enemy>());
    }
}
