using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLifeScr : MonoBehaviour
{
    public bool isLock = false;
    public GameObject obj0;
    public GameObject obj1;
    public List<GameObject> TaiJies;

    public void showEvent()
    { 
        if(!isLock)
        {
            ManagerEventCon.BroadCast(ProEventType.SoundEffects, "Æô¶¯»ú¹Ø");
            isLock = true;
            obj0.SetActive(false);
            obj1.SetActive(true);
            TaiJies[0].SetActive(true);
            TaiJies[0].transform.localPosition = new Vector3(-2.4f, -1.7f, -0.2900009f);
            TaiJies[0].transform.DOLocalMove(new Vector3(-2.4f, -0.3769999f, -0.2900009f), 1.0f).SetEase(Ease.Linear).OnComplete(() =>
            {
                TaiJies[1].SetActive(true);
                TaiJies[1].transform.localPosition = new Vector3(-3.9f, -1.65f, -0.2900009f);
                TaiJies[1].transform.DOLocalMove(new Vector3(-3.9f, 0.04f, -0.2900009f), 1.0f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    TaiJies[2].SetActive(true);
                    TaiJies[2].transform.localPosition = new Vector3(-5.4f, -1.77f, -0.2900009f);
                    TaiJies[2].transform.DOLocalMove(new Vector3(-5.4f, 0.62f, -0.2900009f), 1.0f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        TaiJies[3].SetActive(true);
                        TaiJies[3].transform.localPosition = new Vector3(-6.9f, -1.69f, -0.2900009f);
                        TaiJies[3].transform.DOLocalMove(new Vector3(-6.9f, 1.084f, -0.2900009f), 1.0f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            TaiJies[4].SetActive(true);
                            TaiJies[4].transform.localPosition = new Vector3(-8.4f, -1.07f, -0.2900009f);
                            TaiJies[4].transform.DOLocalMove(new Vector3(-8.4f, 1.65f, -0.2900009f), 1.0f).SetEase(Ease.Linear);
                        });
                    });
                });
            });
        }
    }

    public void closeEvent()
    {
        obj0.SetActive(true);
        obj1.SetActive(false);
        foreach (GameObject item in TaiJies)
        {
            item.SetActive(false);
        }
    }
}
