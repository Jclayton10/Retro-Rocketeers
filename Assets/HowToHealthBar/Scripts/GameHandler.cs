/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour {

	private void Start () {
        HealthBar healthBar = HealthBar.Create(new Vector3(0, 0), new Vector3(40, 5), Color.red, Color.grey);
        AnimateBar(healthBar, Color.red, .05f);

        healthBar = HealthBar.Create(new Vector3(0, 10), new Vector3(40, 5), Color.yellow, Color.white, new HealthBar.Border { thickness = 1f, color = Color.black });
        AnimateBar(healthBar, Color.yellow, .04f);

        healthBar = HealthBar.Create(new Vector3(0, 20), new Vector3(40, 5), Color.red, Color.grey, new HealthBar.Border { thickness = 1f, color = Color.white });
        AnimateBar(healthBar, Color.red, .06f);

	}

    private void AnimateBar(HealthBar healthBar, Color normalColor, float periodicTimer) {
        float health = 1f;
        FunctionPeriodic.Create(() => {
            if (health > .01f) {
                health -= .01f;
                healthBar.SetSize(health);

                if (health < .3f) {
                    // Under 30% health
                    if ((int)(health * 100f) % 3 == 0) {
                        healthBar.SetColor(Color.white);
                    } else {
                        healthBar.SetColor(normalColor);
                    }
                }
            } else {
                health = 1f;
                healthBar.SetColor(normalColor);
            }
        }, periodicTimer);
    }
}
