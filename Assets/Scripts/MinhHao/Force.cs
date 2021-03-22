using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ForceElement
{
    Vector2 Direction;
    float time;
    float force;
    float timeStart;
    public ForceElement(Vector2 Direction, float time, float force)
    {
        timeStart = Time.time;
        this.time = time;
        this.Direction = Direction;
        this.force = force;
    }

    public bool Update(GameObject game, out Vector3 amount)
    {
        amount = Vector3.zero;
        if (Time.time - timeStart >= time)
        {
            return true;
        }
        amount = Direction * force * Time.fixedDeltaTime;
        return false;
    }

    public bool FixedUpdate(GameObject game, out Vector3 amount)
    {
        amount = Vector3.zero;
        if (Time.time - timeStart >= time)
        {
            return true;
        }
        Rigidbody2D rig = game.GetComponent<Rigidbody2D>();
        amount = Direction * force * Time.fixedDeltaTime;
        return false;
    }
}

public class Force : MonoBehaviour
{
    private Rigidbody2D rig;
    private bool phys;
    List<ForceElement> listf;
    private void Awake()
    {
        if (GetComponent<Rigidbody2D>() != null)
        {
            rig = GetComponent<Rigidbody2D>();
            phys = true;
        }
    }

    private void Update()
    {
        if (!phys)
        {
            Vector3 amountotal = Vector3.zero;
            List<ForceElement> listRemove = new List<ForceElement>();
            foreach (ForceElement fe in listf)
            {
                if (fe.Update(gameObject, out Vector3 amount))
                {
                    listRemove.Add(fe);
                } else
                {
                    amountotal += amount;
                }
            }
            foreach (ForceElement fe in listRemove)
            {
                listf.Remove(fe);
            }
            if (listf.Count == 0)
            {
                Destroy(this);
            } else
            {
                transform.position = transform.position + amountotal;
            }
        }
    }

    private void FixedUpdate()
    {
        if (phys)
        {
            Vector3 amounttotal = Vector3.zero;
            List<ForceElement> listRemove = new List<ForceElement>();
            foreach (ForceElement fe in listf)
            {
                if (fe.FixedUpdate(gameObject, out Vector3 amount))
                {
                    listRemove.Add(fe);
                } else
                {
                    amounttotal += amount;
                }
            }
            foreach (ForceElement fe in listRemove)
            {
                listf.Remove(fe);
            }
            if (listf.Count == 0)
            {
                Destroy(this);
            } else
            {
                rig.MovePosition(transform.position + amounttotal);
            }
        }
    }
    public static void BackForce(GameObject game, Vector2 Direction, float force, float time)
    {
        ForceElement fe = new ForceElement(Direction, time, force);
        if (game.TryGetComponent(out Force f))
        {
            f.Add(fe);
        } else
        {
            game.AddComponent<Force>().Add(fe);
        }
    }

    public void Add(ForceElement fe)
    {
        if (listf == null)
        {
            listf = new List<ForceElement>();
        }
        listf.Add(fe);
    }
}
