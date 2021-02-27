using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlX : Control
{
    public Vector3 rotationDefault;
    public float rotationSpeed;

    bool inIt = false;

    protected override void Start()
    {
        base.Start();
        transform.rotation = Quaternion.Euler(rotationDefault);
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (inIt)
        {
            Vector3 old = transform.rotation.eulerAngles;
            iTween.RotateUpdate(gameObject, iTween.Hash(
                "rotation",new Vector3(0,0,old.z - rotationSpeed * Time.fixedDeltaTime)));
        }
    }

    public override void WaitToClick(string x)
    {
        base.WaitToClick(x);
        if (x == IDCODE)
        {
            inIt = true;
        }
    }

    public override void EndWaitToClick(string x)
    {
        base.EndWaitToClick(x);
        if (x == IDCODE)
        {
            inIt = false;
            transform.rotation = Quaternion.Euler(rotationDefault);
        }
    }
}
