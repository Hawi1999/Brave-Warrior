using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ShowHP : MonoBehaviour
{
    [SerializeField] private Slider sli;
    [SerializeField] private Image render;
    [SerializeField] private Gradient color;
    [SerializeField] private RectTransform Rtf;
    private Enemy enemy;

    private void StartUp()
    {
        enemy.OnHPChanged += HPChanged;
        enemy.OnDeath += WhenEnemyDie;
        if (enemy != null)
        {
            Vector2 a = Rtf.sizeDelta;
            Rtf.sizeDelta = new Vector2(enemy.ED.SizeHP, 0.1f);
            HPChanged(0, enemy.Heath);
        } else
        {
            Debug.Log("Enemy is null");
        }
    }

    

    public void HPChanged(int oldHP, int newHP)
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
        StartUp();
    }

    private void WhenEnemyDie(Entity entity)
    {
        enemy.OnHPChanged -= HPChanged;
        enemy.OnDeath -= WhenEnemyDie;
        gameObject.SetActive(false);
    }
}
