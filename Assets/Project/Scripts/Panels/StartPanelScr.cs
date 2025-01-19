using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanelScr : MonoBehaviour
{
    Material material;
    public Image Title;
    public Button BtnStart;
    public Button BtnSet;

    private void Awake()
    {
        material = Title.material;
    }

    private void OnEnable()
    {
        BtnStart.interactable = false;
        BtnSet.interactable = false;
        EnterEvent(() =>
        {
            BtnStart.interactable = true;
            BtnSet.interactable = true;
        });
    }

    void Start()
    {
        BtnStart.onClick.AddListener(() =>
        {
            BtnStart.interactable = false;
            BtnSet.interactable = false;
            OutEvent(() =>
            {
                ManagerScr.Instance.StartPanel.SetActive(false);
                ManagerScr.Instance.LevelPanel.SetActive(true);
            });
        });
        BtnSet.onClick.AddListener(() =>
        {
            ManagerScr.Instance.SetPanel.SetActive(true);
        });
    }

    public void EnterEvent(Action startEvent = null)
    {
        float value = 0.65f;
        DOTween.To(() => value, (len) => value = len, 2, 4).OnUpdate(() =>
        {
            material.SetFloat("_Animation_Factor", value);
        }).SetEase(Ease.Linear).OnComplete(() => startEvent?.Invoke()); ;
        BtnStart.GetComponent<Animator>().Play("Enter", 0);
        BtnSet.GetComponent<Animator>().Play("Enter", 0);
    }

    public void OutEvent(Action endEvent = null)
    {
        float value = 0.65f;
        DOTween.To(() => value, (len) => value = len, 2, 4).OnUpdate(() =>
        {
            material.SetFloat("_Animation_Factor", 2.65f - value);
        }).SetEase(Ease.Linear).OnComplete(() => endEvent?.Invoke());
        BtnStart.GetComponent<Animator>().Play("Out", 0);
        BtnSet.GetComponent<Animator>().Play("Out", 0);
    }
}
