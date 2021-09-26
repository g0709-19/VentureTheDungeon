﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPointing : MonoBehaviour
{

    public Transform centerTransform;

    SpriteRenderer myRenderer;
    SpriteRenderer playerRenderer;

    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        playerRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        float eulerAngle = centerTransform.eulerAngles.z;

        Debug.Log(centerTransform.eulerAngles);

        if (90 <= eulerAngle && eulerAngle <= 270)
        {
            myRenderer.flipY = true;
            float x = transform.parent.position.x - 0.65f;
            float y = transform.parent.position.y;
            Vector2 leftPosition = new Vector2(x, y);
            transform.position = leftPosition;
        }
        else
        {
            float x = transform.parent.position.x + 0.65f;
            float y = transform.parent.position.y;
            Vector2 leftPosition = new Vector2(x, y);
            transform.position = leftPosition;
            myRenderer.flipY = false;
        }
    }
}