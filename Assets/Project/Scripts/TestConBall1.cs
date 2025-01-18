using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using System.Linq;

[RequireComponent(typeof(ObiSoftbody))]
public class TestConBall1 : MonoBehaviour
{
    [HideInInspector]
    public GameManager GameManager;
    public float acceleration = 80;
    public float jumpPower = 1;

    [Range(0, 1)]
    public float airControl = 0.3f;

    ObiSoftbody softbody;
    bool onGround = false;
    bool isLock = false;

    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (GameManager.startPos)
        {
            this.gameObject.transform.position = GameManager.startPos.position;
            GameManager.startPos = null;
        }

        GameManager.WaitTimeEvent(0.1f, this.gameObject, (obj) =>
        {
            softbody = GetComponent<ObiSoftbody>();
            softbody.enabled = true;
            GetComponent<ObiSoftbodySkinner>().enabled = true;
            softbody.solver.OnCollision += Solver_OnCollision;

            GameManager.CreatChangeEffect();
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

        // Determine movement direction:
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward * acceleration;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += -Vector3.right * acceleration;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += -Vector3.forward * acceleration;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right * acceleration;
        }

        // flatten out the direction so that it's parallel to the ground:
        direction.y = 0;

        // apply ground/air movement:
        float effectiveAcceleration = acceleration;

        if (!onGround)
            effectiveAcceleration *= airControl;

        softbody.AddForce(direction.normalized * effectiveAcceleration, ForceMode.Acceleration);

        // jump:
        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            onGround = false;
            softbody.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
        }
    }

    private void Solver_OnCollision(ObiSolver solver, ObiSolver.ObiCollisionEventArgs e)
    {
        onGround = false;

        var world = ObiColliderWorld.GetInstance();
        foreach (Oni.Contact contact in e.contacts)
        {
            if (contact.distance > 0.01)
            {
                var col = world.colliderHandles[contact.bodyB].owner;
                if (col.gameObject.tag == "Ground")
                {
                    onGround = true;
                    return;
                }

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

    public void closeEvent()
    {
        isLock = true;

        GameManager.WaitTimeEvent(1.0f, this.gameObject, (obj) =>
        {
            isLock = false;
        });
    }

    public void ChangeForce(float force)
    {
        acceleration = 0;
        GameManager.WaitTimeEvent(1.0f, this.gameObject,(obj) =>
        {
            acceleration = force;
        });
    }
}
