using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AnimationQ))]
[RequireComponent(typeof(SpriteRenderer))]
public class Chest : MonoBehaviour
{
    protected SpriteRenderer render => GetComponent<SpriteRenderer>();
    protected AnimationQ ani => GetComponent<AnimationQ>();
    protected BoxCollider2D col;
    protected StarsControl starsControl;
    public TypeChest type;
    public Reward[] rewards;
    public Vector2 OffSetSpawnWard;
    bool opened = false;

    private float TimeStart;
    private void Start()
    {
        starsControl = GetComponent<StarsControl>();

        Color a = render.color;
        a.a = 0;
        render.color = a;
        starsControl?.SetUp(transform, transform.position - new Vector3(0.3f, -0.2f, 0), new Vector3(1, 0, 0), 3, 0.3f);
        starsControl?.StartSpawn();
        render.sortingOrder = (int)(-10f * transform.position.y);
        TimeStart = Time.time;
    }

    public void setUp(Reward[] rewards)
    {
        this.rewards = rewards;
    }
    public void OpenChest()
    {
        ani.setAnimation("OpenChest");
        if (rewards == null || rewards.Length == 0)
        {
            Debug.Log("Danh sach phần thưởng trống");
            return;
        }
        Reward reward = rewards[Random.Range(0, rewards.Length)];
        reward = Instantiate(reward, transform.position, Quaternion.identity);
        reward.Appear();
    }

    private void Update()
    {
        if (transform.hasChanged)
        {
            render.sortingOrder = (int)(-10f * transform.position.y);
        }
        if (Time.time - TimeStart < 1f)
        {
            Color a = render.color;
            a.a = Time.time - TimeStart;
            render.color = a;

        } else
        {
            Color a = render.color;
            a.a = 1;
            render.color = a;
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
        if (!opened)
        {
            OpenChest();
            opened = true;
        }
        }
    }
}
