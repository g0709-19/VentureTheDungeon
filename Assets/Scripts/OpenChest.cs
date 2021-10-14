using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{

    public AudioSource audioSource;
    public SpriteRenderer renderer;
    public BoxCollider2D collider;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
            renderer.enabled = false;
            collider.enabled = false;
            Invoke("Destroy", 0.173f);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
