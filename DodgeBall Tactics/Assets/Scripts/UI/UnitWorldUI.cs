using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI unitActionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthbarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged += healthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        healthbarImage.fillAmount = healthSystem.GetHealthNormalized();
    }
    private void UpdateActionPointsText()
    {
        unitActionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }
    private void healthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}
