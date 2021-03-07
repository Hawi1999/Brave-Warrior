using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    [SerializeField] private Sprite[] spriteElement;
    [SerializeField] private DamageElement[] connectSprite;


    private List<DamageElement> listele = new List<DamageElement>();
    private Enemy enemy;

    private void StartUp()
    {
        enemy.OnHPChanged += HPChanged;
        enemy.OnDeath += WhenEnemyDie;
        enemy.OnHide += () => OnEnemyHide(true);
        enemy.OnAppear += () => OnEnemyHide(false);
        if (enemy != null)
        {
            Vector2 a = Rtf.sizeDelta;
            Rtf.sizeDelta = new Vector2(enemy.size.x, 0.1f);
            HPChanged(0, enemy.Heath, enemy.MaxHP);
        } else
        {
            Debug.Log("Enemy is null");
        }
    }


    private void OnEnemyHide(bool a)
    {
        gameObject.SetActive(!a);
        ParentShowBuff?.gameObject.SetActive(!a);
    }

    private void UpdateListBuff()
    {
        if (spriteElement == null || connectSprite == null || spriteElement.Length == 0 || connectSprite.Length == 0 || spriteElement.Length != connectSprite.Length)
        {
            Debug.Log("Dữ liệu cho showbuff ko rõ ràng");
            return;
        }
        for (int i = 0; i < ParentShowBuff.childCount; i++)
        {
            Destroy(ParentShowBuff.GetChild(i).gameObject);
        }
        for (int i = 0; i < listele.Count; i++)
        {
            for (int j = 0; j < connectSprite.Length; j++)
            {
                if (connectSprite[j] == listele[i])
                {
                    Instantiate(PrefabsShowBuffs, ParentShowBuff).sprite = spriteElement[j];
                }
            }
        }
    }

    private void AddBuff(DamageElement ele)
    {
        if (!BuffExist(ele))
        {
            listele.Add(ele);
            UpdateListBuff();
        }
    }

    private bool BuffExist(DamageElement ele)
    {
        foreach (DamageElement elem in listele)
        {
            if (ele == elem)
            {
                return true;
            }
        }
        return false;
    }

    private void RemoveBuff(DamageElement ele)
    {
        if (BuffExist(ele))
        {
            listele.Remove(ele);
            UpdateListBuff();
        }
    }


    public void HPChanged(int oldHP, int newHP, int maxHP)
    {
        if (enemy == null)
        {
            Debug.Log("Không tìm thấy quái vật để hiện thị máu");
            return;
        }
        float a = ((float)(newHP)) / enemy.MaxHP;
        sli.value = a;
        render.color = color.Evaluate(a);
    }

    public void SetStart(Enemy target)
    {
        enemy = target;
        enemy.OnBuffsChanged += OnBuffSet;
        StartUp();
    }

    private void WhenEnemyDie(Entity entity)
    {
        enemy.OnHPChanged -= HPChanged;
        enemy.OnDeath -= WhenEnemyDie;
        enemy.OnHide -= () => OnEnemyHide(true);
        enemy.OnAppear -= () => OnEnemyHide(false);
        gameObject.SetActive(false);
    }

    private void OnBuffSet(DamageElement ele, bool isOK)
    {
        if (isOK)
        {
            AddBuff(ele);
        }
        else
        {
            RemoveBuff(ele);
        }
    }
}
