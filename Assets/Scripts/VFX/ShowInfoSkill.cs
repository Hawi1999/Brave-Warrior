using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInfoSkill : MonoBehaviour
{
    public string CodeInput = "Skill";
    [SerializeField] GameObject main;
    [SerializeField] Image fill;
    [SerializeField] Image sprite;
    [SerializeField] GameObject mainfill;
    [HideInInspector] 
    public Skill skill;
    private void Awake() { 
        if (main != null)
        main.gameObject.SetActive(false);
        if (fill != null)
            fill.fillMethod = Image.FillMethod.Radial360;
        List<SkillConnect> skill = PlayerController.PlayerCurrent.skills;
        if (skill != null && skill.Count != 0)
        {
            for (int i = 0; i < skill.Count; i++)
            {
                if (skill[i].CodeControl == CodeInput)
                {
                    SetUp(skill[i].skill);
                }
            }
        }
    }

    private void Update()
    {
        if (skill == null)
        {
            return;
        }
        float a = skill.GetCountDownPercent;
        if (a == 0)
        {
            if (mainfill != null)
                mainfill.SetActive(false);
        } else
        {
            if (mainfill != null)
                mainfill.SetActive(true);
            if (fill != null)
                fill.fillAmount = a;
        }
    }

    public void SetUp(Skill s)
    {
        this.skill = s;
        if (sprite != null)
        {
            sprite.sprite = s.sprite;
        }
        if (main != null){
            main.SetActive(true);
        }
    }
}
