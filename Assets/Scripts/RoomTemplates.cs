using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{

    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRoom;

    public List<GameObject> rooms;

    public float waitTime;
    private bool spawnedBoss;
    public GameObject boss;
    public GameObject bossRoom;
    public Grid bossRoomSpawnPoint;

    private void Update()
    {
        if (!spawnedBoss && waitTime <= 0)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    rooms[i].name = "boss";
                    rooms[i].GetComponent<RoomEnter>().SetBossRoom();
                    Instantiate(bossRoom, rooms[i].transform.position,
                        Quaternion.identity).transform.parent = bossRoomSpawnPoint.gameObject.transform;
                    spawnedBoss = true;
                }
            }
        } else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
