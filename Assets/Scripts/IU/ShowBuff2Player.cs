using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBuff2Player : MonoBehaviour
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
        WhenValueBuffChanged(TakeBuff.AMOUNT_BUFF2DATA_REGISTED);
    }


    void WhenValueBuffChanged(int a)
    {
        if (a == TakeBuff.AMOUNT_BUFF2DATA_REGISTED)
        {
            ResetList(playertakebuff.GetALlGiveBuff<Buff2Data>());
        }
    }

    void ResetList(List<Buff2Data> buffs)
    {
        for (int i = list.Count - 1; i > -1; i--)
        {
            Destroy(list[i].gameObject);
        }
        list.Clear();
        for (int i = 0; i < buffs.Count; i++)
        {
            Image s = Instantiate(ItemPrefab, parrent);
            s.sprite = buffs[i].sprite;
            list.Add(s);
        }
    }

    private void OnDestroy()
    {
        
    }
}
