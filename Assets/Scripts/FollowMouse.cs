using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        Vector3 point = castPoint.GetPoint(0.0f);
        float x = point.x;
        float y = point.y;
        float z = transform.position.z;
        transform.position = new Vector3(x, y, z);
    }
}
