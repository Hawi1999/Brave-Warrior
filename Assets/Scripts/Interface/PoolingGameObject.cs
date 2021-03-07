using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PoolingGameObject<T> where T: PoolingBehaviour
{
    int Count = 0;
    T Prefab;
    List<T> pooledGobjects;
    Transform parentPooling => VFXManager.PoolingParrent;
    public PoolingGameObject(T t)
    {
        SetUp(t);
    }
    public void SetUp(T t)
    {
        pooledGobjects = new List<T>();
        Prefab = t;
    }

    public T Spawn(Transform transform)
    {
        T t = null;
        if (pooledGobjects != null && pooledGobjects.Count != 0)
        {
            foreach (T pool in pooledGobjects)
            {
                if (pool == null)
                {
                    pooledGobjects.Remove(pool);
                    continue;
                }
                if (pool.isReady)
                {
                    t = pool;
                    pool.Begin();
                    break;
                }
            }
        }
        if (t == null)
        {
            t = CreateGobject();
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
        return t;
    }
    public T Spawn(Vector3 position, Quaternion rotation)
    {
        T t = Spawn(null);
        t.transform.position = position;
        t.transform.rotation = rotation;
        return t;
    }

    public T Spawn(Vector3 position, Transform transform)
    {
        T t = Spawn(transform);
        t.transform.position = position;
        return t;
    }

    public T Spawn(Vector3 position, Quaternion rotation, Transform transform)
    {
        T t = Spawn(position, transform);
        t.transform.rotation = rotation;
        return t;
    }

    T CreateGobject()
    {
        T gobject = GameObject.Instantiate(Prefab);
        gobject.name = "Pooling " + Prefab.name + " " + Count++;
        gobject.Begin();
        pooledGobjects.Add(gobject);
        return gobject;
    }

    public void DestroyAll()
    {
        for (int i = 0; i < pooledGobjects.Count; i++)
        {
            if (pooledGobjects[i] == null)
            {
                continue;
            }
            GameObject a = pooledGobjects[i].gameObject;
            pooledGobjects[i].DestroyWhenDone = true;
            pooledGobjects.Remove(pooledGobjects[i]);
        }
    }



}
