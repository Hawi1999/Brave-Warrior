using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HienThiTaiSan : MonoBehaviour
{
    public Text textDOLA;
    public Text textDia;
    public Text textCoin;
    // Start is called before the first frame update
    void Start()
    {
        if (textDOLA != null)
        {
            Personal.OnDOLAChanged += setTextDOLA;
            setTextDOLA(0, Personal.DOLA);
        }
        if (textDia != null)
        {
            Personal.OnDiaChanged += setTextDia;
            setTextDia(0, Personal.Dia);
        }
        if (textCoin != null)
        {
            Personal.OnCoinChanged += setTextCoin;
            setTextCoin(0, Personal.Coin);
        }

    }
    private void setTextDOLA(int o, int n)
    {
        textDOLA.text = n.ToString();
    }
    private void setTextDia(int o, int n)
    {
        textDia.text = n.ToString();
    }

    private void setTextCoin(int o, int n)
    {
        textCoin.text = n.ToString();
    }

    private void OnDestroy()
    {

        if (textDOLA != null)
        {
            Personal.OnDOLAChanged -= setTextDOLA;
        }
        if (textDia != null)
        {
            Personal.OnDiaChanged -= setTextDia;
        }
        if (textCoin != null)
        {
            Personal.OnCoinChanged -= setTextCoin;
        }
    }
}
