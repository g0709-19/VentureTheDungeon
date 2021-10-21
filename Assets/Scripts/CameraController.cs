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

    // 방 이동 시 사용되는 카메라 이동
    public void MoveTo(Vector3 position)
    {
        targetPosition = position;
        targetPosition.z = transform.position.z;
    }

    // 정해진 좌표로 천천히 카메라 이동
    private void FixedUpdate()
    {
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, moveSpeed);
        transform.position = smoothedPosition;
    }

    const float SHAKE_OFFSET = 1.0f;

    // 화면 흔들림
    public static void Shake()
    {
        Vector3 desiredPosition = camera.transform.position;
        desiredPosition.y += SHAKE_OFFSET;
        float moveSpeed = 2.0f * Time.deltaTime;
        Vector3 shakedPosition = Vector3.Lerp(camera.transform.position, desiredPosition, moveSpeed);
        camera.transform.position = shakedPosition;
        camera.Invoke("CalmDown", 0.05f);
    }

    // 위로 간 화면을 다시 제자리로 돌림
    void CalmDown()
    {
        Vector3 desiredPosition = camera.transform.position;
        desiredPosition.y -= SHAKE_OFFSET;
        float moveSpeed = 2.0f * Time.deltaTime;
        Vector3 shakedPosition = Vector3.Lerp(camera.transform.position, desiredPosition, moveSpeed);
        camera.transform.position = shakedPosition;
    }
}
