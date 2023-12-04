using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    public Slider enemyHealthBar;
    public static EnemyHealthBarUI enemyHealthBarUI;


    private void Awake()
    {
        enemyHealthBarUI = this;
    }
   
    public void SetMaxHealth(int health)
    {
        enemyHealthBar.maxValue = health;
        enemyHealthBar.value = health;
    }

    public void SetHealth(int health)
    {
        enemyHealthBar.value = health;
    }


}
