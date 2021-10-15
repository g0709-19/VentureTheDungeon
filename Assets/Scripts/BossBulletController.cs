using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletController : MonoBehaviour
{
    public float speed;

    public Animator animator;
    public Rigidbody2D rigidBody;

    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        // 총알 생성과 동시에 방향 고정
        heading = rigidBody.velocity;

        startPosition = transform.position;
    }

    Vector2 heading;

    // Update is called once per frame
    void Update()
    {
        // 시간프레임과 speed에 따라 움직인다 
        rigidBody.velocity = (heading * speed * Time.deltaTime);
    }

    const string DESTROY_TRIGGER = "Disappear";
    const float ANIMATION_PLAYTIME = 0.4f;

    BossController boss;

    public void SetBoss(BossController boss)
    {
        this.boss = boss;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boss" || collision.gameObject.tag == "Particle")
        {
            Physics2D.IgnoreLayerCollision(10, 9);
            Physics2D.IgnoreLayerCollision(9, 9);
            return;
        }

        if (!boss.isDead && collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<PlayerController>().DamagedByMonster(gameObject, 20f);

        Destroy(gameObject);
    }
}
