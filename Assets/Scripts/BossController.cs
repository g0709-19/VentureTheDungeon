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
    public int bounceForDead;

    public AudioSource audioSource;
    public AudioClip[] shootSounds;

    public GameObject bullet;
    public Canvas clearUI;

    public clockScript hpBar;

    Collider2D collider;
    public bool isDead = false;

    public static bool cleared = false;

    bool isSceneEnded = false;

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

    IEnumerator SceneEnd()
    {
        yield return new WaitForSeconds(1.5f);
        isSceneEnded = true;
        hpBar.gameObject.SetActive(true);
    }

    void InitBulletPath()
    {
        vertices = new List<Vector2>();
        float heading;
        for (int theta = 0; theta < 360; theta += 360 / 20)
        {
            heading = theta * Mathf.Deg2Rad;
            vertices.Add(new Vector2(Mathf.Cos(heading) * radius, Mathf.Sin(heading) * radius));
        }
    }

    void FixedUpdate()
    {
        if (!isSceneEnded) return;
        if (isDead) return;
        switch (mode)
        {
            case FOLLOW_MODE: Follow(); break;
            case ATTACK_MODE: Attack(); break;
            case STOP_MODE: Stop(); break;
        }
    }

    void Follow()
    {
        StartRunning();
        Vector3 playerPosition = playerTransform.position;
        Vector3 monsterPosition = transform.position;
        Vector3 monsterToPlayerVector = playerPosition - monsterPosition;

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

    void Attack()
    {
        StopRunning();
        if (!isAttacking)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
            Invoke("ShootBullet", 0.9f);
            Invoke("StopAttack", 1.3f);
        }
    }

    List<Vector2> vertices;
    float radius = 2f;

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

    void PlayRandomShootSound()
    {
        RandomlySelectShootSound();
        audioSource.Play();
    }

    void RandomlySelectShootSound()
    {
        int r = Random.Range(0, shootSounds.Length);
        audioSource.clip = shootSounds[r];
    }

    Vector2 GetDirectionFromAngle(int angle)
    {
        angle = (angle + 1) * 13;
        return Quaternion.Euler(new Vector3(0f, 0f, angle)) * Vector2.up;
    }

    void StopAttack()
    {
        isAttacking = false;
    }

    void StopRunning()
    {
        if (animator.GetBool("Running"))
            animator.SetBool("Running", false);
    }

    void StartRunning()
    {
        if (!animator.GetBool("Running"))
            animator.SetBool("Running", true);
    }

    void Stop()
    {
        StopRunning();
    }

    const int FOLLOW_MODE = 0;
    const int ATTACK_MODE = 1;
    const int STOP_MODE = 2;
    int mode = STOP_MODE;

    const float WAIT_TIME_FOR_CHANGE_MODE = 5f;

    private void RandomlySelectMode()
    {
        int rand = 0;
        do
        {
            rand = Random.Range(0, 3);
        } while (rand == mode);
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
                break;
        }
        mode = rand;
        Invoke("RandomlySelectMode", WAIT_TIME_FOR_CHANGE_MODE);
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
            case "Player":
                collision.gameObject.GetComponent<PlayerController>().DamagedByMonster(gameObject, 20f);
                break;
            case "Particle":
                if (collision.gameObject.GetComponent<BulletController>() == null)
                    return;
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
            StopRunning();
            animator.SetTrigger("Die");
            Time.timeScale = 0.5f;
            Invoke("Die", 1.5f);
        }
        hpBar.remainHP = bounceForDead;
    }

    void Die()
    {
        Time.timeScale = 1f;
        Invoke("ShowClearUI", 1f);
    }

    void ShowClearUI()
    {
        cleared = true;
        Instantiate(clearUI, Vector2.zero, Quaternion.identity);
    }
}
