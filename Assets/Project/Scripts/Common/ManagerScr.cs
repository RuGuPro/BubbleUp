using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScr : MonoSingleton<ManagerScr>
{
    public GameObject StartPanel;
    public GameObject SetPanel;
    public GameObject LevelPanel;
    public GameObject SPanel;
    public GameObject FPanel;
    public List<bool> LevelStep;
    public List<bool> MusicBools;
    public int CurSceneNum = 0;
    public List<GameObject> SoundEffectsList;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        SceneManager.LoadSceneAsync("FirstScene");
        ManagerEventCon.AddListener<string>(ProEventType.SoundEffects, CreatBtn_SoundEffects);
    }

    private void OnDestroy()
    {
        ManagerEventCon.RemoveListener<string>(ProEventType.SoundEffects, CreatBtn_SoundEffects);
    }

    public void Btn_BackEvent()
    {
        ManagerEventCon.BroadCast(ProEventType.Btn_Back);
    }

    public void ChangeCurSceneNum(int num)
    {
        CurSceneNum = num;
    }

    public void CreatBtn_SoundEffects(string path)
    {
        if (!MusicBools[1])
        {
            return;
        }

        StartCoroutine(LoadMusic(path));
    }

    IEnumerator LoadMusic(string path)
    {
        ResourceRequest rq1 = Resources.LoadAsync<AudioClip>("Musics/"+ path);
        yield return rq1;

        ResourceRequest rq = Resources.LoadAsync<GameObject>("SoundEffect");
        yield return rq;

        GameObject obj = ResourceObj(rq.asset as GameObject, this.transform);
        obj.GetComponent<AudioSource>().clip = rq1.asset as AudioClip;
        SoundEffectsList.Add(obj);

        obj.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(obj.GetComponent<AudioSource>().clip.length);

        if (obj)
        {
            SoundEffectsList.Remove(obj);
            Destroy(obj);
        }
    }

    public GameObject ResourceObj(GameObject perfab, Transform parent)
    {
        GameObject obj = Instantiate(perfab, parent);
        return obj;
    }

    public void ClearAllSounds()
    {
        foreach (GameObject item in SoundEffectsList)
        {
            Destroy(item);
        }
        SoundEffectsList = new List<GameObject>();
    }
}
