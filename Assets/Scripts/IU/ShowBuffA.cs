using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBuffA : MonoBehaviour
{
    public Image ItemPrefab;
    public Transform parrent;
    TakeBuff playertakebuff => PlayerController.PlayerTakeBuff;
    private List<Image> list = new List<Image>();
    private void Awake()
    {
        if (playertakebuff != null)
        {
            playertakebuff.OnValueChanged += WhenValueBuffChanged;
        }
    }

    private void Start()
    {
        WhenValueBuffChanged(TakeBuff.AMOUNT_GIVEBUFF_REGISTED);
    }


    void WhenValueBuffChanged(int a)
    {
        if (a == TakeBuff.AMOUNT_GIVEBUFF_REGISTED)
        {
            ResetList(playertakebuff.GetALlBuffA());
        }
    }

    void ResetList(List<IShowBuffA> buffs)
    {
        for (int i = list.Count - 1; i > -1; i--)
        {
            Destroy(list[i].gameObject);
        }
        list.Clear();
        for (int i = 0; i < buffs.Count; i++)
        {
            Image s = Instantiate(ItemPrefab, parrent);
            if (buffs[i] != null && buffs[i] as UnityEngine.Object != null)
            {
                s.sprite = buffs[i].Sprite;
                list.Add(s);
            }
        }
    }

    private void OnDestroy()
    {
        
    }
}
