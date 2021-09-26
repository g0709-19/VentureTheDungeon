using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{

    public AudioSource doorOpenAudio;

    BoxCollider2D boxCollider;

    bool isOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpened) return;
        if (collision.gameObject.tag == "Player")
        {
            doorOpenAudio.Play();
            Debug.Log("Touched");
            isOpened = true;
            gameObject.SetActive(false);
        }
    }
}
