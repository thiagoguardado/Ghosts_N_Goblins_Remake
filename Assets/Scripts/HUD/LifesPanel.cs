using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifesPanel : MonoBehaviour {

    public Sprite lifeSprite;
    public Image[] imageUnits;

    private void OnEnable()
    {
        GameEvents.Level.LevelStarted += UpdateLifes;
        GameEvents.Player.PlayerGetsExtraLife += UpdateLifes;
    }

    private void OnDisable()
    {
        GameEvents.Level.LevelStarted -= UpdateLifes;
        GameEvents.Player.PlayerGetsExtraLife -= UpdateLifes;
    }

    private void Start()
    {
        // initial setup
        UpdateLifes();

    }

    public void UpdateLifes()
    {
        TurnOffAll();

        for (int i = 0; i < LevelController.Instance.Lifes; i++)
        {
            TurnOn(imageUnits[i]);
        }
    }


    private void TurnOffAll()
    {
        for (int i = 0; i < imageUnits.Length; i++)
        {
            imageUnits[i].enabled = false;
        }
    }

    private void TurnOn(Image imageUnit)
    {
        imageUnit.enabled = true;
        imageUnit.sprite = lifeSprite;
    }
}
