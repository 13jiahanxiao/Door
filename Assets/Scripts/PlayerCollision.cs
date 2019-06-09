using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        transform.parent.GetComponent<PlayerControl>().onGround = true;
        if (collider.tag == "Door")
        {
            collider.GetComponent<Door>().targetDoor.transform.parent.gameObject.SetActive(true);
            GameManager.Instance.currentRoom = collider.transform.parent.GetComponent<Room>();
            if (collider.GetComponent<Door>().color==GameManager.DoorColor.WHITE)
            {
                for (int i = 0; i < GameManager.Instance.lastRoom.hideIndex.Count; i++)
                {
                    GameManager.Instance.houseObject[(int)GameManager.Instance.lastRoom.house].transform.GetChild(GameManager.Instance.lastRoom.hideIndex[i][0]).GetChild(GameManager.Instance.lastRoom.hideIndex[i][1]).gameObject.SetActive(true);
                }
                GameManager.Instance.lastRoom = GameManager.Instance.currentRoom;
                GameManager.Instance.currentRoom = GameManager.Instance.startRoom;
                Room targetRoom = collider.GetComponent<Door>().targetDoor.GetComponentInParent<Room>();
                GameManager.Instance.RefreshRoom(targetRoom);
                collider.GetComponent<Door>().targetDoor.transform.position 
                    = GameManager.Instance.houseObject[(int)GameManager.Instance.startRoom.house].transform.GetChild(GameManager.Instance.hide[0]).GetChild(GameManager.Instance.hide[1]).position +
                     collider.GetComponent<Door>().targetDoor.transform.up * GameManager.Instance.wallThickness /2;
                 //角色坐标转换 并且为了避免bug 所以要传送到偏前的位置
                 GameManager.Instance.player.transform.position = collider.GetComponent<Door>().targetDoor.transform.position + collider.GetComponent<Door>().targetDoor.transform.up * 2;
            }
            else
            {
                //if (GameManager.Instance.currentRoom.transform != collider.transform.parent)//两者若相等 则意味着更新前的currentRoom就是门所在的room，即角色没有去另一个房间
                {
                   // GameManager.Instance.lastRoom = GameManager.Instance.currentRoom;       //将上一个currentRoom更新为lastRoom
                    //GameManager.Instance.currentRoom = collider.transform.parent.GetComponent<Room>();  //更新currentRoom
                }
                //GameManager.Instance.onMiddle = !(GameManager.Instance.onMiddle);
                Room targetRoom = collider.GetComponent<Door>().targetDoor.GetComponentInParent<Room>();
                GameManager.Instance.RefreshRoom(targetRoom);
                GameManager.Instance.lastRoom = targetRoom;
            }
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
