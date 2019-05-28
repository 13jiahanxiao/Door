using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room:MonoBehaviour
{
    public GameManager.houseNumber house;
    public GameObject room;
    public Vector3 roomPosition;
    public Quaternion roomRotation;
    public List<GameObject> doorList = new List<GameObject>();

    public void AddRoom(GameObject door)
    {
        doorList.Add(door);
    }
}
