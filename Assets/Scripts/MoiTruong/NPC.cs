using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ShowName))]
public class NPC : MonoBehaviour, IManipulation, IShowName
{
    public Animate anim;
    public Info info;
    public ShowName showname;
    public SpriteRenderer render;

    private PlayerController player => PlayerController.PlayerCurrent;

    #region Manipulation Setup
    public bool WaitingForChoose => !geting;

    bool geting = false;
    public virtual void OnChoose(IManipulation manipulation)
    {
        if (manipulation != null && manipulation as UnityEngine.Object == this)
        {
            showname.Show();
        }
        else
        {
            showname.Hide();
        }
    }

    public virtual void TakeManipulation(PlayerController host)
    {
        geting = true;
    }


    public void DeGeting()
    {
        geting = false;
    }


    #endregion

    #region Class In Class
    [System.Serializable]
    public class Animate
    {
        public Sprite[] sprites = new Sprite[2];
        public SpriteRenderer render;
        public float FPSAnimation;

        [HideInInspector] public float time_render = 0;
        [HideInInspector] public int id_render = -1;

    }
    [System.Serializable]
    public class Info
    {
        public string name;
        public Color colorName;
    }
    #endregion

    #region Start And Update
    private void Awake()
    {
        showname = GetComponent<ShowName>();
    }
    private void Start()
    {
        EnableInfo(true);
    }
    private void Update()
    {
        UpdateAnimation();
        CheckPlayerNear();
    }
    void UpdateAnimation()
    {
        if (anim != null && anim.render != null && anim.sprites != null && anim.sprites.Length != 0 && anim.time_render >= 0.4f)
        {
            anim.id_render = (anim.id_render + 1) % anim.sprites.Length;
            anim.render.sprite = anim.sprites[anim.id_render];
            anim.time_render -= 0.4f;
        } else
        {
            if (anim.time_render < 0.4f)
            {
                anim.time_render += Time.deltaTime;
            }
        }
    }
    void CheckPlayerNear()
    {
        if (isNearPlayer(1.5f) && WaitingForChoose)
        {
            ChooseMinapulation.PlayerChoose.Add(this);
        } else
        {
            ChooseMinapulation.PlayerChoose.Remove(this);
        }
    }
    private bool isNearPlayer(float Distance)
    {
        if (player == null)
            return false;
        return Mathf.Pow(transform.position.x - player.transform.position.x, 2) + Mathf.Pow(transform.position.y - player.transform.position.y,2) <= Distance * Distance;
    }
    #endregion

    #region Void
    protected void EnableInfo(bool isEnable)
    {
        if (showname != null)
        {
            if (isEnable)
            {
                showname.Show();
            }
            else
            {
                showname.Hide();
            }
        }
    }
    protected virtual void OnValidate()
    {
        if (render != null)
        {
            render.sortingLayerName = "Current";
            render.sortingOrder = (int)(-10f * transform.position.y);
        }
    }
    #endregion

    #region ShowName
    public string GetName()
    {
        if (info != null)
        {
            return info.name;
        } else
        {
            return "new NPC";
        }
    }

    public Color GetColorName()
    {
        if (info != null)
        {
            return info.colorName;
        } else
        {
            return Color.green;
        }
    }

    public SpriteRenderer GetRender()
    {
        if (anim != null)
            return anim.render;
        return null;
    }
    #endregion


}
