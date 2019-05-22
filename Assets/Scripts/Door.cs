using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject doorParent; //door所创建的新房间对应的所有门的parent
    public GameManager g;
    public enum DoorColor
    {
        red, green, purple
    }
    public DoorColor doorColor;
    public void Start()
    {
        g = GameObject.FindObjectOfType<GameManager>();
    }
    public void OnTriggerEnter(Collider collider)//改变房间以及墙壁碰撞
    {
        //switch..case..
        purpleChange();
    }
    void purpleChange()
    {
        if (g.cr == GameManager.currentRoom.room1)
        {
            g.room2.gameObject.transform.position = g.room1.transform.position
                + new Vector3(transform.up.x * g.moveDistace.x, transform.up.y * g.moveDistace.y, transform.up.z * g.moveDistace.z); //moveDistance为房间长宽高
        }
        else
        {
            g.room1.gameObject.transform.position = g.room2.transform.position
                + new Vector3(transform.up.x * g.moveDistace.x, transform.up.y * g.moveDistace.y, transform.up.z * g.moveDistace.z); //moveDistance为房间长宽高
        }
    }
}
