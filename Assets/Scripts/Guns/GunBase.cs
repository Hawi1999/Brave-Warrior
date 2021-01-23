using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : Weapon
{
    [SerializeField] protected int SatThuong;
    [SerializeField] protected BulletBase VienDan;
    [SerializeField] protected float SpeedShoot;
    [SerializeField] protected float DoGiat = 10;

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
        lastShoot = -distanceShoot;
    }
    protected virtual void Update()
    {
        RotationGun(); 
    }

    public override void Attack()
    {
        Shoot();
    }
    public virtual void Shoot()
    {
        if (!ReadyToAttack)
        {
            return;
        }
        Vector3 DirShoot = GiatSung(Host.DirectFire);
        BulletBase bull = Instantiate(VienDan, viTriRaDan, MathQ.DirectionToQuaternion(DirShoot));
        bull.StartUp(Host, DirShoot.normalized, new DamageData(SatThuong, DirShoot, default, Host, new RaycastHit2D()));
        lastShoot = Time.time;
    }


    // Được gọi mỗi lần Update
    private void RotationGun()
    {
        if (Host == null)
            return;
        transform.rotation = MathQ.DirectionToQuaternion(Host.DirectFire);
        if (Host.DirectFire.x < 0)
        {
            render.flipY = true;
        } else if (Host.DirectFire.x > 0)
        {
            render.flipY = false;
        }
    }

    protected virtual Vector3 GiatSung(Vector3 direction)
    {
        Vector3 Do = MathQ.DirectionToRotation(direction);
        Do += new Vector3(0, 0, Random.Range(-DoGiat / 2, DoGiat / 2));
        return MathQ.RotationToDirection(Do.z).normalized;
    }
}
