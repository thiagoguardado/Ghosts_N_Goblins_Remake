using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    private bool finished = false;
    public bool Finished { get { return finished; } }

    public float timeBeforeFall;
    public float timeFalling;
    public Transform keySprite;

    private void Start()
    {
        StartKeyRoutine();
    }

    void StartKeyRoutine()
    {
        StartCoroutine(KeyRoutine(() => FinishRoutine()));
    }

    void FinishRoutine()
    {
        finished = true;
    }

    IEnumerator KeyRoutine(Action actionWhenDone)
    {
        yield return new WaitForSeconds(timeBeforeFall);

        float timer = 0f;
        float startY = keySprite.localPosition.y;
        while (timer <= timeFalling)
        {
            float posY = Mathf.Lerp(startY, 0, timer / timeFalling);
            keySprite.localPosition = new Vector3(0, posY, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        actionWhenDone.Invoke();
    }

}
