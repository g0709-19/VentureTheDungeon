using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletController : MonoBehaviour
{
    public float speed;

    public Animator animator;
    public Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        // 총알 생성과 동시에 방향 고정
        heading = rigidBody.velocity;
    }

    Vector2 heading;

    // Update is called once per frame
    void Update()
    {
        // 시간프레임과 speed에 따라 움직인다 
        rigidBody.velocity = (heading * speed * Time.deltaTime);
    }

    BossController boss;

    public void SetBoss(BossController boss)
    {
        this.boss = boss;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 보스 몬스터의 공격은 보스 몬스터, 같은 총알끼리 충돌 무시
        if (collision.gameObject.tag == "Boss" || collision.gameObject.tag == "Particle")
        {
            Physics2D.IgnoreLayerCollision(10, 9);
            Physics2D.IgnoreLayerCollision(9, 9);
            return;
        }

        // 총알과 충돌 시 플레이어에게 데미지 부여
        if (!boss.isDead && collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<PlayerController>().DamagedByMonster(gameObject, 20f);

        // 충돌 후 총알 제거
        Destroy(gameObject);
    }
}
