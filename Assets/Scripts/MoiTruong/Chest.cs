using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AnimationQ))]
[RequireComponent(typeof(SpriteRenderer))]
public class Chest : MonoBehaviour
{
    [HideInInspector]
    public ChestData Data;
    protected SpriteRenderer render => GetComponent<SpriteRenderer>();
    protected AnimationQ ani => GetComponent<AnimationQ>();
    protected BoxCollider2D col;
    public ColorChest colorChest;
    [HideInInspector]
    public TypeChest type;
    public Vector2 OffSetSpawnWard;
    [SerializeField] ParticleSystem _flyupPrejab;
    [SerializeField] ParticleSystem _sinePrejab;

    private ParticleSystem flyup;
    private ParticleSystem sine;
    bool opened = false;

    private float TimeStart;
    private void Awake()
    {
        if (_flyupPrejab != null)
        {
            flyup = Instantiate(_flyupPrejab, transform);
            flyup.startColor = GetColorByType(colorChest);
            flyup.Stop();
        }
        if (_sinePrejab != null)
        {
            sine = Instantiate(_sinePrejab, transform);
            sine.startColor = GetColorByType(colorChest);
        }
    }

    public void SetUpData(ChestData Data)
    {
        this.Data = Data;
        type = Data.Type;
    }

    public static Color GetColorByType(ColorChest type)
    {
        switch (type)
        {
            case ColorChest.Copper: return Color.green;
            case ColorChest.Silver: return Color.yellow;
            case ColorChest.Gold: return Color.red;
        }
        return Color.cyan;
    }

    private void Start()
    {
        Color a = render.color;
        a.a = 0;
        render.color = a;
        render.sortingOrder = (int)(-10f * transform.position.y);
        TimeStart = Time.time;
    }

    public void OpenChest()
    {
        ani.setAnimation("OpenChest");
        Reward reward = Data.getRandomReward();
        if (reward == null)
        {
            Debug.Log("Reward is Null");
            return;
        }
         reward = Instantiate(reward, transform.position, Quaternion.identity);
        if (flyup != null)
        {
            flyup.Play();
        }
        PositionControl pct = reward.gameObject.AddComponent<PositionControl>();
        pct.SetUp(reward.transform.position, reward.transform.position + new Vector3(0, 0.4f, 0), 0.5f);
        pct.StartAnimation();
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
