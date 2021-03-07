using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(GunJerky))]
public class GunBase : Weapon
{
    [SerializeField] private float _criticalRate = 0.2f;
    [SerializeField] protected BulletBase VienDan;
    [SerializeField] protected ParticleSystem VFXShoot;
    [SerializeField] protected float SpeedShoot;
    [Range(0, 90)]
    [SerializeField] protected float DoGiat = 10;
    [SerializeField] protected Transform HeadGun;
    [HideInInspector] public bool isLeftDir;

    protected PoolingGameObject<BulletBase> poolling_bullet;

    protected float CriticalRate => _criticalRate;
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

    protected override void Start()
    {
        base.Start();
        lastShoot = -distanceShoot;
    }
    protected virtual void Update()
    {
        if (Host != null && (transform.hasChanged || Host.TargetFire != null))
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
        Vector3 DirShoot = GiatSung(Host.DirectFire);
        BulletBase bull = poolling_bullet.Spawn(PositionStartAttack, MathQ.DirectionToQuaternion(DirShoot));
        SetUpDamageData(damageData, DirShoot);
        bull.StartUp(damageData);
    }

    protected virtual void SetUpDamageData(DamageData damageData, Vector3 Direction)
    {
        bool isCritical = Random.Range(0, 1f) < CriticalRate;
        int SatThuong = this.SatThuong;
        if (isCritical) SatThuong = (int)(this.SatThuong * (this.CriticalRate + 1));
        damageData.Damage = SatThuong;
        damageData.Direction = Direction;
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
        poolling_bullet = new PoolingGameObject<BulletBase>(VienDan);
    }

    public override void OnTuDo()
    {
        base.OnTuDo();
        if (poolling_bullet != null)
        {
            poolling_bullet.DestroyAll();
        }
    }

    protected virtual void OnLevelWasLoaded(int level)
    {
        poolling_bullet?.DestroyAll();
        poolling_bullet = new PoolingGameObject<BulletBase>(VienDan);
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
}
