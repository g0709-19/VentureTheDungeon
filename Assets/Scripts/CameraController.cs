using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform targetTransform;
    public Transform crosshairTransform;
    public float moveSpeed = 0.125f;
    public float distance = 2.0f;
    public Vector3 offset;

    void FixedUpdate()
    {
        Vector3 desiredPosition = targetTransform.position + offset;
        Vector3 directionToCrosshairFromPlayer = Vector3.zero - GetMousePoint();
        directionToCrosshairFromPlayer = directionToCrosshairFromPlayer.normalized * -1 * distance;
        desiredPosition += directionToCrosshairFromPlayer;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, moveSpeed);
        transform.position = smoothedPosition;

    }
    Vector3 GetMousePoint()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        Vector3 point = castPoint.GetPoint(0.0f);
        return point;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
