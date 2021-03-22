using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PoolingGameObject
{
    public static PoolingGameObject PoolingMain = new PoolingGameObject();
    List<PoolingType> list_Pooling;
    List<PoolingType> pooledGobjects;
    Transform parentPooling => VFXManager.PoolingParrent;

    bool[] hihi = new bool[100000];

    public int AddPrefab(PoolingBehaviour prefabs)
    {
        if (pooledGobjects == null)
        {
            pooledGobjects = new List<PoolingType>();
        }
        if (list_Pooling == null)
        {
            list_Pooling = new List<PoolingType>();
        }
        int id = NextCode;
        list_Pooling.Add(new PoolingType(prefabs, id));
        return id;
    }

    int NextCode
    {
        get
        {
            for (int i = 1; i < 100000; i++)
            {
                if (!hihi[i])
                {
                    hihi[i] = true;
                    return i;
                };
            }
            return 0;
        }
    }

    public void RemovePrefab(int id)
    {
        List<PoolingType> listRemove = new List<PoolingType>();
        if (list_Pooling == null || list_Pooling.Count == 0)
        {
            return;
        }
        foreach (PoolingType poolingType in list_Pooling)
        {
            if (poolingType.id == id)
            {
                listRemove.Add(poolingType);
            }
        }
        foreach (PoolingType pooling in listRemove)
        {
            hihi[pooling.id] = false;
            list_Pooling.Remove(pooling);
        }
        RemoveAllPooled(id);
    }

    public class PoolingType
    {
        public PoolingBehaviour gameobject;
        public int id;

        public PoolingType(PoolingBehaviour prefabs, int id)
        {
            this.id = id;
            this.gameobject = prefabs;
        }
    }

    public PoolingBehaviour Spawn(int id, Transform transform)
    {
        PoolingBehaviour t = null;
        if (pooledGobjects != null && pooledGobjects.Count != 0)
        {
            List<PoolingType> listRemove = new List<PoolingType>();
            foreach (PoolingType poolingType in pooledGobjects)
            {
                if (poolingType.gameobject == null)
                {
                    listRemove.Add(poolingType);
                    continue;
                }
                if (poolingType.id == id && poolingType.gameobject.isReady)
                {
                    t = poolingType.gameobject;
                    break;
                }
            }
            foreach(PoolingType poolingType1 in listRemove)
            {
                pooledGobjects.Remove(poolingType1);
            }
        }
        if (t == null)
        {
            t = CreateGobject(id);
        }
        if (transform == null)
        {
            t.transform.parent = parentPooling;
            t.transform.position = Vector3.zero;
        } else
        {
            t.transform.parent = transform;
            t.transform.position = transform.position;
        }
        t.Begin();
        return t;
    }
    public PoolingBehaviour Spawn(int id, Vector3 position, Quaternion rotation)
    {
        PoolingBehaviour t = Spawn(id, null);
        t.transform.position = position;
        t.transform.rotation = rotation;
        return t;
    }

    public PoolingBehaviour Spawn(int id, Vector3 position, Transform transform)
    {
        PoolingBehaviour t = Spawn(id, transform);
        t.transform.position = position;
        return t;
    }

    public PoolingBehaviour Spawn(int id, Vector3 position, Quaternion rotation, Transform transform)
    {
        PoolingBehaviour t = Spawn(id, position, transform);
        t.transform.rotation = rotation;
        return t; 
    }

    PoolingBehaviour CreateGobject(int id)
    {
        PoolingBehaviour prefab = null;
        if (list_Pooling == null || list_Pooling.Count == 0)
        {

        } else
        {
            foreach (PoolingType poolingType in list_Pooling)
            {
                if (poolingType.id == id)
                {
                    prefab = poolingType.gameobject;
                    break;
                }
            }
        }
        PoolingBehaviour ins = GameObject.Instantiate(prefab);
        pooledGobjects.Add(new PoolingType(ins, id));
        return ins;
    }

    public void RemoveAllPooled()
    {
        List<PoolingType> listRemove = new List<PoolingType>();
        foreach (PoolingType poolingType in pooledGobjects)
        {
            if (poolingType.gameobject != null)
            {
                poolingType.gameobject.Rest();
            }
            GameObject.Destroy(poolingType.gameobject.gameObject);
            listRemove.Add(poolingType);
        }
        foreach (PoolingType poolingType1 in  listRemove)
        {
            pooledGobjects.Remove(poolingType1);
        }
    }

    public void RemoveAllPooled(int id)
    {
        List<PoolingType> listRemove = new List<PoolingType>();
        if (pooledGobjects == null || pooledGobjects.Count == 0)
        {
            return;
        }
        foreach (PoolingType poolingType in pooledGobjects)
        {
            if (poolingType.id == id)
            {
                if (poolingType.gameobject != null)
                {
                    if (poolingType.gameobject.isReady)
                    {
                        GameObject.Destroy(poolingType.gameobject.gameObject);
                    } else
                    {
                        poolingType.gameobject.DestroyWhenDone = true;
                    }
                }
                listRemove.Add(poolingType);
            }
        }
        foreach (PoolingType poolingType1 in listRemove)
        {
            pooledGobjects.Remove(poolingType1);
        }
    }



}
