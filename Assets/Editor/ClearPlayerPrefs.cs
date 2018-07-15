using UnityEditor;
using UnityEngine;

public class AddMenu : EditorWindow
{

    [MenuItem("Edit/Reset Playerprefs")]

    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

}

