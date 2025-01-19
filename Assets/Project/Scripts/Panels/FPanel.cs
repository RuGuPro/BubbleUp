using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPanel : MonoBehaviour
{
    public Button Btn_TryAgain;
    public Button Btn_Home;

    private void Start()
    {
        Btn_TryAgain.onClick.AddListener(() =>
        {
            ManagerScr.Instance.FPanel.SetActive(false);
            GameManager GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            GameManager.ReTry();
        });

        Btn_Home.onClick.AddListener(() =>
        {
            ManagerScr.Instance.FPanel.SetActive(false);
            LoadSceneManager.Instance.LoadScene("FirstScene", () =>
            {
                ManagerScr.Instance.StartPanel.SetActive(true);
            });
        });
    }
}
