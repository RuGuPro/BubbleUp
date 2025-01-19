using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour
{
    public Button Btn_Level1;
    public Button Btn_Level2;

    private void OnEnable()
    {
        if (ManagerScr.Instance.LevelStep[0])
        {
            Btn_Level2.interactable = true;
        }
        ManagerEventCon.AddListener(ProEventType.Btn_Back, BackEvent);
    }

    private void OnDisable()
    {
        ManagerEventCon.RemoveListener(ProEventType.Btn_Back, BackEvent);
    }

    public void BackEvent()
    {
        ManagerScr.Instance.StartPanel.SetActive(true);
        ManagerScr.Instance.LevelPanel.SetActive(false);
    }

    public void LoadIng(string sceneName)
    {
        ManagerEventCon.BroadCast(ProEventType.SoundEffects, "¹Ø¿¨Ñ¡Ôñ°´¼üÒô");
        ManagerScr.Instance.LevelPanel.SetActive(false);
        LoadSceneManager.Instance.LoadScene(sceneName);
    }
}
