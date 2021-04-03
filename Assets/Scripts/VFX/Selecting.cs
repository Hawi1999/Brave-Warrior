using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Selecting : MonoBehaviour
{
    private IFindTarget target;
    [SerializeField] float speedRotate = 36;

    private SpriteRenderer render;

    private float start;
    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        start = Random.Range(0, 0.5f);
        OnValueChange(start);
    }

    public void StartUp(IFindTarget target)
    {
        this.target = target;
        if (Random.Range(0, 2) == 1)
        {
            StartAlpha05(start);
        } else
        {
            StartAlpha0(start);
        }
    }

    private void Update()
    {
        if (target == null || target as Object == null)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.position = target.center;
        transform.localScale = Mathf.Max(target.size.x, target.size.y) * Vector3.one * 2.4f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + speedRotate * Time.deltaTime));

    }

    #region Call By iTween
    private void OnAlpha05()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 0.5f,
            "to", 0,
            "time", 2,
            "onupdate", "OnValueChange",
            "easetype", iTween.EaseType.linear,
            "oncomplete", "OnAlpha0")); ;
    }

    private void StartAlpha05(float start)
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", start,
            "to", 0,
            "time", start * 2,
            "onupdate", "OnValueChange",
            "easetype", iTween.EaseType.linear,
            "oncomplete", "OnAlpha0")); ;
    }
    private void StartAlpha0(float start)
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", start,
            "to", 0.5f,
            "time", (0.5 - start) * 2,
            "onupdate", "OnValueChange",
            "easetype", iTween.EaseType.linear,
            "oncomplete", "OnAlpha05"));
    }

    private void OnAlpha0()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 0,
            "to", 0.5f,
            "time", 1,
            "onupdate", "OnValueChange",
            "easetype", iTween.EaseType.linear,
            "oncomplete", "OnAlpha05"));
    }

    private void OnValueChange(float a)
    {
        Color c = render.color;
        c.a = a;
        render.color = c;
    }

    #endregion
}
