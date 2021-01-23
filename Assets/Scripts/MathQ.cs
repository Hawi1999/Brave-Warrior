using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathQ 
{
    public static Quaternion DirectionToQuaternion(Vector3 dir)
    {
        if (dir.x == 0)
        {
            if (dir.y > 0)
                return Quaternion.Euler(new Vector3(0, 0, 90));
            else if (dir.y < 0)
                return Quaternion.Euler(new Vector3(0, 0, -90));
            else return Quaternion.Euler(Vector3.zero);
        }

        if (dir.y == 0)
        {
            if (dir.x > 0)
                return Quaternion.Euler(Vector3.zero);
            else if (dir.x < 0)
                return Quaternion.Euler(new Vector3(0, 0, 180));
        }
        int k = (int)(dir.y / Mathf.Abs(dir.y));
        float Do = Vector3.Angle(dir, Vector3.right) * k;
        return Quaternion.Euler(new Vector3(0,0,Do));
    }
    public static Vector3 RotationToDirection(float rotZ)
    {
        return Quaternion.Euler(new Vector3(0, 0, rotZ)) * Vector3.right;
    }
    public static Vector3 QuaternionToDirection(Quaternion a)
    {
        return a * Vector3.right;
    }
    public static Vector3 DirectionToRotation(Vector3 dir)
    {
        if (dir.x == 0)
        {
            if (dir.y > 0)
                return (new Vector3(0, 0, 90));
            else if (dir.y < 0)
                return (new Vector3(0, 0, -90));
            else return (Vector3.zero);
        }

        if (dir.y == 0)
        {
            if (dir.x > 0)
                return (Vector3.zero);
            else if (dir.x < 0)
                return (new Vector3(0, 0, 180));
        }
        int k = (int)(dir.y / Mathf.Abs(dir.y));
        return new Vector3(0,0, Vector3.Angle(dir, Vector3.right) * k);
    }

}
