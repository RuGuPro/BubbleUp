using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerType
{
    None,
    Ball,
    Liquid,
    Ice
}

public class GameManager : MonoBehaviour
{
    public Transform Camera;
    [HideInInspector]
    public GameObject player;
    public GameObject curSolver;
    public Transform creatParent;
    public Transform startPos;
    public PlayerType curPlayerType = PlayerType.None;
    public Material TouMingMat;
    public Material BallMat;
    private bool isFinish = false;

    private bool isLock = false;
    public bool isFirst = true;

    private void Start()
    {
        ManagerEventCon.AddListener<PlayerType>(ProEventType.ChangeType, CreatPlayer);
        ManagerEventCon.AddListener(ProEventType.FinishEvent, FinishEvent);
        ManagerEventCon.AddListener(ProEventType.DeathEvent, DeathEvent);
        CreatPlayer(PlayerType.Ball);
    }

    private void OnDestroy()
    {
        ManagerEventCon.RemoveListener<PlayerType>(ProEventType.ChangeType, CreatPlayer);
        ManagerEventCon.RemoveListener(ProEventType.FinishEvent, FinishEvent);
        ManagerEventCon.RemoveListener(ProEventType.DeathEvent, DeathEvent);
        StopAllCoroutines();
    }

    public void FinishEvent()
    {
        if (!isFinish)
        {
            isFinish = true;
            Debug.Log("³É¹¦");
            ManagerScr.Instance.SPanel.SetActive(true);
            if (CanvasLevel1Scr.Instance)
                CanvasLevel1Scr.Instance.show(-1);
        }
    }

    public void DeathEvent()
    {
        if (!isFinish)
        {
            isFinish = true;
            Debug.Log("Ê§°Ü");
            ManagerScr.Instance.FPanel.SetActive(true);
            if (CanvasLevel1Scr.Instance)
                CanvasLevel1Scr.Instance.show(-1);
        }
    }

    public void CreatPlayer(PlayerType type)
    {
        if (isLock)
        {
            return;
        }

        StopAllCoroutines();

        switch (type)
        {
            case PlayerType.Ball:
                if (curPlayerType == PlayerType.Ball)
                    return;

                if (player != null)
                {
                    if (curPlayerType == PlayerType.Liquid)
                    {
                        if (curSolver != null)
                        {
                            curSolver.SetActive(false);
                        }

                        player.GetComponent<ObiSoftbody>().deformationResistance = 1.0f;
                        player.GetComponent<TestConBall1>().closeEvent();
                        player.GetComponent<TestConBall1>().ChangeForce(10.0f);
                        player.GetComponent<Renderer>().material = BallMat;
                    }
                    else if (curPlayerType == PlayerType.Ice)
                    {
                        StartCoroutine(LoadObj("Players/Ball", creatParent, (obj) =>
                        {
                            obj.transform.position = player.transform.position;
                            Destroy(player.gameObject);
                            player = obj;
                            player.GetComponent<Renderer>().material = BallMat;
                            StartCoroutine(LoadObj("Effects/FireEffect", creatParent, (effect) =>
                            {
                                CreatRay((pos) =>
                                {
                                    effect.transform.position = pos;
                                });
                            }));
                        }));
                    }
                }
                else
                {
                    StartCoroutine(LoadObj("Players/Ball", creatParent, (obj) =>
                    {
                        player = obj;
                        player.GetComponent<Renderer>().material = BallMat;
                    }));
                }
                curPlayerType = PlayerType.Ball;
                ManagerEventCon.BroadCast(ProEventType.SoundEffects, "×´Ì¬×ª»»");
                isLock = true;
                Invoke("LockOpen", 2.0f);
                break;
            case PlayerType.Liquid:
                if (curPlayerType == PlayerType.Liquid)
                    return;

                if (curPlayerType == PlayerType.Ball)
                {
                    player.GetComponent<ObiSoftbody>().deformationResistance = 0.05f;
                    player.GetComponent<TestConBall1>().closeEvent();
                    player.GetComponent<TestConBall1>().ChangeForce(12.0f);

                    curSolver.SetActive(true);
                    player.GetComponent<Renderer>().material = TouMingMat;
                    curPlayerType = PlayerType.Liquid;
                    ManagerEventCon.BroadCast(ProEventType.SoundEffects, "×´Ì¬×ª»»");
                    isLock = true;
                    Invoke("LockOpen", 2.0f);
                }
                break;
            case PlayerType.Ice:
                if (curPlayerType == PlayerType.Ice)
                    return;

                if (curPlayerType == PlayerType.Ball)
                {
                    StartCoroutine(LoadObj("Players/Ice", creatParent, (obj) =>
                    {
                        obj.transform.position = player.transform.position;
                        Destroy(player.gameObject);
                        player = obj;

                        StartCoroutine(LoadObj("Effects/IceEffect", creatParent, (effect) =>
                        {
                            CreatRay((pos) =>
                            {
                                effect.transform.position = pos;
                            });
                        }));
                    }));
                    curPlayerType = PlayerType.Ice;
                    ManagerEventCon.BroadCast(ProEventType.SoundEffects, "×´Ì¬×ª»»");
                    isLock = true;
                    Invoke("LockOpen", 2.0f);
                }
                break;
            default:
                break;
        }
    }

    public void OtherLoadObj(string path, Transform parent, Action<GameObject> e)
    {
        StartCoroutine(LoadObj(path, parent, e));
    }

    IEnumerator LoadObj(string path, Transform parent, Action<GameObject> e)
    {
        ResourceRequest rq = Resources.LoadAsync<GameObject>(path);
        yield return rq;

        GameObject obj = ResourceObj(rq.asset as GameObject, parent);
        e?.Invoke(obj);
    }

    IEnumerator WaitTime(float time, GameObject obj, Action<GameObject> e)
    {
        yield return new WaitForSeconds(time);
        e?.Invoke(obj);
    }

    public GameObject ResourceObj(GameObject perfab, Transform parent)
    {
        GameObject obj = Instantiate(perfab, parent);
        return obj;
    }

    public void CreatRay(Action<Vector3> e)
    {
        if (player)
        {
            Ray ray = new Ray(player.transform.position + new Vector3(0, 1, 0), Vector3.down);
            RaycastHit[] hitInfos;
            hitInfos = Physics.RaycastAll(ray, 4);
            for (int i = 0; i < hitInfos.Length; i++)
            {
                if (hitInfos[i].collider.gameObject.tag == "Ground")
                {
                    e?.Invoke(hitInfos[i].point);
                    break;
                }
            }
        }
    }

    public void CreatRay(Action<Vector3> e, Action eNull)
    {
        if (player)
        {
            Ray ray = new Ray(player.transform.position + new Vector3(0, 2, 0), Vector3.down);
            RaycastHit[] hitInfos;
            hitInfos = Physics.RaycastAll(ray, 10);
            for (int i = 0; i < hitInfos.Length; i++)
            {
                if (hitInfos[i].collider.gameObject.tag == "Ground")
                {
                    e?.Invoke(hitInfos[i].point);
                    return;
                }
            }

            eNull?.Invoke();
        }
    }

    public void LockOpen()
    {
        isLock = false;
    }

    private void Update()
    {
        if (player)
        {
            if (isFinish)
            {
                Camera.GetComponent<MouseFollowRotation>().enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.P) && curPlayerType == PlayerType.Ball)
        {
            ManagerEventCon.BroadCast(ProEventType.ChangeType, PlayerType.Liquid);
        }
        if (Input.GetKeyUp(KeyCode.P) && curPlayerType == PlayerType.Liquid)
        {
            ManagerEventCon.BroadCast(ProEventType.ChangeType, PlayerType.Ball);
        }
    }

    public void CreatChangeEffect(string name)
    {
        StartCoroutine(LoadObj("Effects/" + name, creatParent, (effect) =>
         {
             effect.transform.position = player.transform.position;
         }));
    }

    public void WaitTimeEvent(float time, GameObject obj, Action<GameObject> e)
    {
        StartCoroutine(WaitTime(time, obj, e));
    }

    public void ReTry()
    {
        //ManagerEventCon.BroadCast(ProEventType.Init);
        //isFirst = true;
        //isFinish = false;
        //isLock = false;
        //Destroy(player.gameObject);
        //Camera.GetComponent<MouseFollowRotation>().enabled = true;
        //Invoke("Wait", 0.5f);

        LoadSceneManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Wait()
    {
        curPlayerType = PlayerType.None;
        CreatPlayer(PlayerType.Ball);
    }
}
