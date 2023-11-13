using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxeygenUI : MonoBehaviour
{
    public Slider oxeygenSlider;

    public void SetMaxOxyegen(float oxeygen)
    {
        oxeygenSlider.maxValue = oxeygen;
        oxeygenSlider.value = oxeygen;
    }
    public void SetOxyegen(float oxeygen)
    {
        oxeygenSlider.value = oxeygen;
    }

}
