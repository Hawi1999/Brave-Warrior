using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ShowHPSub : MonoBehaviour
{
    [SerializeField] Text T;
    [SerializeField] Vector3 RandomRange;
    [SerializeField] float TimeToDestroy;
    [SerializeField] float TocDoBay;
    private Vector3 localTarget = new Vector3(0,1,0);

    private float t = 0;

    private void Start()
    {
        Vector3 crposition = transform.localPosition;
        Vector3 newposition = new Vector3(crposition.x + Random.Range(-RandomRange.x, RandomRange.x), crposition.y + Random.Range(-RandomRange.y, RandomRange.y), crposition.z);
        transform.localPosition = newposition;
        localTarget = newposition + new Vector3(0, 1, 0);
    }

    private void Update()
    {
        Vector3 pos = transform.localPosition;
        Vector3 newPos = Vector3.Lerp(pos, localTarget, TocDoBay);
        transform.localPosition = newPos;
        t += Time.deltaTime;
        if (t > TimeToDestroy) Destroy(this.gameObject);
    }
    public void StartUp(int Damage, [DefaultValue(DamageElement.Normal)] DamageElement ele)
    {
        Color color = Color.yellow;
        if (ele == DamageElement.Fire)
        {
            color = Color.red;
        }
        if (ele == DamageElement.Ice)
        {
            color = Color.blue;
        }
        if (ele == DamageElement.Poison)
        {
            color = Color.green;
        }
        T.text = Damage.ToString();
        T.color = color;
    }
}

