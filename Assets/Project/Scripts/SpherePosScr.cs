using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePosScr : MonoBehaviour
{
    [HideInInspector]
    public GameManager GameManager;
    private float curTime = 0.00f;
    private float targetTime = 0.00f;
    private bool isStartTime = false;

    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Invoke("CreatZSphere", 1.0f);
    }

    void Update()
    {
        if (isStartTime)
        {
            curTime += Time.deltaTime;
            if (curTime >= targetTime)
            {
                isStartTime = false;
                CreatZSphere();
            }
        }
    }

    public void CreatZSphere()
    {
        GameManager.OtherLoadObj("ZSphere", this.transform, (obj) =>
        {
            obj.transform.position = this.transform.position;
            targetTime = Random.Range(5f, 10f);
            curTime = 0.00f;
            isStartTime = true;
        });
    }
}
