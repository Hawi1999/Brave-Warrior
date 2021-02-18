using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHPPlayer : MonoBehaviour
{
    PlayerController player => PlayerController.PlayerCurrent;

    public Slider sliderHeath;
    public Slider sliderShield;
    public Slider sliderSpeedAttack;

    public Text textHealth;
    public Text textShield;
    public Image fillHealPhy;
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
            OnShieldChanged(0, player.ShieldCurrent, player.MaxShield);
            OnHealPhyChanged(0, player.CurrentHealPhy, player.MaxHealphy, false);
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

    private void OnHealPhyChanged(float a, float b, float c, bool Tied)
    {
        sliderSpeedAttack.maxValue = c;
        sliderSpeedAttack.value = b;
        if (Tied)
        {
            fillHealPhy.color = Color.red;
        }
        else
        {
            fillHealPhy.color = Color.yellow;
        }
    }

    private void OnDestroy()
    {
        PlayerController.OnHeathChanged -= OnHPChanged;
        PlayerController.OnShieldChanged -= OnShieldChanged;
        PlayerController.OnHealPhyChanged -= OnHealPhyChanged;
    }
}
