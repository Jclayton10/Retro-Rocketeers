using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxeygenUI : MonoBehaviour
{
    public Slider oxeygenSlider;

    public void SetMaxOxeygen(int  oxeygen)
    {
        oxeygenSlider.maxValue = oxeygen;
        oxeygenSlider.value = oxeygen;
    }
    public void SetHealth(int oxeygen)
    {
        oxeygenSlider.value = oxeygen;
    }

}
