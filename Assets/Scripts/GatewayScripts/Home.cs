using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    GameObject doorPrefab;
    GameObject HomePrefab;
    Vector3 doorPosition;
    Transform home;
    Vector3 homePosition;
    Vector3 newDoorPosition;
    Transform player;

    private void Awake()
    {
        GameObject.Find("Player").GetComponent<Door>().enabled=false;
    }

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        doorPrefab = Resources.Load<GameObject>("Door");
        HomePrefab = Resources.Load<GameObject>("Home");
        home = GameObject.Find("Home").GetComponent<Transform>();
       

        homePosition = new Vector3(home.position.x, home.position.y + 10, home.position.z + 16);
    }
    private void Update()
    {
        doorPosition = new Vector3(player.position.x + 1, player.position.y, player.position.z);
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject door = Instantiate(doorPrefab, doorPosition, Quaternion.Euler(0f, 90.0f, 0f));
            DoorManager.Instance.DoorAdd(door);
            GameObject newHome = Instantiate(HomePrefab, homePosition, Quaternion.Euler(0f, 0f, 180.0f));
            newDoorPosition = homePosition - (doorPosition - home.position);
            GameObject newDoor = Instantiate(doorPrefab, newDoorPosition, Quaternion.Euler(0f, 90.0f, 0f));
            DoorManager.Instance.DoorAdd(newDoor);
            GameObject.Find("Player").GetComponent<Door>().enabled = true;
        }
    }
}
