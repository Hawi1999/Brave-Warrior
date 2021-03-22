using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MeleeBase : Weapon
{
    [SerializeField] ColliderMeleeAttack colliderAttack;
    [SerializeField] float SpeedAttack = 3f;
    [SerializeField] float TimeShit = 0.3f;
    [SerializeField] Vector2 AngleAttack = new Vector2(10f, 80f);
    private bool isLeftDir
    {
        get
        {
            if (Host == null)
            {
                return false;
            } else
            {
                return Host.DirectFire.x < 0;
            }
        }
    }

    private bool isCutDir
    {
        get
        {
            if (Host == null)
            {
                return false;
            }
            else
            {
                float z = MathQ.DirectionToRotation(Host.DirectFire).z;
                if (z < -135 || z > 45)
                    return true;
                else
                    return false;
            }
        }
    }
    private float lastTimeCut;
    private bool _lastCutisAboveToBelow;
    private bool lastCutisAboveToBelow
    {
        get
        {
            if (Time.time - lastTimeCut > TimeShit * 2)
            {
                return false;
            }
            return _lastCutisAboveToBelow;
        }
        set
        {
            _lastCutisAboveToBelow = value;
        }
    }
    private float distanceAttack => 1 / SpeedAttack;
    private bool Attacking;
    protected override bool ReadyToAttack => (Time.time - lastTimeCut >= distanceAttack && TrangThai == WeaponStatus.Equiping && Host != null);
    public override float TakeTied => 0.5f / SpeedAttack;
    public override Vector3 PositionStartAttack => transform.position;

    protected override void Start()
    {
        base.Start();
        OnCutBegin += OnAttackBegin;
        OnCutComplete += OnAttackEnd;
    }

    public override bool Attack(DamageData damageData)
    {
        if (ReadyToAttack)
        {
            Cut(damageData);
            Attacking = true;
            OnAttacked?.Invoke();
            lastTimeCut = Time.time;
            return true;
        }else
        {
            return false;
        }
    }

    private void Update()
    {
        ScaleLocal();
        LookTarget();
    }

    void LookTarget()
    {
        if (Host == null)
        {
            gameObject.transform.rotation = Quaternion.identity;
            return;
        }
        if (Attacking)
        {
            iTween.RotateTo(gameObject, iTween.Hash(
                "z", GetRotationByDirection(Host.DirectFire).z));
        } else
        {
            iTween.RotateTo(gameObject, iTween.Hash(
                "z", 0));
        }
    }

    void ScaleLocal()
    {
        if (Host == null)
        {
            transform.localScale = new Vector3(1, 1, 1);
        } else
        {
            if (isLeftDir)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            } 
            else
            {
                transform.localScale = Vector3.one;
            }
        }
    }
    public override string GetNameOfWeapon()
    {
        return "Melee " + nameOfWeapon;
    }
    protected virtual void Cut(DamageData damageData)
    {
        SetUpDamageData(damageData);
        colliderAttack.StartDamage(damageData.Clone);
        AnimaCut();
    }

    protected virtual void AnimaCut()
    {
        if (lastCutisAboveToBelow)
        {
            
            iTween.RotateTo(render.gameObject, iTween.Hash(
                "z", AngleAttack.y - 45, 
                "time", TimeShit, 
                "islocal", true, 
                "easeType", iTween.EaseType.easeOutExpo,
                "oncomplete", "onAttackComplete",
                "oncompletetarget", this.gameObject));
            lastCutisAboveToBelow = false;
            OnCutBegin?.Invoke();
        } else
        {
            if (Time.time - lastTimeCut > TimeShit*2)
            {
                iTween.RotateTo(render.gameObject, iTween.Hash(
                "z", AngleAttack.y - 45,
                "time", TimeShit/2,
                "islocal", true,
                "easeType", iTween.EaseType.easeOutCubic,
                "oncomplete", "onBeginAttackToFirstCut",
                "oncompletetarget", this.gameObject));
            } else
            {
                onBeginAttackToFirstCut();
            }
        }
    }
    private void onBeginAttackToFirstCut()
    {
        iTween.RotateTo(render.gameObject, iTween.Hash(
            "z", AngleAttack.x - 45, 
            "time", TimeShit, 
            "islocal", true, 
            "easeType", iTween.EaseType.easeOutCubic,
            "oncomplete", "onAttackComplete",
            "oncompletetarget", gameObject));
        lastCutisAboveToBelow = true;
        OnCutBegin?.Invoke();
    }

    private void onAttackComplete()
    {
        iTween.RotateTo(render.gameObject, iTween.Hash(
                "z", 0f,
                "time", 0.3f,
                "islocal", true,
                "easeType", iTween.EaseType.easeOutCubic));
        colliderAttack.EndDamage();
        Attacking = false;
        OnCutComplete?.Invoke();
    }

    protected Vector3 GetRotationByDirection(Vector3 Direction)
    {
        if (Direction.x < 0)
        {
            Direction.x = -Direction.x;
            Direction.y = -Direction.y;
        }
        return MathQ.DirectionToRotation(Direction);
    }

    protected virtual void SetUpDamageData(DamageData damageData)
    {
        damageData.Damage = SatThuong;
        damageData.BackForce = 1;
        damageData.FromMeleeWeapon = true;
    }
    
    protected virtual void OnTakeHit(ITakeHit takeHit, DamageData damage)
    {
        damage.Direction = Host.DirectFire;
        takeHit.TakeDamaged(damage);
    }

    public UnityAction OnCutBegin;
    public UnityAction OnCutComplete;


    public override void reset()
    {
        base.reset();
        if (GetComponent<iTween>() != null)
        {
            iTween[] it = GetComponents<iTween>();
            foreach (iTween iTween in it)
            {
                Destroy(iTween);
            }
        }
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    protected virtual void OnAttackBegin()
    {

    }

    protected virtual void OnAttackEnd()
    {

    }

}
