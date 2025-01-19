using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPanel : MonoBehaviour
{
    public Button Btn_1;
    public Button Btn_2;

    private void OnEnable()
    {
        ManagerEventCon.AddListener(ProEventType.Btn_Back, BackEvent);
    }

    private void OnDisable()
    {
        ManagerEventCon.RemoveListener(ProEventType.Btn_Back, BackEvent);
    }

    private void Start()
    {
        Btn_1.onClick.AddListener(() =>
        {
            if (ManagerScr.Instance.MusicBools[0])
            {
                ManagerScr.Instance.MusicBools[0] = false;
                Btn_1.transform.GetChild(0).GetComponent<Animator>().Play("BtnClose", 0);
                ManagerScr.Instance.GetComponent<AudioSource>().enabled = false;
            }
            else
            {
                ManagerScr.Instance.MusicBools[0] = true;
                Btn_1.transform.GetChild(0).GetComponent<Animator>().Play("BtnOpen", 0);
                ManagerScr.Instance.GetComponent<AudioSource>().enabled = true;
                ManagerScr.Instance.GetComponent<AudioSource>().Play();
            }
        });

        Btn_2.onClick.AddListener(() =>
        {
            if (ManagerScr.Instance.MusicBools[1])
            {
                ManagerScr.Instance.MusicBools[1] = false;
                Btn_2.transform.GetChild(0).GetComponent<Animator>().Play("BtnClose", 0);
                ManagerScr.Instance.ClearAllSounds();
            }
            else
            {
                ManagerScr.Instance.MusicBools[1] = true;
                Btn_2.transform.GetChild(0).GetComponent<Animator>().Play("BtnOpen", 0);
            }
        });
    }

    public void BackEvent()
    {
        ManagerScr.Instance.SetPanel.SetActive(false);
    }
}
