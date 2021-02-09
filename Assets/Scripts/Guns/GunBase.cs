using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(GunJerky))]
public class GunBase : Weapon
{
    [SerializeField] private int _satThuong;
    [SerializeField] private float _criticalRate = 0.2f;
    [SerializeField] protected BulletBase VienDan;
    [SerializeField] protected float SpeedShoot;
    [SerializeField] protected float DoGiat = 10;

    [HideInInspector] public bool isLeftDir;

    protected int SatThuong => _satThuong;

    protected float CriticalRate => _criticalRate;
    protected float distanceShoot
    {
        get
        {
            return 1 / SpeedShoot;
        }
    }
    protected float lastShoot;
    public override Vector3 viTriRaDan
    {
        get
        {
            return transform.position;
        }
    }
    protected override bool ReadyToAttack
    {
        get
        {
            return (Time.time - lastShoot >= distanceShoot && TrangThai == TrangThaiTrangBiVuKhi.DangTrangBi && Host != null);
        }
    }

    public override string NameOfWeapon
    {
        get
        {
            return "Gun " + nameOfWeapon;
        }
    }

    protected override void Start()
    {
        base.Start();
        lastShoot = -distanceShoot;
        OnAttack += Shoot;
        OnAttack += OverLoadShooting;
    }
    protected virtual void Update()
    {
        if (Host != null && (transform.hasChanged || Host.TargetFire != null))
        {
            RotationGun(); 
        }
    }

    public override void Attack()
    {
        if (!ReadyToAttack)
        {
            return;
        }
        OnAttack?.Invoke();
        
    }
    public virtual void Shoot()
    {
        Vector3 DirShoot = GiatSung(Host.DirectFire);
        BulletBase bull = Instantiate(VienDan, viTriRaDan, MathQ.DirectionToQuaternion(DirShoot));
        DamageData damageData = setUpDamageData();
        bull.StartUp(damageData);
        lastShoot = Time.time;
    }

    protected virtual DamageData setUpDamageData()
    {
        Vector3 DirShoot = GiatSung(Host.DirectFire);
        bool isCritical = Random.Range(0, 1f) < CriticalRate;
        int SatThuong = this.SatThuong;
        if (isCritical) SatThuong = (int)(this.SatThuong * (this.CriticalRate + 1));
        DamageData damageData = new DamageData();
        damageData.Damage = SatThuong + Random.Range(-1, 2);
        damageData.Direction = DirShoot;
        damageData.From = Host;
        damageData.IsCritical = isCritical;
        return damageData;
    }

    protected virtual void OverLoadShooting()
    {
        if (Host is PlayerController)
        {
            PlayerController player = Host as PlayerController;
            player.UseHealPhy(0.45f/ SpeedShoot);
        }
    }


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

    private void OnDestroy()
    {
        OnAttack -= Shoot;
        OnAttack -= OverLoadShooting;
    }
}
