using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHPPlayer : MonoBehaviour
{
    PlayerController player => PlayerController.Instance;

    public Slider sliderHeath;
    public Slider sliderShield;
    public Slider sliderSpeedAttack;

    public Text textHealth;
    public Text textShield;
    private void Awake()
    {
        SetUp();
    }

    private void SetUp()
    {
        PlayerController.OnHeathChanged += OnHPChanged;
        PlayerController.OnShieldChanged += OnShieldChanged;
        PlayerController.OnHealPhyChanged += OnHealPhyChanged;
    }

    private void Start()
    {
        if (player != null)
        {
            OnHPChanged(0, player.Heath, player.MaxHP);
            OnShieldChanged(0, player.CurentShield, player.MaxShield);
            OnHealPhyChanged(0, player.CurrentHealPhy, player.MaxHealphy);
        }
    }

    private void OnHPChanged(int a, int b, int c)
    {
        sliderHeath.maxValue = c;
        sliderHeath.value = b;
        textHealth.text = b.ToString() + "/" + c.ToString();
    }

    private void OnShieldChanged(int a, int b, int c)
    {
        sliderShield.maxValue = c;
        sliderShield.value = b;
        textShield.text = b.ToString() + "/" + c.ToString();
    }

    private void OnHealPhyChanged(float a, float b, float c)
    {
        Debug.Log("Cham hoi luon");
        sliderSpeedAttack.maxValue = c;
        sliderSpeedAttack.value = b;
    }

    private void OnDestroy()
    {
        PlayerController.OnHeathChanged -= OnHPChanged;
        PlayerController.OnShieldChanged -= OnShieldChanged;
        PlayerController.OnHealPhyChanged -= OnHealPhyChanged;
    }
}
