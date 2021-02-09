using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance
    {
        get; private set;
    }
    public Sprite[] SpritesBui;
    public Sprite[] SpritesIce;

    public SpriteRenderer IcePrefab;
    public Dust DustPrefab;
    public AnimationQ giatDien;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }


    public static void ThuHoach(VFXThuHoach Prejabs, CayTrong cay, int soluong, Vector3 Pos)
    {
        VFXThuHoach vfx = GameObject.Instantiate(Prejabs, Pos, Quaternion.identity);
        vfx.set(cay.Pic, soluong);
        Destroy(vfx.gameObject, 2f);
    }

    public static void GiatDien(Vector3 position, float timeToDestroy)
    {
        AnimationQ gd = Instantiate(Instance.giatDien, position, Quaternion.identity);
        gd.setAnimation(Random.Range(0, 4).ToString());
        Destroy(gd.gameObject, timeToDestroy);
    }

    public static void GiatDien(Transform transform, Vector3 position, float timeToDestroy)
    {
        AnimationQ gd = Instantiate(Instance.giatDien, position, Quaternion.identity, transform);
        gd.setAnimation(Random.Range(0, 3).ToString());
        Destroy(gd.gameObject, timeToDestroy);
    }
}
