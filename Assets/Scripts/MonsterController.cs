using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{

    public Transform playerTransform;
    public Animator animator;
    public float moveSpeed = 2.0f;
    public int bounceForDead;

    Collider2D collider;
    bool isDead = false;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (isDead) return;
        Vector3 playerPosition = playerTransform.position;
        Vector3 monsterPosition = transform.position;
        Vector3 monsterToPlayerVector = playerPosition - monsterPosition;
        monsterToPlayerVector = monsterToPlayerVector.normalized * moveSpeed * Time.deltaTime;
        transform.Translate(monsterToPlayerVector);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Particle"))
            return;
        
        switch (collision.gameObject.tag)
        {
            case "Player":
                collision.gameObject.GetComponent<PlayerController>().DamagedByMonster(gameObject, 20f);
                break;
            case "Particle":
                int bounce = collision.gameObject.GetComponent<BulletController>().GetBounce() - 1;
                Damaged(bounce);
                Debug.Log(bounce);
                break;
        }
    }

    void Damaged(int damage)
    {
        bounceForDead -= damage;
        if (bounceForDead <= 0)
        {
            bounceForDead = 0;
            isDead = true;
            animator.SetTrigger("Die");
            Invoke("Die", 1f);
        }
        // UI 표시: 못할듯?
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
