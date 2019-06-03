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
    public bool toStartHouse;
    public Door targetDoor;
    public GameManager.DoorColor color;
}
