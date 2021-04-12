using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suriken : Weapon
{
    [SerializeField] float _speedAttack = 1f;
    [SerializeField] int _amountTarget = 3;
    [SerializeField] SurikenBullet _bulletPrefab;
    [SerializeField] LayerMask layerTarget;

    float m_time_delay_attack => 1 / _speedAttack;
    float m_lastTime_Attack;
    private PoolingGameObject pool => PoolingGameObject.PoolingMain;
    private int id_bul;
    protected override void Awake()
    {
        base.Awake();
        m_lastTime_Attack = Time.time - 1 / _speedAttack;
        if (_bulletPrefab != null)
        {
            id_bul = pool.AddPrefab(_bulletPrefab);
        }
    }

    private void Update()
    {
        Rotate();
    }

    public override Vector3 PositionStartAttack => transform.position;

    protected override bool ReadyToAttack => Time.time - m_lastTime_Attack > m_time_delay_attack && _bulletPrefab != null;

    public override bool Attack(DamageData damageData)
    {

        if (!ReadyToAttack)
        {
            OnNotAttacked?.Invoke();
            return false;
        }
        List<Vector2> list = FindPositionEnemys(Host.DistanceFindTarget);
        Shoot(damageData, list);
        m_lastTime_Attack = Time.time;
        OnAttacked?.Invoke();
        return true;
    }
    private void Rotate()
    {
        if (Host == null)
            return;
        transform.rotation = MathQ.DirectionToQuaternion(Host.DirectFire);
    }

    protected virtual void Shoot(DamageData damage, List<Vector2> list)
    {
        if (list == null || list.Count == 0)
        {
            DamageData damageData = damage.Clone;
            damageData.Damage = SatThuong;
            SurikenBullet bul = pool.Spawn(id_bul,transform.position, Quaternion.identity) as SurikenBullet;
            bul.StartUp(damageData);
            return;
        }
        for (int i = 0; i < list.Count; i++)
        {
            DamageData damageData = damage.Clone;
            Vector2 pos = list[i];
            Vector2 Dir = (pos - (Vector2)transform.position).normalized;
            damageData.Direction = Dir;
            damageData.Damage = SatThuong;
            SurikenBullet bul = pool.Spawn(id_bul,transform.position, Quaternion.identity) as SurikenBullet;
            bul.StartUp(damageData);
        }
    }

    public override float TakeTied => 0.5f/_speedAttack;

    public override string GetNameOfWeapon()
    {
        return "Suriken " + nameOfWeapon;
    }



    List<Vector2> FindPositionEnemys(float distance)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, distance, layerTarget);
        if (cols == null || cols.Length == 0)
        {
            return null;
        }
        List<Vector2> list = new List<Vector2>();
        List<float> Distance = new List<float>();
        foreach (Collider2D collider2D in cols)
        {
            if (collider2D.TryGetComponent(out ITakeHit take))
            {
                Vector2 pos = (take as MonoBehaviour).transform.position;
                list.Add(pos);
                Distance.Add(Vector2.Distance(transform.position, pos));
            }
        }
        if (list.Count > _amountTarget)
        {
            int leng = list.Count;
            for (int i = 0; i < leng - 1; i++)
            {
                for (int j = i + 1; j < leng; j++)
                {
                    if (Distance[j] < Distance[i])
                    {
                        float a = Distance[i];
                        Distance[i] = Distance[j];
                        Distance[j] = a;

                        Vector2 b = list[i];
                        list[i] = list[j];
                        list[j] = b;
                    }
                }
            }
        }
        List<Vector2> listkq = new List<Vector2>();
        int max = list.Count > _amountTarget ? _amountTarget : list.Count;
        for (int i = 0; i < max; i++)
        {
            listkq.Add(list[i]);
        }
        return listkq;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        pool.RemovePrefab(id_bul);
        id_bul = 0;
    }
}