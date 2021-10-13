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
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Particle"))
            return;
        
        switch (collision.gameObject.tag)
        {
            case "Player":
                Debug.Log("Player에게 데미지!");
                collision.gameObject.GetComponent<PlayerController>().DamagedByMonster(gameObject, 20f);
                break;
            case "Particle":
                Debug.Log("Player에게 공격받음!");
                break;
        }
    }
}
