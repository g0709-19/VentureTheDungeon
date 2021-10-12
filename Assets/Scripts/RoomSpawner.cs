using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{

    public int openingDirection;

    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;
    private Grid parent;

    public float waitTime = 4f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
        parent = GetComponentInParent<Grid>();
    }

    const int BOTTOM = 1;
    const int TOP = 2;
    const int LEFT = 3;
    const int RIGHT = 4;
    
    void Spawn()
    {
        if (spawned == false)
        {
            switch (openingDirection)
            {
                case BOTTOM:
                    rand = Random.Range(0, templates.bottomRooms.Length);
                    InstantiateInParent(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                    break;
                case TOP:
                    rand = Random.Range(0, templates.topRooms.Length);
                    InstantiateInParent(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
                    break;
                case LEFT:
                    rand = Random.Range(0, templates.leftRooms.Length);
                    InstantiateInParent(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                    break;
                case RIGHT:
                    rand = Random.Range(0, templates.rightRooms.Length);
                    InstantiateInParent(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                    break;
            }
            spawned = true;
        }
    }

    GameObject InstantiateInParent(GameObject original, Vector3 position, Quaternion rotation)
    {
        GameObject createdObject = Instantiate(original, position, rotation);
        createdObject.transform.parent = parent.transform;
        return createdObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint"))
        {
            if (collision.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                InstantiateInParent(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
