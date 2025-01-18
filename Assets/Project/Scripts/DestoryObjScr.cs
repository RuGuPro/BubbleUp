using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryObjScr : MonoBehaviour
{
    public float Time;

    public void Start()
    {
        Invoke("Wait", Time);
    }

    public void Wait()
    {
        Destroy(this.gameObject);
    }
}
