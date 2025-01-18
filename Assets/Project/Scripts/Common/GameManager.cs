using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
    }

    public void DeathEvent()
    {
        if (!isFinish)
        {
            isFinish = true;
            Debug.Log("Ê§°Ü");
        }
    }

    public void CreatPlayer(PlayerType type)
    {
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
                        player.GetComponent<TestConBall1>().ChangeForce(1.5f);
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
                break;
            case PlayerType.Liquid:
                if (curPlayerType == PlayerType.Liquid)
                    return;

                if (curPlayerType == PlayerType.Ball)
                {
                    player.GetComponent<ObiSoftbody>().deformationResistance = 0.05f;
                    player.GetComponent<TestConBall1>().closeEvent();
                    player.GetComponent<TestConBall1>().ChangeForce(4.0f);

                    curSolver.SetActive(true);
                    player.GetComponent<Renderer>().material = TouMingMat;
                    curPlayerType = PlayerType.Liquid;
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
                }
                break;
            default:
                break;
        }
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
            Ray ray = new Ray(player.transform.position + new Vector3(0, 1, 0), Vector3.down);
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

    private void Update()
    {
        if (player)
        {
            if (!isFinish)
            {
                Camera.position = Vector3.Lerp(Camera.position, (player.transform.position + new Vector3(5, 5, -5)), 2.0f * Time.deltaTime);
            }
        }
        else
        {
            Camera.transform.position = startPos.transform.position + new Vector3(5, 5, -5);
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

    public void CreatChangeEffect()
    {
        StartCoroutine(LoadObj("Effects/ChangeEffect", creatParent, (effect) =>
        {
            effect.transform.position = player.transform.position;
        }));
    }

    public void WaitTimeEvent(float time, GameObject obj, Action<GameObject> e)
    {
        StartCoroutine(WaitTime(time, obj, e));
    }
}
