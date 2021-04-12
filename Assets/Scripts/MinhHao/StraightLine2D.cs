using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLine2D 
{
    // y = ax + b
    float m_a;
    float m_b;
    float m_c;

    public float a => m_a;
    public float b => m_b;
    public float c => m_c;

    public enum TypeContruct
    {
        /// <summary>
        /// đầu vào là 2 vị trí
        /// </summary>
        PositionAndPosition,
        /// <summary>
        /// Đầu vào là vector chỉ phương và vị trí
        /// </summary>
        UAndPosition,
        /// <summary>
        /// Đầu vào là vector pháp tuyến và vị trí
        /// </summary>
        NAndPosition,
    }
    public StraightLine2D(Vector2 v1, Vector2 v2, TypeContruct type = TypeContruct.PositionAndPosition)
    {  Vector2 n;
        switch (type)
        {
            case TypeContruct.PositionAndPosition:
                Vector2 u = v2 - v1;
                n = new Vector2(-u.y, u.x);
                break;
            case TypeContruct.UAndPosition:
                n = new Vector2(-v1.y, v1.x);
                break;
            case TypeContruct.NAndPosition:
                n = v1;
                break;
            default:
                u = v2 - v1;
                n = new Vector2(-u.y, u.x);
                break;

        }
        m_a = n.x;
        m_b = n.y;
        m_c = - m_a * v1.x - m_b * v1.y;
    }
}
