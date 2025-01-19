using UnityEngine;
using UnityEngine.UI;

public class MouseFollowRotation : MonoBehaviour
{
    [HideInInspector]
    public GameManager GameManager;

    public Transform target;
    public float xSpeed = 200, ySpeed = 200, mSpeed = 2;
    public float yMinLimit = 10, yMaxLimit = 80;
    public float xMinLimit = -360, xMaxLimit = 360;
    public float distance = 2, minDistance = 0.5f, maxDistance = 3;

    public bool canAxis = true;

    public bool needDamping = false;
    float damping = 5.0f;
    public float x = 0.0f;
    public float y = 0.0f;
    public Camera Cam;
    public float distanceY = 0.0f;

    private void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void SetTarget(GameObject go)
    {
        target = go.transform;
    }

    void LateUpdate()
    {
        if (target)
        {
            if (Input.GetMouseButton(1))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);
                x = ClampAngle(x, xMinLimit, xMaxLimit);
            }

            if (canAxis)
            {
                distance -= Input.GetAxis("Mouse ScrollWheel") * mSpeed;
            }

            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
            Vector3 disVector = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * disVector + target.position;

            if (needDamping)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
            }
            else
            {
                transform.rotation = rotation;
                transform.position = position;
            }
        }
        else
        {
            if (GameManager.player)
                target = GameManager.player.transform;
        }
    }
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}