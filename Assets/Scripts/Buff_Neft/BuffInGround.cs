using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ShowName))]
public class BuffInGround : Reward, IShowName, IManipulation
{
    Buff2Data Data;
    [SerializeField] SpriteRenderer render;
    [SerializeField] SpriteRenderer renderVFX;
    private ShowName showname;
    public override string Name => "Buff " + Data.name;
    public override bool WaitingForChoose =>  !appearing && base.WaitingForChoose;


    private bool appearing = true;

    float timeVFX = 1;
    protected override void Awake()
    {
        base.Awake();
        showname = GetComponent<ShowName>();
    }

    private void Start()
    {
        showname.Hide();
        StartCoroutine(AnimationStart());

    }
    IEnumerator AnimationStart() 
    { 
        Color color = render.color;
        color.a = 0;
        render.color = color;
        float time = 0;
        yield return null;
        while (time < timeVFX)
        {
            time += Time.time;
            color = render.color;
            color.a = time / timeVFX;
            render.color = color;
            yield return null;
        }
        color = render.color;
        color.a = 1;
        render.color = color;
        appearing = false;
    }

    IEnumerator AnimationEnd()
    {
        float s = render.transform.localScale.x;
        float speed = 2f;
        float time = 0;
        while (time < timeVFX)
        {
            time += Time.deltaTime;
            render.transform.localScale = Vector3.one * (1 - time / timeVFX) * s;
            render.transform.rotation = Quaternion.Euler(new Vector3(0, 0, render.transform.rotation.eulerAngles.z + speed * 360 * Time.deltaTime));
            yield return null;
        }
        Destroy(this.gameObject);
    }

    public override void OnPlayerInto()
    {
        
    }

    public override void OnChoose(IManipulation manipulation)
    {
        if (manipulation != null && manipulation as Object == this)
        {
            showname.Show();
        }
        else
        {
            showname.Hide();
        }
    }

    public override void TakeManipulation(PlayerController host)
    {
        if (!taked)
        {
            base.TakeManipulation(host);
            Data.Register(host.take);
            StartCoroutine(AnimationEnd());
        }
    }

    public void SetUp(Buff2Data buff)
    {
        Data = buff;
        render.sprite = Data.sprite;
    }

    public string GetName()
    {
        return "Reward " + Name;
    }

    public Color GetColorName()
    {
        return Data.colorName;
    }

    public SpriteRenderer GetRender()
    {
        return render;
    }
}
