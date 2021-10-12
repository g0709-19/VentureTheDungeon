using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            Destroy(collision.transform.parent.parent.gameObject);
            GameObject.FindGameObjectWithTag("Player").transform.position = Vector2.zero;
        }
    }
}
