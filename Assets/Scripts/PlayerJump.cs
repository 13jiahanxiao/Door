using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        transform.parent.GetComponent<PlayerControl>().onGround = true;
        if(collider.tag=="Door")
        {
            if (GameManager.Instance.currentRoom.transform != collider.GetComponentInParent<Room>().transform)//两者若相等 则意味着更新前的currentRoom就是门所在的room，即角色没有去另一个房间
            {
                GameManager.Instance.lastRoom = GameManager.Instance.currentRoom;       //将上一个currentRoom更新为lastRoom
                GameManager.Instance.currentRoom = collider.transform.parent.GetComponent<Room>();  //更新currentRoom
            }
            //GameManager.Instance.onMiddle = !(GameManager.Instance.onMiddle);
            collider.GetComponent<Door>().targetDoor.transform.parent.gameObject.SetActive(true);
            Room targetRoom =collider.GetComponent<Door>().targetDoor.GetComponentInParent<Room>();
            GameManager.Instance.RefreshRoom(targetRoom);
        }
    }
    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Middle")
        {
            GameManager.Instance.onMiddle = true;
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if(collider.tag=="Middle")
        {
            GameManager.Instance.onMiddle = false;
        }
    }
}
