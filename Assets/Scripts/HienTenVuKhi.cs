using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HienTenVuKhi : MonoBehaviour
{
    public Sprite Name;
    public Sprite MuiTen;
    public Vector3 LocalPosition;

    private GameObject ThongTin;
    private SpriteRenderer renderName;
    private SpriteRenderer renderMT;
    private Weapon weapon => GetComponent< Weapon>();
    public void Start()
    {
        ThongTin = Instantiate(new GameObject("Thong Tin"), transform);
        if (Name != null)
        {
            renderName = Instantiate(new GameObject("Name"), transform.position + LocalPosition + new Vector3(0,0.2f,0), Quaternion.identity, ThongTin.transform).AddComponent<SpriteRenderer>();
            renderName.sprite = Name;
            renderName.sortingLayerName = "Skin";
            renderName.sortingOrder = 15;
            renderName.color = Weapon.getColorByLevelWeapon(weapon.TypeOfWeapon);
        } else
        {
            Debug.Log("Chưa có hình ảnh tên cho súng " + weapon.GetNameOfWeapon());
        }
        if (MuiTen != null)
        {
            renderMT = Instantiate(new GameObject("MT"), transform.position + LocalPosition, Quaternion.identity, ThongTin.transform).AddComponent<SpriteRenderer>();
            renderMT.sprite = MuiTen;
            renderMT.sortingLayerName = "Skin";
            renderMT.sortingOrder = 15;
        } else
        {
            Debug.Log("Chưa có hình ảnh mũi tên cho súng " + weapon.GetNameOfWeapon());
        }
        AnDi();
    }

    public void HienLen()
    {
        if (ThongTin != null)
            ThongTin.SetActive(true);
    }

    public void AnDi()
    {
        if (ThongTin != null)
            ThongTin.SetActive(false);
    }


}

