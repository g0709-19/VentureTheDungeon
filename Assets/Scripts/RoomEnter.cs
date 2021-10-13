using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnter : MonoBehaviour
{
    CameraController camera;
    public GameObject block;

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    void SpawnMonster()
    {
        if (block == null) return;
        //EnableBlock();
        Debug.Log("spawn monster");
        // Spawn monsters
        // if all kill, disable block
    }

    void EnableBlock()
    {
        block.SetActive(true);
    }

    void DisableBlock()
    {
        block.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            camera.MoveTo(transform.position);
            SpawnMonster();
        }
    }
}
