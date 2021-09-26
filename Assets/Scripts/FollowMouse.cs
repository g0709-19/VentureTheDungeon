using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
