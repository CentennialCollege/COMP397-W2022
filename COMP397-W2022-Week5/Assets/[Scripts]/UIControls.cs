using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UIControls : MonoBehaviour
{
    public Slider HealthBar;
    public TMP_Text HealthBarValue;


    public void OnStartButton_Pressed()
    {
        SceneManager.LoadScene("Main");
    }

    public void TakeDamage(int damage)
    {
        int health = Int32.Parse(HealthBar.value.ToString());
        health -= damage;
        HealthBar.value = health;
    }

    public void OnHealthBar_Changed()
    {
        HealthBarValue.text = HealthBar.value.ToString();
    }
}
