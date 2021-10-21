using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{

    public Transform playerTransform;
    public Animator animator;
    public SpriteRenderer renderer;
    public Transform shadow;
    public float moveSpeed = 2.0f;
    public int bounceForDead = 10;          // 몬스터의 체력

    public AudioSource audioSource;
    public AudioClip[] shootSounds;

    public GameObject bullet;   // 보스가 발사하는 총알 Prefab
    public Canvas clearUI;      // 보스 클리어 시 화면에 표시할 UI

    public clockScript hpBar;

    Collider2D collider;
    public bool isDead = false;

    public static bool cleared = false;

    bool isSceneEnded = false;  // 보스 등장 장면 끝난 여부

    void Start()
    {
        collider = GetComponent<Collider2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        RandomlySelectMode();
        InitBulletPath();
        StartCoroutine(SceneEnd());
        hpBar.maxHP = bounceForDead;
        hpBar.remainHP = bounceForDead;
    }

    // 보스 등장 장면 실행 후 움직이도록 설정
    IEnumerator SceneEnd()
    {
        yield return new WaitForSeconds(1.5f);
        isSceneEnded = true;
        hpBar.gameObject.SetActive(true);
    }

    List<Vector2> vertices;
    float radius = 2f;

    // 보스 주변으로 원을 그리는 형태로 총알 생성하는데,
    // 그 좌표를 보스 생성 시 미리 구해놓음
    void InitBulletPath()
    {
        vertices = new List<Vector2>();
        float heading;
        for (int theta = 0; theta < 360; theta += 360 / 30)
        {
            heading = theta * Mathf.Deg2Rad;
            vertices.Add(new Vector2(Mathf.Cos(heading) * radius, Mathf.Sin(heading) * radius));
        }
    }

    void FixedUpdate()
    {
        if (!isSceneEnded) return;
        if (isDead) return;
        switch (mode)   // 패턴에 따른 행동
        {
            case FOLLOW_MODE: Follow(); break;
            case ATTACK_MODE: Attack(); break;
            case STOP_MODE: Stop(); break;
        }
    }

    // 패턴1: 플레이어를 따라다님
    void Follow()
    {
        StartRunning(); // 달리는 애니메이션 실행
        Vector3 playerPosition = playerTransform.position;
        Vector3 monsterPosition = transform.position;
        Vector3 monsterToPlayerVector = playerPosition - monsterPosition;

        // 몬스터가 플레이어 바라보는 방향에 따라 스프라이트를 뒤집어줌
        if (monsterToPlayerVector.x < 0)
        {
            renderer.flipX = false;
            shadow.localPosition = new Vector2(0.1f, -0.32f);
        }
        else
        {
            renderer.flipX = true;
            shadow.localPosition = new Vector2(-0.1f, -0.32f);
        }

        monsterToPlayerVector = monsterToPlayerVector.normalized * moveSpeed * Time.deltaTime;
        transform.Translate(monsterToPlayerVector);
    }

    bool isAttacking = false;

    // 패턴2: 플레이어를 공격함
    void Attack()
    {
        StopRunning();  // 달리는 애니메이션 정지
        if (!isAttacking)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
            Invoke("ShootBullet", 0.9f);
            Invoke("StopAttack", 1.3f);
        }
    }

    // 미리 구해놓은 원 좌표에 총알을 생성하고 각 방향으로 이동시킴
    void ShootBullet()
    {
        CameraController.Shake();
        PlayRandomShootSound();

        for (int i=0; i<vertices.Count; ++i)
        {
            Vector2 vertice = vertices[i];
            Vector2 bossPosition = new Vector2(transform.position.x, transform.position.y);
            GameObject spawnedBullet = Instantiate(bullet, vertice + bossPosition, Quaternion.identity);
            spawnedBullet.GetComponent<Rigidbody2D>().velocity = GetDirectionFromAngle(i) * 2f;
            spawnedBullet.GetComponent<BossBulletController>().SetBoss(this);
        }
    }

    // 공격 사운드 재생
    void PlayRandomShootSound()
    {
        RandomlySelectShootSound();
        audioSource.Play();
    }

    // 공격 사운드 랜덤 선택
    void RandomlySelectShootSound()
    {
        int r = Random.Range(0, shootSounds.Length);
        audioSource.clip = shootSounds[r];
    }

    // 각도로 방향 벡터 구하는 함수
    Vector2 GetDirectionFromAngle(int angle)
    {
        angle = (angle + 1) * 12;
        return Quaternion.Euler(new Vector3(0f, 0f, angle)) * Vector2.up;
    }

    // 공격 딜레이에 사용되는 함수
    void StopAttack()
    {
        isAttacking = false;
    }

    // 달리는 애니메이션 정지
    void StopRunning()
    {
        if (animator.GetBool("Running"))
            animator.SetBool("Running", false);
    }

    // 달리는 애니메이션 실행
    void StartRunning()
    {
        if (!animator.GetBool("Running"))
            animator.SetBool("Running", true);
    }

    // 패턴3: 멈춰서 아무 행동도 하지 않음
    void Stop()
    {
        StopRunning();
    }

    const int FOLLOW_MODE = 0;
    const int ATTACK_MODE = 1;
    const int STOP_MODE = 2;
    int mode = STOP_MODE;

    const float WAIT_TIME_FOR_CHANGE_MODE = 5f;

    // 5초마다 패턴을 바꿈
    private void RandomlySelectMode()
    {
        int rand = 0;
        do
        {
            rand = Random.Range(0, 2);
        } while (rand == mode);
        float wait = WAIT_TIME_FOR_CHANGE_MODE;
        switch (rand)
        {
            case FOLLOW_MODE:
                StartFollow();
                break;
            case ATTACK_MODE:
                StartFollow();
                break;
            case STOP_MODE:
                StopMove();
                wait = 1f;
                break;
        }
        mode = rand;
        Invoke("RandomlySelectMode", wait);
    }

    void StartFollow()
    {
        mode = FOLLOW_MODE;
    }

    void StartAttack()
    {
        mode = ATTACK_MODE;
    }

    void StopMove()
    {
        mode = STOP_MODE;
        animator.SetTrigger("Idle");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isSceneEnded) return;
        if (isDead) return;

        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Particle"))
            return;
        
        switch (collision.gameObject.tag)
        {
            case "Player":  // 플레이어, 보스 충돌 시 플레이어에게 데미지 부여
                collision.gameObject.GetComponent<PlayerController>().DamagedByMonster(gameObject, 20f);
                break;
            case "Particle":// 플레이어의 총알과 충돌했을 시, 총알의 충돌 횟수만큼 데미지 받음
                if (collision.gameObject.GetComponent<BulletController>() == null)
                    return;
                int bounce = collision.gameObject.GetComponent<BulletController>().GetBounce() - 1;
                Damaged(bounce);
                Debug.Log(bounce);
                break;
        }
    }

    // 보스 피격 처리
    void Damaged(int damage)
    {
        bounceForDead -= damage;
        if (bounceForDead <= 0)
        {
            bounceForDead = 0;
            isDead = true;
            StopRunning();
            animator.SetTrigger("Die");
            Time.timeScale = 0.5f;  // 죽을 때 0.5배속으로 재생됨
            Invoke("Die", 1.5f);    // 죽고 1.5초 후 종료
        }
        hpBar.remainHP = bounceForDead;
    }

    void Die()
    {
        Time.timeScale = 1f;
        Invoke("ShowClearUI", 1f);
    }

    // 클리어 화면 표시
    void ShowClearUI()
    {
        cleared = true;
        Instantiate(clearUI, Vector2.zero, Quaternion.identity);
    }
}
