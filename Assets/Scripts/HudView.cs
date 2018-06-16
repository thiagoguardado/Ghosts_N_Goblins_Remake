using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudView : MonoBehaviour {

    public Text time;
    public Text score;
    public Text lifes;
    public Image weapon;


    private void OnEnable()
    {
        GameEvents.PlayerPickedWeapon += ChangeWeapon;
        GameEvents.PlayerDied += UpdateLifes;
        GameEvents.LevelStarted += UpdateLifes;
        GameEvents.ScoreIncremented += IncrementScore;
    }

    private void OnDisable()
    {
        GameEvents.PlayerPickedWeapon -= ChangeWeapon;
        GameEvents.PlayerDied -= UpdateLifes;
        GameEvents.LevelStarted -= UpdateLifes;
        GameEvents.ScoreIncremented += IncrementScore;
    }

    private void Start()
    {
        // initial setup
        ChangeWeapon();
        UpdateTimer();
        UpdateLifes();

    }

    private void Update()
    {

        // update timer
        UpdateTimer();

    }

    // update timer display
    private void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(GameController.Instance.Timer / 60);
        int seconds = Mathf.CeilToInt(GameController.Instance.Timer % 60);
        if (seconds == 60)
        {
            minutes += 1;
            seconds = 0;
        }

        time.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }


    // update life display
    private void UpdateLifes()
    {
        lifes.text = GameController.Instance.Lifes.ToString();
    }

    // update life display
    private void IncrementScore()
    {
        score.text = GameController.Instance.Score.ToString();
    }


    // change current weapon being displayed
    private void ChangeWeapon()
    {
        weapon.sprite = PlayerController.Instance.CurrentWeapon.weaponSprite;
    }


}
