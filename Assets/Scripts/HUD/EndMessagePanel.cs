using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMessagePanel : MonoBehaviour {

    public Transform lettersParent;

    private void Awake()
    {
        lettersParent.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.Level.PlayerReachedEnd += ShowPanel;
    }

    private void OnDisable()
    {
        GameEvents.Level.PlayerReachedEnd -= ShowPanel;
    }

    private void ShowPanel()
    {
        lettersParent.gameObject.SetActive(true);
    }
}
