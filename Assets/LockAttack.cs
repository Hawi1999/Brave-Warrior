using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockAttack : MonoBehaviour
{
    Entity entity;
    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();
        entity.OnCheckForAttack += Lock;
    }

    private void Lock(BoolAction a)
    {
        a.Priority_level_false = 1;
    }


}
