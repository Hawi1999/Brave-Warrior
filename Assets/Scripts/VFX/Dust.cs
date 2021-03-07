using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Dust : PoolingBehaviour
{
    protected SpriteRenderer render;
    float timeToDestroy;
    Vector3 dir;
    float speed;
    Color color;
    float size;
    float startTime;
    Sprite sprite;
    protected virtual Sprite[] sprites => VFXManager.Instance.SpritesDust;
    public class ClassReset
    {
        public SpriteRenderer render;
        public float timeToDestroy;
        public Vector3 dir;
        public float speed;
        public float size;
        public float startTime;
        public Sprite sprite;
        public Color color;
    }
    // Start is called before the first frame update
    protected virtual void StartUp()
    {
        startTime = Time.time;
        render = GetComponent<SpriteRenderer>();
        render.sprite = sprite;
        render.color = color;
        render.sortingOrder = (int)(-10f * transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        float a = (startTime + timeToDestroy - Time.time) >= 0 ? (startTime + timeToDestroy - Time.time) : 0;
        transform.localScale = (startTime + timeToDestroy - Time.time) * size * Vector3.one;
        transform.position = transform.position + dir * speed * Time.deltaTime;
        if (a <= 0)
            Rest();
    }

    public void SetUp(float timeToDestroy, Vector3 dir, float speed, float size, Color color)
    {
        this.sprite = getSprite();
        if (sprite == null)
            return;
        this.timeToDestroy = timeToDestroy;
        this.speed = speed;
        this.size = size;
        this.color = color;
        this.dir = dir;
        StartUp();
    }

    public void setSortingLayerName(string Code)
    {
        if (render != null)
            render.sortingLayerName = Code;
    }

    protected virtual Sprite getSprite()
    {
        if (sprites == null || sprites.Length == 0)
        {
            Debug.Log("Không có sprite cho Bụi");
            Rest();
            return null;
        }
        return sprites[Random.Range(0, sprites.Length)];
    }
}
