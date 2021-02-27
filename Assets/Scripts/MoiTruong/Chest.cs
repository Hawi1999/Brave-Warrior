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
    public TypeChest type;
    ChestData chestData;
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
            flyup.startColor = GetColorByType(type);
            flyup.Stop();
        }
        if (_sinePrejab != null)
        {
            sine = Instantiate(_sinePrejab, transform);
            sine.startColor = GetColorByType(type);
        }
    }

    public static Color GetColorByType(TypeChest type)
    {
        switch (type)
        {
            case TypeChest.Copper: return Color.green;
            case TypeChest.Silver: return Color.yellow;
            case TypeChest.Gold: return Color.red;
            case TypeChest.Start: return Color.white;
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

    public void setUp(ChestData chestData)
    {
        this.chestData = chestData;
    }
    public void OpenChest()
    {
        ani.setAnimation("OpenChest");
        if (chestData.NameOfRewards == null || chestData.NameOfRewards.Length == 0)
        {
            Debug.Log("Danh sach phần thưởng trống");
            return;
        }
        Reward reward = RewardManager.GetRewardByName(chestData.getRandomReward());
        reward = Instantiate(reward, transform.position, Quaternion.identity);
        reward.Appear();
        if (flyup != null)
        {
            flyup.Play();
        }
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
