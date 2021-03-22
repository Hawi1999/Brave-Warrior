using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPracticeEnemy : MonoBehaviour
{
    public Enemy EnemyPrefab;
    public Vector2 min;
    public Vector2 max;

    private List<Enemy> list = new List<Enemy>();

    private float lastSpawn;
    private float distanceSpawn = 1f;

    private Transform PREnemy;
    // Start is called before the first frame update
    void Start()
    {
        PREnemy = Instantiate(new GameObject("PREnemy"), transform).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (list.Count < 1)
        {
            if (Time.time - lastSpawn > distanceSpawn)
            {
                SpawnEnemy();
                lastSpawn = Time.time;
            }
        }
    }

    void SpawnEnemy()
    {
        Vector3 position = getPosition();
        Enemy enemy = EntityManager.Instance.SpawnEnemy(EnemyPrefab, position, PREnemy, getLimit());
        list.Add(enemy);
        enemy.gameObject.AddComponent<LockAttack>();
    }

    Vector2 getPosition()
    {
        int i = 0;
        Collider2D[] cols;
        Vector2 position;
        do
        {
            i++;
            position = new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
            cols = Physics2D.OverlapBoxAll(position, Vector2.one / 2, 0, EntityManager.Instance.WallAndBarrier);
        } while (cols != null && cols.Length != 0 && i < 50);
        return position;
    }

    Vector2[] getLimit()
    {
        Vector2[] vector2s = new Vector2[2];
        vector2s[0] = min;
        vector2s[1] = max;
        return vector2s;
    }
}
