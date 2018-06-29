using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Font Dictionary", menuName = "", order = 0)]
public class FontImages : ScriptableObject
{

    [System.Serializable]
    public class CharImage
    {
        public char character;
        public Sprite sprite;
    }

    public CharImage[] charImages;

    public Dictionary<char, Sprite> dictionary = new Dictionary<char, Sprite>();

    private void OnEnable()
    {
        for (int i = 0; i < charImages.Length; i++)
        {
            dictionary.Add(charImages[i].character, charImages[i].sprite);
        }
    }

    public Sprite GetSprite(char character)
    {
        if (dictionary.ContainsKey(character))
        {
            return dictionary[character];
        }

        return null;
    }
}