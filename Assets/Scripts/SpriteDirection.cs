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
            return LookingDirectionToVector3(lookingDirection);
        }
    }


    Dictionary<LookingDirection, Vector3> scalingValues = new Dictionary<LookingDirection, Vector3>();


	private void Awake()
    {
        lookingDirection = startLookingDirection;
        if (lookingDirection == LookingDirection.Left || lookingDirection == LookingDirection.Right)
        {
            scalingValues.Add(startLookingDirection, new Vector3(1, 1, 1));
            scalingValues.Add(OtherDirectionY(lookingDirection), new Vector3(-1, 1, 1));
        }
	}


    public static Vector2 LookingDirectionToVector2(LookingDirection lookingDirection)
    {
        switch (lookingDirection)
        {
            case LookingDirection.None:
                return Vector2.zero;
            case LookingDirection.Left:
                return Vector2.left;
            case LookingDirection.Right:
                return Vector2.right;
            case LookingDirection.Up:
                return Vector2.up;
            case LookingDirection.Down:
                return Vector2.down;
            default:
                return Vector2.zero;
        }
    }

    public static Vector3 LookingDirectionToVector3(LookingDirection lookingDirection)
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

    public static LookingDirection OtherDirectionY(LookingDirection direction)
    {
        switch (direction)
        {
            case LookingDirection.Left:
                return LookingDirection.Right;
            case LookingDirection.Right:
                return LookingDirection.Left;
            default:
                return direction;
        }

    }

    public void FlipDirectionY()
    {
        FaceDirection(OtherDirectionY(lookingDirection));
    }

    public void FaceDirection(LookingDirection newDirection)
    {
        lookingDirection = newDirection;
        if (scalingValues.ContainsKey(lookingDirection))
        {
            transform.localScale = scalingValues[lookingDirection];
        }
    }

    public static LookingDirection ConvertVectorToLookingDirection(Vector2 vector)
    {

        float horizontal = Vector2.Dot(vector, Vector2.right);
        float vertical = Vector2.Dot(vector, Vector2.up);

        if (horizontal == vertical && horizontal == 0)
        {
            return LookingDirection.None;
        }


        if (Mathf.Abs(horizontal) >= Mathf.Abs(vertical))
        {
            if (Mathf.Sign(horizontal) > 0)
            {
                return LookingDirection.Right;
            }
            else
            {
                return LookingDirection.Left;
            }
        }
        else
        {
            if (Mathf.Sign(vertical) > 0)
            {
                return LookingDirection.Up;
            }
            else
            {
                return LookingDirection.Down;
            }
        }
        

    }

}
