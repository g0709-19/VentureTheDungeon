using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{

    public Transform playerTransform;
    public float moveSpeed = 2.0f;

    Collider2D collider;

    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3 monsterPosition = transform.position;
        Vector3 monsterToPlayerVector = playerPosition - monsterPosition;
        monsterToPlayerVector = monsterToPlayerVector.normalized * moveSpeed * Time.deltaTime;
        transform.Translate(monsterToPlayerVector);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.collider, collider);
        }
    }
}
