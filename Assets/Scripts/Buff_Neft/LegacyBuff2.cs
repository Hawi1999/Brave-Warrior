using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LegacyBuff1", menuName = "Buff/LegacyBuff2 (Tăng Điểm hồi kĩ năng)")]
public class LegacyBuff2 : LegacyBuff
{
    int type = BuffRegister.TypeBuff.DecreaseCoolDownSkillByFix100;
    [SerializeField] Sprite[] sprites;
    [SerializeField] float[] ValueFix100;


    public override Sprite Sprite => sprites[ID];

    [HideInInspector] public int ID = 0;
    private float Value => ValueFix100[ID];
    protected override string ThongBao => base.ThongBao + "+" + Value;
    public override void OnHostTake(Entity entity)
    {
        if (entity is PlayerController)
        {
            base.OnHostTake(entity);
            entity.take.Register(this, type, Value);
        }
    }

    protected override void OnCreateClone(LegacyBuff clone)
    {
        base.OnCreateClone(clone);
        int max = Mathf.Max(sprites.Length, ValueFix100.Length);
        (clone as LegacyBuff2).ID = Random.Range(0, max);
    }

}
