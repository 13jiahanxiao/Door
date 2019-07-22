using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestDoor : MonoBehaviour
{
    Transform door1, door2;
    public float speed = 1;
    void Start()
    {
        door1 = transform.Find("ChestDoor1");
        door2 = transform.Find("ChestDoor2");
    }
    public void chestDoorOpen()
    {
        StartCoroutine(open());
    }
    IEnumerator open()
    {
        while (door1.localEulerAngles.y < 90)
        {
            door1.Rotate(Vector3.up, Time.deltaTime * speed, Space.Self);
            door2.Rotate(Vector3.up, -Time.deltaTime * speed, Space.Self);
            yield return null;
        }
    }
}
