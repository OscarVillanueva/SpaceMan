using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    [SerializeField] private BarType barType;

    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();

        switch (barType)
        {
            case BarType.healthBar:
                slider.maxValue = PlayerController.MAX_HEALTH;
                break;
            case BarType.manaBar:
                slider.maxValue = PlayerController.MAX_MANA;
                break;
        }

    }

    private void Update()
    {
        switch (barType)
        {
            case BarType.healthBar:
                slider.value = FindObjectOfType<PlayerController>().GetHealth();
                break;
            case BarType.manaBar:
                slider.value = FindObjectOfType<PlayerController>().GetMana();
                break;
        }

    }
}
