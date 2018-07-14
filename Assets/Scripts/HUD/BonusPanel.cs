using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPanel : HUDStringDisplay
{
    public RectTransform objectsParent;

    override protected void Awake()
    {
        objectsParent.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.Score.BonusScore += DisplayBonus;
    }

    private void OnDisable()
    {
        GameEvents.Score.BonusScore -= DisplayBonus;
    }

    private void DisplayBonus(Vector3 position, int bonusValue)
    {
        GetComponent<RectTransform>().position = Camera.main.worldToCameraMatrix * position;

        objectsParent.gameObject.SetActive(true);

        DisplayString(bonusValue.ToString());
    }
}
