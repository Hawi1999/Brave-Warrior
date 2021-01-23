using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DK : MonoBehaviour
{
    public Joystick MyJoy { get => GameController.MyJoy; }
    
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (MyJoy != null)
        {
            Vector2 dire = MyJoy.Direction;
            player.Move(dire.normalized);
            if (dire != Vector2.zero)
                player.Direction = dire;
        } else
        {
            Debug.LogWarning("Khong tim thay he thong dieu khien");
        }
    }
}
