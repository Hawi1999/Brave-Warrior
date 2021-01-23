using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HienThiTaiSan : MonoBehaviour
{
    public Text textDOLA;
    public Text textDia;
    public Text textTL;
    public Text textDKN;
    // Start is called before the first frame update
    void Start()
    {
        Personal.OnDOLAChanged += setTextDOLA;
        Personal.OnDiaChanged += setTextDia;
        Personal.OnTheLucChanged += setTextTL;
        Personal.OnDKNChanged += setTextDKN;
        setTextDOLA(0, Personal.DOLA);
        setTextDia(0, Personal.Dia);
        setTextDKN(0, 0);
        setTextTL(0, 0);
    }
    private void setTextDOLA(int o, int n)
    {
        textDOLA.text = n.ToString();
    }
    private void setTextDia(int o, int n)
    {
        textDia.text = n.ToString();
    }
    private void setTextTL(int o, int n)
    {
        textTL.text = Personal.TheLuc.ToString() + "/" + Personal.THELUCMAX.ToString();
    }
    private void setTextDKN(int o, int n)
    {
        textDKN.text = Personal.DKN.ToString();
    }

    private void OnDestroy()
    {
        Personal.OnDOLAChanged -= setTextDOLA;
        Personal.OnDiaChanged -= setTextDia;
        Personal.OnTheLucChanged -= setTextTL;
        Personal.OnDKNChanged -= setTextDKN;
    }
}
