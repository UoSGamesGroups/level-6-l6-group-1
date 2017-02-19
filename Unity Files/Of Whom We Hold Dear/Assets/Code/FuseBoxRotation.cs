using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBoxRotation : MonoBehaviour {

    public enum Direction {forward,Backwards,Left,Right};
    public Vector3 rotation;

    public Direction direction; 

    public Vector3 Rotation()
    {
        switch (direction)
        {
            case Direction.forward:
                rotation = new Vector3(0, 90, 0);
                return rotation;
            case Direction.Backwards:
                rotation = new Vector3(0, 270, 0);
                break;
            case Direction.Left:
                rotation = new Vector3(0, 180, 0);
                break;
            case Direction.Right:
                rotation = new Vector3(0, 0, 0);
                break;
            default:
                break;
        }
        return rotation;
    }
}
