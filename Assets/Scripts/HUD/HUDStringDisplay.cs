using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDStringDisplay : MonoBehaviour {

    public Image[] spriteUnits;

    public FontImages fontDictionary;


    protected virtual void Awake()
    {
        TurnOffAllImages();
    }


    protected void DisplayString(string displayString)
    {

        TurnOffAllImages();

        List<Sprite> sprites = new List<Sprite>();

        for (int i = displayString.Length - 1; i >= 0; i--)
        {
            Sprite charSprite = fontDictionary.GetSprite(displayString[i]);
            if (charSprite != null)
            {
                sprites.Add(charSprite);
            }
            else
            {
                Debug.LogError("Could not find " + displayString[i] + " sprite on font dictionary");
                return;
            }
        }

        DisplayString(sprites);

    }

    private void TurnOffAllImages()
    {
        for (int i = 0; i < spriteUnits.Length; i++)
        {
            TurnOffImage(spriteUnits[i]);
        }
    }

    private void DisplayString(List<Sprite> sprites)
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            TurnOnImage(spriteUnits[i], sprites[i]);
        }
    }

    private void TurnOffImage(Image image)
    {
        image.enabled = false;
    }

    private void TurnOnImage(Image image, Sprite sprite)
    {
        image.enabled = true;
        image.color = Color.white;
        image.sprite = sprite;
    }

}
