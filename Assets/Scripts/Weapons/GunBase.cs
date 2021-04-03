using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(GunJerky))]
public class GunBase : Weapon
{
    [SerializeField] protected BulletBase VienDan;
    [SerializeField] protected ParticleSystem VFXShoot;
    [SerializeField] protected float SpeedShoot;
    [Range(0, 90)]
    [SerializeField] protected float DoGiat = 10;
    [SerializeField] protected Transform HeadGun;
    [HideInInspector] public bool isLeftDir;

    protected PoolingGameObject pool => PoolingGameObject.PoolingMain;
    protected int id_pool_bullet;
    protected float distanceShoot
    {
        get
        {
            return 1 / SpeedShoot;
        }
    }
    protected float lastShoot;
    public override Vector3 PositionStartAttack
    {
        get
        {
            if (HeadGun != null)
            {
                return HeadGun.position;
            } else
            {
                return transform.position;
            }
        }
    }
    protected override bool ReadyToAttack
    {
        get
        {
            return (Time.time - lastShoot >= distanceShoot && TrangThai == WeaponStatus.Equiping && Host != null);
        }
    }

    public override string GetNameOfWeapon()
    {
        return "Gun " + nameOfWeapon;
    }

    protected override void Awake()
    {
        base.Awake();
        if (VienDan != null)
        {
            id_pool_bullet = pool.AddPrefab(VienDan);
        }
    }

    protected override void Start()
    {
        base.Start();
        lastShoot = -distanceShoot;
    }
    protected virtual void Update()
    {
        if (Host != null && (transform.hasChanged || (Host.TargetFire != null && Host.TargetFire as UnityEngine.Object != null)))
        {
            RotationGun(); 
        }
    }

    public override bool Attack(DamageData damageData)
    {
        if (!ReadyToAttack)
        {
            OnNotAttacked?.Invoke();
            return false;
        }
        Shoot(damageData);
        ShowVFXAttack();
        lastShoot = Time.time;
        OnAttacked?.Invoke();
        return true;
        
    }
    public virtual void Shoot(DamageData damageData)
    {
        Vector3 DirShoot = damageData.Direction;
        BulletBase bull = pool.Spawn(id_pool_bullet, PositionStartAttack, MathQ.DirectionToQuaternion(DirShoot)) as BulletBase;
        SetUpDamageData(damageData);
        bull.StartUp(damageData);
    }

    protected virtual void SetUpDamageData(DamageData damageData)
    {
        bool isCritical = Random.Range(0, 1f) < 0.2f;
        int SatThuong = this.SatThuong;
        damageData.Damage = SatThuong;
        damageData.FromGunWeapon = true;
        damageData.IsCritical = isCritical;
    }

    public override float TakeTied => 0.5f / SpeedShoot;


    // Được gọi mỗi lần Update
    private void RotationGun()
    {
        if (Host == null)
            return;
        transform.rotation = MathQ.DirectionToQuaternion(Host.DirectFire);
        isLeftDir = Host.DirectFire.x < 0;
        render.flipY = isLeftDir;
    }

    protected virtual Vector3 GiatSung(Vector3 direction)
    {
        Vector3 Do = MathQ.DirectionToRotation(direction);
        Do += new Vector3(0, 0, Random.Range(-DoGiat / 2, DoGiat / 2));
        return MathQ.RotationToDirection(Do.z).normalized;
    }

    protected virtual void ShowVFXAttack(){
        if (VFXShoot != null){
            VFXShoot.Play();
        }
    }

    public override void OnEquip()
    {
        base.OnEquip();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(PositionStartAttack, PositionStartAttack + MathQ.RotationToDirection(transform.rotation.eulerAngles.z + DoGiat/2) * 10f);
        Gizmos.DrawLine(PositionStartAttack, PositionStartAttack + MathQ.RotationToDirection(transform.rotation.eulerAngles.z - DoGiat/2) * 10f);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        pool.RemovePrefab(id_pool_bullet);
    }
}
