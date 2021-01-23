using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleByTree : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NapSauCay")
        {
            Vector4 a = collision.GetComponent<SpriteRenderer>().color;
            a.w = 0.4f;
            collision.GetComponent<SpriteRenderer>().color = a;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NapSauCay")
        {
            Vector4 a = collision.GetComponent<SpriteRenderer>().color;
            a.w = 1f;
            collision.GetComponent<SpriteRenderer>().color = a;
        }
    }
}
