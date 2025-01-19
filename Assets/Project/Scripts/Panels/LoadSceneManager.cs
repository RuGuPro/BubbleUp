using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoSingleton<LoadSceneManager>
{
    public GameObject LoadingUI;
    public Slider loadingSlider;
    public Text loadingText;
    private float loadingSpeed = 1f;
    private bool startLoading = false;
    private float targetValue;
    private AsyncOperation operation;

    private void Start()
    {
        if (loadingSlider != null)
            loadingSlider.value = 0.0f;
    }
    public void LoadScene(string name, Action onFinish = null)
    {
        if (LoadingUI == null)
        {
            Debug.LogError("请在LoadSceneManager脚本上配置加载界面UI");
            return;
        }
        LoadingUI.SetActive(true);
        startLoading = true;
        operation = SceneManager.LoadSceneAsync(name);
        StartCoroutine(LoadIE(onFinish));
        StartCoroutine(AsyncLoading(name));
    }

    IEnumerator AsyncLoading(string name)
    {
        operation.allowSceneActivation = false;
        yield return operation;
    }
    IEnumerator LoadIE(Action onFinsh = null)
    {
        while (startLoading)
        {
            targetValue = operation.progress;
            if (operation.progress >= 0.9f)
            {
                targetValue = 1.0f;
            }
            if (targetValue != loadingSlider.value)
            {
                loadingSlider.value = Mathf.Lerp(loadingSlider.value, targetValue, Time.deltaTime * loadingSpeed);
                if (Mathf.Abs(loadingSlider.value - targetValue) < 0.01f)
                {
                    loadingSlider.value = targetValue;
                }
            }
            loadingText.text = ((int)(loadingSlider.value * 100)).ToString() + "%";
            //loadingText.text = sliderTip;
            if ((int)(loadingSlider.value * 100) == 100)
            {
                operation.allowSceneActivation = true;
                while (!operation.isDone)
                {
                    yield return null;
                }
                startLoading = false;
                loadingSlider.value = 0.0f;
                LoadingUI.SetActive(false);
                if (onFinsh != null)
                {
                    onFinsh.Invoke();
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
