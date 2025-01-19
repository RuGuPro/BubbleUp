using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLevel1Scr : MonoSingleton<CanvasLevel1Scr>
{
    public void show(int num)
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == num)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
