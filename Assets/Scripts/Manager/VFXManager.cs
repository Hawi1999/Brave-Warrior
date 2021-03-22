using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [Header("Sprites VFX")]
    public static Sprite[] SpritesDust;
    public static Sprite[] SpritesIce;
    [Space]
    [Header("Prefab VFX")]
    public static Dust DustPrefab;
    public static ControlPartice FirePrefab;
    public static ControlPartice PoisonPrefab;
    public static SelectingEnemy SelectingEnemyPrefab;
    [Space]
    [Header("Orther VFX")]
    public static SpriteRenderer IcePrefab;
    public static AnimationQ giatDien;

    public static int IDPooling_Fire;
    public static int IDPooling_Dust;
    public static int IDPooling_Poison;
    public static Transform PoolingParrent;
    public enum TypeLoad
    {
        All,
    }

    public static void LoadData()
    {
        // Load Prefabs
        FirePrefab = Resources.Load<ControlPartice>("Prefabs/VFXBurnt");
        PoisonPrefab = Resources.Load<ControlPartice>("Prefabs/VFXPoison");
        DustPrefab = Resources.Load<Dust>("Prefabs/Bui");
        SelectingEnemyPrefab = Resources.Load<SelectingEnemy>("Prefabs/SelectingEnemy");
        giatDien = Resources.Load<AnimationQ>("Prefabs/VFXGiatDien");
        IcePrefab = Resources.Load<SpriteRenderer>("Prefabs/Ice");
        // Load Sprites

        SpritesDust = Resources.LoadAll<Sprite>("Sprites/bui");
        SpritesIce = Resources.LoadAll<Sprite>("Sprites/Ices");

        // LoadPooling
        PoolingParrent = Instantiate(new GameObject("PoolingParrent")).transform;
        PoolingParrent.transform.parent = GameController.Instance.transform;
        IDPooling_Fire = PoolingGameObject.PoolingMain.AddPrefab(FirePrefab);
        IDPooling_Dust = PoolingGameObject.PoolingMain.AddPrefab(DustPrefab);
        IDPooling_Poison = PoolingGameObject.PoolingMain.AddPrefab(PoisonPrefab);

        Debug.Log("VFX Loaded");
    }

    public static void FreeData()
    {

    }

    public static void ThuHoach(VFXThuHoach Prejabs, CayTrong cay, int soluong, Vector3 Pos)
    {
        VFXThuHoach vfx = GameObject.Instantiate(Prejabs, Pos, Quaternion.identity);
        vfx.set(cay.Pic, soluong);
        Destroy(vfx.gameObject, 2f);
    }

    public static void GiatDien(Vector3 position, float timeToDestroy)
    {
        AnimationQ gd = Instantiate(giatDien, position, Quaternion.identity);
        gd.setAnimation(Random.Range(0, 4).ToString());
        Destroy(gd.gameObject, timeToDestroy);
    }

    public static void GiatDien(Transform transform, Vector3 position, float timeToDestroy)
    {
        AnimationQ gd = Instantiate(giatDien, position, Quaternion.identity, transform);
        gd.setAnimation(Random.Range(0, 3).ToString());
        Destroy(gd.gameObject, timeToDestroy);
    }
}
