using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Bui : MonoBehaviour
{
    protected SpriteRenderer render;
    float timeToDestroy;
    Vector3 dir;
    float speed;
    float size;
    float startTime;
    Sprite sprite;
    Color color;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        startTime = Time.time;
        transform.localScale = Vector3.zero * size;
        render = GetComponent<SpriteRenderer>();
        render.sprite = sprite;
        render.color = color;
        render.sortingOrder = (int)(-10f * transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.hasChanged)
        {
            
        }
        float a = (startTime + timeToDestroy - Time.time) >= 0 ? (startTime + timeToDestroy - Time.time) : 0;
        transform.localScale = (startTime + timeToDestroy - Time.time) * size * Vector3.one;
        transform.position = transform.position + dir * speed * Time.deltaTime;
        if (a <= 0)
            Destroy(gameObject);
    }

    public void SetUp(Sprite sprite, float timeToDestroy, Vector3 dir, float Speed, float Size, Color color)
    {
        this.sprite = sprite;
        this.timeToDestroy = timeToDestroy;
        this.speed = Speed;
        this.size = Size;
        this.dir = dir;
        this.color = color;
    }

    public void setSortingLayerName(string Code)
    {
        if (render != null)
            render.sortingLayerName = Code;
    }
}
