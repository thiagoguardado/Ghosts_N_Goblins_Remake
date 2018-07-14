using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class CameraAspectRatio : MonoBehaviour {

    float width;
    float height;
    float targetaspect = 256.0f / 224.0f;

    void Start()
    {
        width = (float)Screen.width;
        height = (float)Screen.height;

        ChangeAspect();
    }

    void Update()
    {
        if (ScreenSizeChanged())
            ChangeAspect();
    }

    bool ScreenSizeChanged()
    {
        bool changed = false;
        if ((float)Screen.width != width || (float)Screen.height != height)
        {
            changed = true;
        }

        width = (float)Screen.width;
        height = (float)Screen.height;

        return changed;
    }

    void ChangeAspect()
    {
        float currentAspect = width / height;
        float factor = currentAspect / targetaspect;
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;

        if (factor < 1.0f)
        {
            rect.width = 1f;
            rect.height = factor;
            rect.x = 0;
            rect.y = (1f - factor) / 2f;
            
        }
        else 
        {
            rect.width = 1f/factor ;
            rect.height = 1f;
            rect.x = (1f - 1f / factor) / 2f;
            rect.y = 0;
        }

        camera.rect = rect;
    }
}
