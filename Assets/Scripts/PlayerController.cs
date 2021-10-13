using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioSource walkSound;
    public Transform gun;
    public float speed = 5.0f;
    public float hp = 20.0f;

    float h, v;
    Rigidbody2D rigidBody;
    Animator animator;
    SpriteRenderer spriteRenderer;
    bool isDead = false;

    const string IS_WALKING = "Walking";

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            if (isWalking()) stopWalkMotion();
            return;
        }

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (isWalking())
        {
            if (isMoveToLeft())
                lookLeft();
            else if (isMoveToRight())
                lookRight();
            if (!walkSound.isPlaying)
                walkSound.Play();
            if (!isPlayingWalkMotion())
            {
                startWalkMotion();
            }
        } else
        {
            if (walkSound.isPlaying)
                walkSound.Stop();
            stopWalkMotion();
        }
    }

    bool isWalking()
    {
        return h < 0 || h > 0 || v < 0 || v > 0;
    }

    bool isMoveToLeft()
    {
        return h < 0;
    }

    bool isMoveToRight()
    {
        return h > 0;
    }

    void lookLeft()
    {
        spriteRenderer.flipX = true;
    }

    void lookRight()
    {
        spriteRenderer.flipX = false;
    }

    void startWalkMotion()
    {
        animator.SetBool(IS_WALKING, true);
    }

    bool isPlayingWalkMotion()
    {
        return animator.GetBool(IS_WALKING);
    }

    void stopWalkMotion()
    {
        animator.SetBool(IS_WALKING, false);
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        Vector2 newVelocity = new Vector2(h, v);
        newVelocity *= speed;
        rigidBody.velocity = newVelocity;
    }

    public void DamagedByMonster(GameObject monster, float damage)
    {
        if (isDead) return;
        hp -= damage;
        if (hp < 0)
            hp = 0;
        Debug.Log(monster + " 에게 " + damage + " 데미지 입음");
        Debug.Log(hp + " 남음");
        if (hp <= 0)
            Die();
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        gun.gameObject.SetActive(false);
        isDead = true;
    }
}
