using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GoTo : MonoBehaviour
{
    [SerializeField]
    private Canvas CV;
    [SerializeField]
    private SpriteRenderer sprite;
    public Text vanBan;
    private Vector3 position;
    private void Start()
    {
        position = transform.position;
        int sortOther = (int)(-10*position.y);
        CV.sortingOrder = sortOther;
        sprite.sortingOrder = sortOther;
    }

    public UnityAction<Collider2D> OnGoIn;
    public UnityAction<Collider2D> OnGoOut;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnGoIn?.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnGoOut?.Invoke(collision);
    }


}
