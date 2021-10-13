using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    static CameraController camera;

    public Transform targetTransform;
    public Transform crosshairTransform;
    public float moveSpeed = 0.125f;
    public float distance = 2.0f;
    public Vector3 offset;

    void Start()
    {
        if (camera == null) {
            camera = this;
            targetPosition = transform.position;
        }
        else
            Destroy(this);
    }

    Vector3 targetPosition;

    public void MoveTo(Vector3 position)
    {
        Debug.Log(position);
        targetPosition = position;
        targetPosition.z = transform.position.z;
    }

    private void FixedUpdate()
    {
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, moveSpeed);
        transform.position = smoothedPosition;
    }

    //void FixedUpdate()
    //{
    //    Vector3 desiredPosition = targetTransform.position + offset;
    //    Vector3 directionToCrosshairFromPlayer = Vector3.zero - GetMousePoint();
    //    directionToCrosshairFromPlayer = directionToCrosshairFromPlayer.normalized * -1 * distance;
    //    desiredPosition += directionToCrosshairFromPlayer;
    //    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, moveSpeed);
    //    transform.position = smoothedPosition;

    //}
    //Vector3 GetMousePoint()
    //{
    //    Vector3 mouse = Input.mousePosition;
    //    Ray castPoint = Camera.main.ScreenPointToRay(mouse);
    //    Vector3 point = castPoint.GetPoint(0.0f);
    //    return point;
    //}

    const float SHAKE_OFFSET = 1.0f;

    public static void Shake()
    {
        Vector3 desiredPosition = camera.transform.position;
        desiredPosition.y += SHAKE_OFFSET;
        float moveSpeed = 2.0f * Time.deltaTime;
        Vector3 shakedPosition = Vector3.Lerp(camera.transform.position, desiredPosition, moveSpeed);
        camera.transform.position = shakedPosition;
        camera.Invoke("CalmDown", 0.05f);
    }

    void CalmDown()
    {
        Vector3 desiredPosition = camera.transform.position;
        desiredPosition.y -= SHAKE_OFFSET;
        float moveSpeed = 2.0f * Time.deltaTime;
        Vector3 shakedPosition = Vector3.Lerp(camera.transform.position, desiredPosition, moveSpeed);
        camera.transform.position = shakedPosition;
    }
}
