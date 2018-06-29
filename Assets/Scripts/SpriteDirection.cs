using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LookingDirection
{
    None,
    Left,
    Right,
    Up,
    Down,
}


public class SpriteDirection : MonoBehaviour {

    public LookingDirection startLookingDirection;
    public LookingDirection lookingDirection { get; private set; }
    public Vector3 WorldLookingDirection
    {
        get
        {
            switch (lookingDirection)
            {
                case LookingDirection.None:
                    return Vector3.zero;
                case LookingDirection.Left:
                    return Vector3.left;
                case LookingDirection.Right:
                    return Vector3.right;
                case LookingDirection.Up:
                    return Vector3.up;
                case LookingDirection.Down:
                    return Vector3.down;
                default:
                    return Vector3.zero;
            }
        }
    }
    Dictionary<LookingDirection, Vector3> scalingValues = new Dictionary<LookingDirection, Vector3>();


	private void Awake()
    {
        lookingDirection = startLookingDirection;
        if (lookingDirection == LookingDirection.Left || lookingDirection == LookingDirection.Right)
        {
            scalingValues.Add(startLookingDirection, new Vector3(1, 1, 1));
            scalingValues.Add(OtherDirectionY(), new Vector3(-1, 1, 1));
        }
	}


    public LookingDirection OtherDirectionY()
    {
        switch (lookingDirection)
        {
            case LookingDirection.Left:
                return LookingDirection.Right;
            case LookingDirection.Right:
                return LookingDirection.Left;
            default:
                return lookingDirection;
        }

    }

    public void FlipDirectionY()
    {
        FaceDirection(OtherDirectionY());
    }

    public void FaceDirection(LookingDirection newDirection)
    {
        lookingDirection = newDirection;
        if (scalingValues.ContainsKey(lookingDirection))
        {
            transform.localScale = scalingValues[lookingDirection];
        }
    }

}
