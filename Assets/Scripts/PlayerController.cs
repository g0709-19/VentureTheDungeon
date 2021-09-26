using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public AudioSource walkSound;

    float h, v;
    Rigidbody2D rigidBody;
    Animator animator;
    SpriteRenderer spriteRenderer;

    const string IS_WALKING = "isWalking";

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
                Debug.Log("start walking motion");
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
        Vector2 newVelocity = new Vector2(h, v);
        newVelocity *= speed;
        rigidBody.velocity = newVelocity;
    }
}
