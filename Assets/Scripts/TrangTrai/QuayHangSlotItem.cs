using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuayHangSlotItem : SlotItem
{
    [SerializeField]
    private GameObject Selected;
    [SerializeField]
    private Button ThaoTac;
    public override void setSelectedActive(bool tf)
    {
        Selected.SetActive(tf);
    }
    public override void setButtonAction(UnityAction action, ButtonAction bt)
    {
        if (bt == ButtonAction.Add)
            ThaoTac.onClick.AddListener(action);
        if (bt == ButtonAction.Set)
        {
            ThaoTac.onClick.RemoveAllListeners();
            ThaoTac.onClick.AddListener(action);
        }
        if (bt == ButtonAction.Del)
            ThaoTac.onClick.RemoveListener(action);

    }

    public override GameObject getSelected()
    {
        return Selected;
    }
}
