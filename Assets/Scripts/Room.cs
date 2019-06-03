using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room:MonoBehaviour
{
    public GameManager.houseNumber house;
   // public GameManager.houseNumber nextHouse;
    public Vector3 housePosition;
    public Vector3 houseRotationEular;
    public List<GameObject> doorList = new List<GameObject>();
    public List<int[]> hideIndex = new List<int[]>();  //被隐藏的子物体下标，被代替时重新显示，代替时隐藏(房间转换时触发)

    public void AddRoom(GameObject door)
    {
        doorList.Add(door);
    }
}
