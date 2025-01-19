using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

[RequireComponent(typeof(ObiSoftbody))]
public class TestConIce1 : MonoBehaviour
{
    [HideInInspector]
    public GameManager GameManager;
    public Transform cameraTra;
    public float acceleration = 80;
    public float kickAcceleration = 10.0f;
    bool isLock = false;
    bool isTongLock = false;

    ObiSoftbody softbody;

    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cameraTra = GameManager.Camera;
        GameManager.WaitTimeEvent(0.1f, this.gameObject, (obj) =>
        {
            softbody = GetComponent<ObiSoftbody>();
            softbody.enabled = true;
            GetComponent<ObiSoftbodySkinner>().enabled = true;
            GameManager.CreatChangeEffect("ChangeIce");
            softbody.solver.OnCollision += Solver_OnCollision;
        });

        closeEvent();
    }

    private void OnDestroy()
    {
        softbody.solver.OnCollision -= Solver_OnCollision;
    }

    void Update()
    {
        if (!softbody || isLock)
        {
            return;
        }

        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += cameraTra.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += -cameraTra.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += -cameraTra.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += cameraTra.right;
        }

        direction.y = 0;

        float effectiveAcceleration = acceleration;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            effectiveAcceleration = acceleration + kickAcceleration;
            softbody.AddForce(direction.normalized * effectiveAcceleration, ForceMode.VelocityChange);
        }
        else
        {
            softbody.AddForce(direction.normalized * effectiveAcceleration, ForceMode.Impulse);
        }
    }

    public void closeEvent()
    {
        isLock = true;

        GameManager.WaitTimeEvent(1.0f, this.gameObject, (obj) =>
        {
            isLock = false;
        });
    }

    public void Wait2()
    {
        isTongLock = false;
    }

    private void Solver_OnCollision(ObiSolver solver, ObiSolver.ObiCollisionEventArgs e)
    {
        var world = ObiColliderWorld.GetInstance();
        foreach (Oni.Contact contact in e.contacts)
        {
            if (contact.distance > 0.01)
            {
                var col = world.colliderHandles[contact.bodyB].owner;

                if (col.gameObject.name == "tishi1")
                {
                    CanvasLevel1Scr.Instance.show(0);
                }
                else if (col.gameObject.name == "tishi2")
                {
                    CanvasLevel1Scr.Instance.show(1);
                }
                else if (col.gameObject.name == "tishi3")
                {
                    CanvasLevel1Scr.Instance.show(2);
                }
                else if (col.gameObject.name == "tishi4")
                {
                    CanvasLevel1Scr.Instance.show(3);
                }
                else if (col.gameObject.name == "tishi5")
                {
                    CanvasLevel1Scr.Instance.show(4);
                }

                if (col.gameObject.tag == "Barrel" && !isTongLock)
                {
                    ManagerEventCon.BroadCast(ProEventType.SoundEffects, "×²»÷Ä¾Í°");
                    isTongLock = true;
                    Invoke("Wait2", 2.0f);
                }

                if (col.gameObject.layer == LayerMask.NameToLayer("Ice"))
                {
                    if (GameManager.curPlayerType == PlayerType.Ball)
                        ManagerEventCon.BroadCast(ProEventType.ChangeType, PlayerType.Ice);
                    return;
                }

                if (col.gameObject.layer == LayerMask.NameToLayer("Fire"))
                {
                    if (GameManager.curPlayerType == PlayerType.Ice)
                        ManagerEventCon.BroadCast(ProEventType.ChangeType, PlayerType.Ball);
                    return;
                }

                if (col.gameObject.tag == "Finish")
                {
                    ManagerEventCon.BroadCast(ProEventType.FinishEvent);
                    return;
                }

                if (col.gameObject.tag == "Death")
                {
                    ManagerEventCon.BroadCast(ProEventType.DeathEvent);
                    return;
                }
            }
        }
    }
}
