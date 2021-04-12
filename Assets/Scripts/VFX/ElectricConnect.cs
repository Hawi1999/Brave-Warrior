using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricConnect : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    [SerializeField] SpriteRenderer render1;
    [SerializeField] SpriteRenderer render2;
    
    private void Awake()
    {
        if (line == null)
        {
            line = GetComponentInChildren<LineRenderer>();
        }
        if (line != null)
        {
            line.gameObject.SetActive(false);
        }
        if (render1 != null)
        {
            render1.enabled = false;
        }
        if (render2 != null)
        {
            render2.enabled = false;
        }
    }

    public void SetUp(Entity e1, Entity e2)
    {
        if (line != null)
        {
            line.gameObject.SetActive(true);
            line.startWidth = 0;
            line.endWidth = 0;
            Vector3[] a= GetPos(Vector2.Distance(e1.center, e2.center)).ToArray();
            line.positionCount = a.Length;
            line.SetPositions(a);
            if (render1 != null)
            {
                render1.enabled = true;
                render1.transform.localPosition = Vector3.zero;
                render1.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
            }
            if (render2 != null)
            {
                render2.enabled = true;
                render2.transform.localPosition = a[a.Length - 1];
                render2.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
            }
            transform.position = e1.center;
            transform.rotation = Quaternion.Euler(MathQ.DirectionToRotation((e2.center - e1.center).normalized));
            StartCoroutine(UpdateSize(0.3f));
        } else
        {
            Destroy(gameObject);
            return;
        }
    }

    IEnumerator UpdateSize(float maxtime)
    {
        float time = 0;
        float maxSize = 0.1f;
        float a = maxtime / 3;
        while (time < a)
        {
            line.startWidth = time * maxSize / a;
            line.endWidth = time * maxSize / a;
            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(a);
        while (time > 0)
        {
            line.startWidth = time * maxSize / a;
            line.endWidth = time * maxSize / a;
            time -= Time.deltaTime;
            yield return null;
        } 
        Destroy(gameObject);
    }

    List<Vector3> GetPos(float Distance)
    {
        List<Vector3> list = new List<Vector3>();
        list.Add(Vector3.zero);
        int amount = 4;
        float DistancePerPoint = Distance / amount;
        for (int i = 1; i < amount; i++)
        {
            Vector3 pos = Vector3.right * DistancePerPoint * i;
            pos.y += Random.Range(-0.5f, 0.5f);
            list.Add(pos);
        }
        list.Add(Vector3.right * Distance);
        return list;
    }
}
