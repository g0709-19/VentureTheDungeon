using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{

    public AudioSource audioSource;
    public SpriteRenderer renderer;
    public BoxCollider2D collider;

    ShootDelay shootDelay;
    BulletSpeed bulletSpeed;

    private void Start()
    {
        shootDelay = ShootDelay.shootDelay;
        bulletSpeed = BulletSpeed.bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.CanOpenChest())
            {
                GiveEffectToPlayer(player);
                Open();
            }
        }
    }

    void Open()
    {
        audioSource.Play();
        renderer.enabled = false;
        collider.enabled = false;
        Invoke("Destroy", 0.173f);
    }

    void GiveEffectToPlayer(PlayerController player)
    {
        if (Random.Range(0, 1) == 0)
            shootDelay.affect();
        else
            bulletSpeed.affect();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
