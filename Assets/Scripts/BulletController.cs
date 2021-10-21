using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
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
        mousePosition = GetPositionToMouse().normalized * 5.0f;

        startPosition = transform.position;
    }

    Vector2 mousePosition;

    // Update is called once per frame
    void Update()
    {
        // 시간프레임과 speed에 따라 움직인다 
        rigidBody.velocity = (mousePosition * speed * Time.deltaTime);
    }
    Vector2 GetPositionToMouse()
    {
        //카메라 스크린의 마우스 거리와 총과의 방향 
        return Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    const string DESTROY_TRIGGER = "Disappear";
    const float ANIMATION_PLAYTIME = 0.4f;

    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        animator.SetTrigger(DESTROY_TRIGGER);
        Destroy(gameObject, ANIMATION_PLAYTIME);
    }

    int bounce = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 총알이 플레이어나 같은 총알과 부딪히면 충돌 무시
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Particle")
        {
            Physics2D.IgnoreLayerCollision(8, 9);
            Physics2D.IgnoreLayerCollision(9, 9);
            return;
        }

        // 충돌 지점
        Vector3 hitPosition = collision.contacts[0].point;

        // 벽에 충돌시 튕기도록 입사각, 반사각 구함
        Vector3 incomingVec = hitPosition - startPosition;
        Vector3 reflectVec = Vector3.Reflect(incomingVec, collision.contacts[0].normal);
        
        startPosition = transform.position;

        mousePosition = reflectVec.normalized * 2.0f;
        speed *= 1.5f;

        ++bounce;

        if (collision.gameObject.CompareTag("Monster") || collision.gameObject.CompareTag("Boss"))
            Destroy(gameObject);
    }

    public int GetBounce()
    {
        return bounce;
    }

    public void BuffSpeed(float percent)
    {
        this.speed *= percent;
    }
}
