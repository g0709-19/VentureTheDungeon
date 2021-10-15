using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletController : MonoBehaviour
{
    public float speed;
    public float lifeTime;

    public Animator animator;
    public Rigidbody2D rigidBody;

    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        // 총알을 시간에 맞게 지운다
        StartCoroutine(WaitForDestroy());

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

    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        animator.SetTrigger(DESTROY_TRIGGER);
        Destroy(gameObject, ANIMATION_PLAYTIME);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boss" || collision.gameObject.tag == "Particle")
        {
            Physics2D.IgnoreLayerCollision(10, 9);
            Physics2D.IgnoreLayerCollision(9, 9);
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<PlayerController>().DamagedByMonster(gameObject, 20f);

        Destroy(gameObject);
    }
}
