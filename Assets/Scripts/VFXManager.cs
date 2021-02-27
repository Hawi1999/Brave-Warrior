using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance
    {
        get; private set;
    }
    [Header("Sprites VFX")]
    public Sprite[] SpritesDust;
    public Sprite[] SpriteFire;
    [Space]
    [Header("Prefab VFX")]
    public Dust DustPrefab;
    public Fire FirePrefab;
    [Space]
    [Header("Orther VFX")]
    public SpriteRenderer IcePrefab;
    public AnimationQ giatDien;

    public static PoolingGameObject<Dust> PoolingDust;
    public static PoolingGameObject<Fire> PoolingFire;
    public static Transform PoolingParrent;
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

    private void OnLevelWasLoaded(int level)
    {
        PoolingParrent = Instantiate(new GameObject("PoolingParrent")).transform;

        PoolingDust = new PoolingGameObject<Dust>(DustPrefab);
        PoolingFire = new PoolingGameObject<Fire>(FirePrefab);

    }
}
