using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShowHPEnemy : MonoBehaviour
{
    [SerializeField] private Slider sli;
    [SerializeField] private Image render;
    [SerializeField] private Gradient color;
    [SerializeField] private RectTransform Rtf;
    [SerializeField] private Transform ParentShowBuff;
    [SerializeField] private Image PrefabsShowBuffs;
    private List<Buff1Datas> listBuffs;

    private Enemy enemy;
    private void Awake()
    {
        listBuffs = new List<Buff1Datas>();

    }

    private void StartUp()
    {
        if (enemy != null)
        {
            AddEvents();
            Rtf.sizeDelta = new Vector2(enemy.size.x, 0.1f);
            HPChanged();
        } else
        {
            Debug.Log("Enemy is null");
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        transform.position = enemy.PR_HP.position;
    }

    private void OnEnemyHide(bool a)
    {
        gameObject.SetActive(!a);
        ParentShowBuff?.gameObject.SetActive(!a);
    }

    private void UpdateListBuff()
    {
        for (int i = 0; i < ParentShowBuff.childCount; i++)
        {
            Destroy(ParentShowBuff.GetChild(i).gameObject);
        }
        for (int i = 0; i < listBuffs.Count; i++)
        {
            Image s = Instantiate(PrefabsShowBuffs, ParentShowBuff);
            s.sprite = listBuffs[i].sprite;
        }
    }

    private void AddBuff(Buff1Datas ele)
    {
        if (!BuffExist(ele))
        {
            listBuffs.Add(ele);
            UpdateListBuff();
        }
    }

    private bool BuffExist(Buff1Datas ele)
    {
        foreach (Buff1Datas elem in listBuffs)
        {
            if (ele == elem)
            {
                return true;
            }
        }
        return false;
    }

    private void RemoveBuff(Buff1Datas ele)
    {
        if (BuffExist(ele))
        {
            listBuffs.Remove(ele);
            UpdateListBuff();
        }
    }


    public void HPChanged()
    {
        if (enemy == null)
        {
            Debug.Log("Không tìm thấy quái vật để hiện thị máu");
            return;
        }
        float a = ((float)enemy.CurrentHeath / enemy.MaxHP);
        sli.value = a;
        a = Mathf.Clamp01(a);
        render.color = color.Evaluate(a);
    }

    public void SetStart(Enemy target)
    {
        enemy = target;
        StartUp();
    }

    private void WhenEnemyDie(Entity entity)
    {
        RemoveEvents();
        Destroy(gameObject);
    }
    private void AddEvents()
    {
        enemy.OnValueChanged += OnHasValueChaned;
        enemy.OnDeath += WhenEnemyDie;
        enemy.OnHide += () => OnEnemyHide(true);
        enemy.OnAppear += () => OnEnemyHide(false);

    }

    private void RemoveEvents()
    {
        if (enemy != null)
        {
            enemy.OnValueChanged -= OnHasValueChaned;
            enemy.OnDeath -= WhenEnemyDie;
            enemy.OnHide -= () => OnEnemyHide(true);
            enemy.OnAppear -= () => OnEnemyHide(false);
        }
    }

    private void OnHasValueChaned(int c)
    {
        #region Buff
        string code = string.Empty ;
        bool value = false;
        bool hasBuff = true;
        if (c == Entity.HARMFUL_POISON)
        {
            code = "Poison";
            value = enemy.Harmful_Poison;
        } else
        if (c == Entity.HARMFUL_ICE)
        {
            code = "Ice";
            value = enemy.Harmful_Ice;
        } else
        if (c == Entity.HARMFUL_FIRE)
        {
            code = "Fire";
            value = enemy.Harmful_Fire;
        } else
        if (c == Entity.HARMFUL_ELECTIC)
        {
            code = "Electric";
            value = enemy.Harmful_Electric;
        } else
        {
            hasBuff = false;
        }
        if (hasBuff)
        {
            Buff1Datas b = DataMap.GetBuff(code);
            if (b != null)
            {
                if (value)
                {
                    AddBuff(b);
                }
                else
                {
                    RemoveBuff(b);
                }
            }
        }
        #endregion

        if (c == Entity.HP || c == Entity.MAPHP)
        {
            HPChanged();
        }
    }
}
