using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

[RequireComponent(typeof(ObiSoftbody))]
public class TestConIce1 : MonoBehaviour
{
    [HideInInspector]
    public GameManager GameManager;
    public float acceleration = 80;
    public float kickAcceleration = 10.0f;
    bool isLock = false;

    ObiSoftbody softbody;

    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        GameManager.WaitTimeEvent(0.1f, this.gameObject, (obj) =>
        {
            softbody = GetComponent<ObiSoftbody>();
            softbody.enabled = true;
            GetComponent<ObiSoftbodySkinner>().enabled = true;
            GameManager.CreatChangeEffect();
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
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += -Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += -Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
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

    private void Solver_OnCollision(ObiSolver solver, ObiSolver.ObiCollisionEventArgs e)
    {
        var world = ObiColliderWorld.GetInstance();
        foreach (Oni.Contact contact in e.contacts)
        {
            if (contact.distance > 0.01)
            {
                var col = world.colliderHandles[contact.bodyB].owner;

                if (col.gameObject.tag == "Ice")
                {
                    if (GameManager.curPlayerType == PlayerType.Ball)
                        ManagerEventCon.BroadCast(ProEventType.ChangeType, PlayerType.Ice);
                    return;
                }

                if (col.gameObject.tag == "Fire")
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
