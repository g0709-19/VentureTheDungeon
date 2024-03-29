﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class RoomEnter : MonoBehaviour
{
    public GameObject block;
    public AudioSource audioSource;
    public PlayableDirector playable;

    private CameraController camera;
    private MonsterTemplates monsters;

    private bool isBossRoom = false;
    private GameObject boss;
    public AudioSource bgmSource;
    public AudioClip bossBGM;

    public void SetBossRoom()
    {
        isBossRoom = true;
    }

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        monsters = GameObject.FindGameObjectWithTag("Monsters").GetComponent<MonsterTemplates>();
        boss = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>().boss;
        playable = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayableDirector>();
        bgmSource = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isBossRoom)
        {
            if (!cleared && spawned)
            {
            }
        } else
        {
            if (!cleared && spawned && CountSurviveMonsters() == 0)
            {
                DisableBlock();
                spawned = false;
                cleared = true;
            }
        }
    }

    int CountSurviveMonsters()
    {
        int count = 0;
        for (int i = 0; i < spawnedMonsters.Count; ++i)
        {
            if (spawnedMonsters[i] != null)
                ++count;
        }
        return count;
    }

    List<GameObject> spawnedMonsters = new List<GameObject>();
    bool spawned = false;
    bool cleared = false;

    void BlockRoom()
    {
        if (block == null) return;
        EnableBlock();
        Invoke("SpawnMonster", 1f);
    }

    void SpawnMonster()
    {
        int rand = Random.Range(0, monsters.monsters.Length);
        GameObject monster = monsters.monsters[rand];
        const int AMOUNT_FOR_SPAWN = 3;
        for (int i = 0; i < AMOUNT_FOR_SPAWN; ++i)
        {
            Vector2 position = RandomPosition();
            GameObject spawnedMonster = Instantiate(monster, position, Quaternion.identity);
            spawnedMonsters.Add(spawnedMonster);
        }
        audioSource.Play();
        spawned = true;
    }

    const int ROOM_WIDTH = 3;
    const int ROOM_HEIGHT = 3;
    const int WALL_GAP = 0;

    Vector2 RandomPosition()
    {
        const float halfWidth = ROOM_WIDTH / 2.0f;
        const float halfHeight = ROOM_HEIGHT / 2.0f;
        float x = Random.Range(-halfWidth + WALL_GAP, halfWidth - WALL_GAP);
        float y = Random.Range(-halfHeight + WALL_GAP, halfHeight - WALL_GAP);
        return new Vector2(x + transform.position.x, y + transform.position.y);
    }

    void EnableBlock()
    {
        if (block == null) return;
        block.SetActive(true);
    }

    void DisableBlock()
    {
        if (block == null) return;
        block.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 벽에 끼지 않도록 플레이어를 방 안쪽으로 밀어냄
            Vector2 playerDirection = (transform.position - collision.transform.position).normalized;
            playerDirection *= 1.5f;
            playerDirection += new Vector2(collision.gameObject.transform.position.x,
                collision.gameObject.transform.position.y);
            collision.gameObject.GetComponent<Rigidbody2D>().transform.position = playerDirection;

            camera.MoveTo(transform.position);  // 화면을 들어간 방으로 전환함
            if (cleared || spawned) return;
            if (isBossRoom)
            {
                EnterBossRoom();
            } else
            {
                BlockRoom();
            }
        }
    }

    void EnterBossRoom()
    {
        EnableBlock();
        Invoke("SpawnBossMonster", 1f);
    }

    void SpawnBossMonster()
    {
        bgmSource.Stop();
        playable.Play();
        Instantiate(boss, transform.position, Quaternion.identity);
        Invoke("PlayBossBGM", 0.5f);
    }

    void PlayBossBGM()
    {
        bgmSource.clip = bossBGM;
        bgmSource.Play();
    }
}
