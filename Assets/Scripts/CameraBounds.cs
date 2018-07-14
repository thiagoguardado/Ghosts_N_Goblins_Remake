using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{

    private Rect bounds = new Rect();
    private Rect boundsWithOffset = new Rect();
    public float externalBoundsSizeMultiplier;
    public Color internalBoundsColor;
    public Color externalBoundsColor;

    public Rect Bounds
    {
        get
        {
            return bounds;
        }
    }

    public Rect BoundsWithOffset
    {
        get
        {
            return boundsWithOffset;
        }
    }

    private void Awake()
    {
        UpdateBounds();
    }

    private void Update()
    {
        UpdateBounds();
    }

    void UpdateBounds()
    {
        bounds.center = transform.position;
        bounds.height = Camera.main.orthographicSize * 2f;
        bounds.width = Camera.main.aspect * bounds.height;

        boundsWithOffset.center = transform.position;
        boundsWithOffset.height = bounds.height * externalBoundsSizeMultiplier;
        boundsWithOffset.width = bounds.width * externalBoundsSizeMultiplier;

    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(new Vector3(bounds.xMin, bounds.yMax), new Vector3(bounds.xMax, bounds.yMax), internalBoundsColor);
        Debug.DrawLine(new Vector3(bounds.xMin, bounds.yMin), new Vector3(bounds.xMax, bounds.yMin), internalBoundsColor);
        Debug.DrawLine(new Vector3(bounds.xMin, bounds.yMin), new Vector3(bounds.xMin, bounds.yMax), internalBoundsColor);
        Debug.DrawLine(new Vector3(bounds.xMax, bounds.yMin), new Vector3(bounds.xMax, bounds.yMax), internalBoundsColor);

        Debug.DrawLine(new Vector3(boundsWithOffset.xMin, boundsWithOffset.yMax), new Vector3(boundsWithOffset.xMax, boundsWithOffset.yMax), externalBoundsColor);
        Debug.DrawLine(new Vector3(boundsWithOffset.xMin, boundsWithOffset.yMin), new Vector3(boundsWithOffset.xMax, boundsWithOffset.yMin), externalBoundsColor);
        Debug.DrawLine(new Vector3(boundsWithOffset.xMin, boundsWithOffset.yMin), new Vector3(boundsWithOffset.xMin, boundsWithOffset.yMax), externalBoundsColor);
        Debug.DrawLine(new Vector3(boundsWithOffset.xMax, boundsWithOffset.yMin), new Vector3(boundsWithOffset.xMax, boundsWithOffset.yMax), externalBoundsColor);
    }

}
