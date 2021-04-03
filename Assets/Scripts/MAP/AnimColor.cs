using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimColor : MonoBehaviour
{
    [SerializeField] Gradient gradient;
    [SerializeField] float speed;
    SpriteRenderer render;
    private float id;
    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        id += Time.deltaTime * speed;
        id %= 1;
        render.color = gradient.Evaluate(id);
    }
}
