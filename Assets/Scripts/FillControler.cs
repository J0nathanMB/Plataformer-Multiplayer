using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillControler : MonoBehaviour
{
    Image fill;

    private void Awake()
    {
        fill = GetComponent<Image>();
    }
  

    public void SetfillAmount(float value)
    {
        fill.fillAmount = value;
    }
     public void ChangeFillAmount(float change)
    {
        fill.fillAmount += change;
        if (fill.fillAmount < 0)
        {
            fill.fillAmount = 0;
        }
        else if (fill.fillAmount > 1)
        {
            fill.fillAmount = 1;
        }

    }
}
