using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "DoorPosition")
        {
            other.gameObject.SetActive(false);
        }
    }
    */
    public Vector3 position;
    public bool toStartRoom;  //若为true则需要更新初始房的roomManager位置
    public Door targetDoor;
    public GameManager.DoorColor color;
}
