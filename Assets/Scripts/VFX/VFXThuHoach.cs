using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VFXThuHoach : MonoBehaviour
{
    [SerializeField]
    private Image Picture;
    [SerializeField]
    private Text Amount;
    private float TocDo = 0.5f;

    public void set(Sprite pic, int soluong)
    {
        Picture.sprite = pic;
        Amount.text = "+" + soluong.ToString(); 
    }

    private void Update()
    {
        transform.position = transform.position + Vector3.up * TocDo * Time.deltaTime;
    }
}
