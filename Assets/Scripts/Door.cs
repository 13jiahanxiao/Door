using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Door : MonoBehaviour
{
    public GameObject doorParent; //door所创建的新房间对应的所有门的parent
    public GameManager g;
    public void Start()
    {
        g = GameObject.FindObjectOfType<GameManager>();
    }
    public abstract void OnTriggerEnter(Collider collider);//改变房间以及墙壁碰撞
}
public class PurpleDoor : Door
{
    public PurpleDoor(GameObject doorParent)
    {
        this.doorParent = doorParent;
    }
    public override void OnTriggerEnter(Collider collider)
    {
        change();
    }
    void change()
    {
        if(g.cr==GameManager.currentRoom.room1)
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

