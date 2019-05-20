using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    GameObject player;
    GameObject player_Sub;
    GameObject door;
    GameObject door1;
    Vector3 dis;
    private void Start()
    {
        player = GameObject.Find("Player");
        player_Sub = GameObject.Find("Player_Sub");
        door = DoorManager.Instance.doorManagerList[0];
        door1 = DoorManager.Instance.doorManagerList[1];
    }
    private void Update()
    {
        dis = door.transform.position - player.transform.position;
        player_Sub.transform.position = door1.transform.position + dis;
        player_Sub.transform.position = new Vector3 (player_Sub.transform.position.x,player.transform.position.y,player_Sub.transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Door(Clone)")
        {
            Vector3 temp = player.transform.position;
            player.transform.position = player_Sub.transform.position;
            player_Sub.transform.position = temp;
            Vector3 tempDoor = door.transform.position;
            door.transform.position = door1.transform.position;
            door1.transform.position = tempDoor;
        }
    }
}
