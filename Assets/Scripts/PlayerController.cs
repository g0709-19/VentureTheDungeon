﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public AudioSource walkSound;
    public GunController gun;
    public Canvas gameOverUI;
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
        // 플레이어가 죽었거나, 보스 클리어했을 경우 애니메이션 정지
        if (isDead || BossController.cleared)
        {
            if (isWalking())
            {
                stopWalkMotion();
                stopWalkSound();
            }
            return;
        }

        // 수평, 수직 키보드로부터 입력 받음
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
            stopWalkSound();
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

    void stopWalkSound()
    {
        if (walkSound.isPlaying)
            walkSound.Stop();
    }

    bool isPlayingWalkMotion()
    {
        return animator.GetBool(IS_WALKING);
    }

    void stopWalkMotion()
    {
        animator.SetBool(IS_WALKING, false);
    }

    public void BuffBulletSpeed()
    {
        gun.MakeFasterBulletSpeed();
    }

    public void BuffShootDelay()
    {
        gun.MakeFasterShootSpeed();
    }

    bool hasOpened = false;

    public bool CanOpenChest()
    {
        if (hasOpened)
            return false;
        hasOpened = true;
        return true;
    }

    const int MOUSE_LEFT = 0;
    const int MAIN_SCENE = 2;

    private void FixedUpdate()
    {
        // 죽은 상태에서 마우스 클릭 시, 새로 시작
        if (isDead)
        {
            if (Input.GetMouseButtonDown(MOUSE_LEFT))
                SceneManager.LoadScene(MAIN_SCENE);
            return;
        }

        // 플레이어 이동
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
        gameOverUI.gameObject.SetActive(true);
        isDead = true;
    }
}
