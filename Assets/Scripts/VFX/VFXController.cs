using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController
{
    public static void ThuHoach(VFXThuHoach Prejabs,  CayTrong cay, int soluong, Vector3 Pos)
    {
        VFXThuHoach vfx = GameObject.Instantiate(Prejabs);
        vfx.transform.position = Pos;
        vfx.set(cay.Pic, soluong);
        GameObject.Destroy(vfx.gameObject, 2f);
    }
}