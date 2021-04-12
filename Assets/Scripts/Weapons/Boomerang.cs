using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Weapon
{
    [SerializeField] float _speedAttack = 1f;
    [SerializeField] Transform parentRender;
    [SerializeField] BoomerangBullet _bulletPRefab;
    public override Vector3 PositionStartAttack => transform.position;

    protected override bool ReadyToAttack => Time.time - m_last_attack > m_time_range_attack && _bulletPRefab != null;

    float speedAttack => _speedAttack;
    float m_time_range_attack => 1 / speedAttack;

    float m_last_attack;
    float timeApear = 0.3f;
    protected PoolingGameObject pool => PoolingGameObject.PoolingMain;
    protected int id_bullet = 0;
    public override float TakeTied => 0.5f/speedAttack;
    protected override void Awake()
    {
        base.Awake();
        if (_bulletPRefab != null)
        {
            id_bullet = pool.AddPrefab(_bulletPRefab);
        }
    }

    protected virtual void Update(){
        UpdateRender();
    }

    private void UpdateRender()
    {
        bool left = false;
        if (Host != null)
        {
            if (Host.DirectFire.x < 0)
            {
                left = true;
            }
        }
        render.flipX = left;
    }

    public override bool Attack(DamageData damageData)
    {
        if (!ReadyToAttack)
        {
            OnNotAttacked?.Invoke();
            return false;
        }
        Shoot(damageData);
        VFXAttack();
        m_last_attack = Time.time;
        OnAttacked?.Invoke();
        return true;
    }
    protected virtual void VFXAttack()
    {
        parentRender.localScale = Vector3.zero;
        float m = m_time_range_attack - timeApear - 0.1f;
        if (m < 0)
            m = 0;
        StartCoroutine(Appear(m));
    }
    IEnumerator Appear(float a)
    {
        yield return new WaitForSeconds(a);
        float timec = 0;
        while (timec < timeApear)
        {
            parentRender.localScale = Vector3.one * timec / timeApear;
            timec += Time.deltaTime;
            yield return null;
        }
        parentRender.localScale = Vector3.one;
    }

    public virtual void Shoot(DamageData damageData)
    {
        Vector3 DirShoot = damageData.Direction;
        BoomerangBullet bull = pool.Spawn(id_bullet, PositionStartAttack, MathQ.DirectionToQuaternion(DirShoot)) as BoomerangBullet;
        SetUpDamageData(damageData);
        bull.StartUp(damageData);
    }
    protected virtual void SetUpDamageData(DamageData damageData)
    {
        int SatThuong = this.SatThuong;
        damageData.Damage = SatThuong;
    }

    public override string GetNameOfWeapon()
    {
        return "Boomerang " + nameOfWeapon;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_bulletPRefab != null)
        {
            pool.RemovePrefab(id_bullet);
            id_bullet = 0;
        }
    }
}
