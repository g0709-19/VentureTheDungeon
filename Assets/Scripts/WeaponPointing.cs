using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPointing : MonoBehaviour
{

    public Transform centerTransform;
    public Transform sPointTransform;

    SpriteRenderer myRenderer;
    SpriteRenderer playerRenderer;

    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        playerRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    public float distanceFromPlayer = 1.0f;

    void FixedUpdate()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        float eulerAngle = centerTransform.eulerAngles.z;

        //Debug.Log(centerTransform.eulerAngles);

        if (90 <= eulerAngle && eulerAngle <= 270)
        {
            if (!myRenderer.flipY)
            {
                myRenderer.flipY = true;
                FlipSPoint();
                float x = transform.parent.position.x - distanceFromPlayer;
                float y = transform.parent.position.y;
                Vector2 leftPosition = new Vector2(x, y);
                transform.position = leftPosition;
            }
        }
        else
        {
            if (myRenderer.flipY)
            {
                float x = transform.parent.position.x + distanceFromPlayer;
                float y = transform.parent.position.y;
                Vector2 leftPosition = new Vector2(x, y);
                transform.position = leftPosition;
                myRenderer.flipY = false;
                FlipSPoint();
            }
        }
    }

    void FlipSPoint()
    {
        Vector3 temp = sPointTransform.localPosition;
        temp.y *= -1;
        sPointTransform.localPosition = temp;
    }
}
