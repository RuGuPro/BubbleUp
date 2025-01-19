using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SPanel : MonoBehaviour
{
    public Button Btn_Nextlevel;
    public Button Btn_TryAgain;
    public Button Btn_Home;

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "Level1Scene")
        {
            ManagerScr.Instance.LevelStep[0] = true;
        }

        if (SceneManager.GetActiveScene().name == "Leve21Scene")
        {
            ManagerScr.Instance.LevelStep[1] = true;
        }

        if (ManagerScr.Instance.CurSceneNum == 0)
        {
            Btn_Nextlevel.interactable = true;
        }
        else
        {
            Btn_Nextlevel.interactable = false;
        }

        ManagerEventCon.BroadCast(ProEventType.SoundEffects, "ЗаЬк");
    }

    private void Start()
    {
        Btn_Nextlevel.onClick.AddListener(() =>
        {
            ManagerScr.Instance.SPanel.SetActive(false);
            LoadSceneManager.Instance.LoadScene("Level2Scene");

            ManagerScr.Instance.ClearAllSounds();

            if (CanvasLevel1Scr.Instance)
                CanvasLevel1Scr.Instance.show(-1);
        });

        Btn_TryAgain.onClick.AddListener(() =>
        {
            ManagerScr.Instance.SPanel.SetActive(false);
            GameManager GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            GameManager.ReTry();

            ManagerScr.Instance.ClearAllSounds();
        });

        Btn_Home.onClick.AddListener(() =>
        {
            ManagerScr.Instance.SPanel.SetActive(false);

            ManagerScr.Instance.ClearAllSounds();
            LoadSceneManager.Instance.LoadScene("FirstScene", () =>
            {
                ManagerScr.Instance.StartPanel.SetActive(true);
            });
        });
    }
}
