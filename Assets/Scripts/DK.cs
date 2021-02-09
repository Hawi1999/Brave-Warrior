using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DK : MonoBehaviour
{
    public Joystick MyJoy { get => GameController.MyJoy; }

    PlayerController player => PlayerController.PlayerCurrent;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (MyJoy != null && player != null)
        {
            Vector2 dire = MyJoy.Direction;
            player.Move(dire.normalized);
            if (dire != Vector2.zero)
                player.Direction = dire;
        } else
        {
            Debug.LogWarning("Khong tim thay he thong dieu khien hoac player khong ton tai");
        }
    }
}
