using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFluid : MonoBehaviour
{
    [HideInInspector]
    public GameManager GameManager;

    private void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        GameManager.CreatRay(GetPos, NoPos);
    }

    public void GetPos(Vector3 pos)
    {
        //this.transform.GetChild(0).gameObject.SetActive(true);
        this.transform.GetChild(0).GetChild(0).position = pos + new Vector3(0, 0.1f, 0);
        //this.transform.position = pos + new Vector3(0, 0.1f, 0);
    }

    public void NoPos()
    {
        //this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(0).GetChild(0).position = GameManager.player.transform.position;
    }
}
