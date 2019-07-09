using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        if (!collider.isTrigger)
        {
            transform.parent.GetComponent<PlayerControl>().onGround = true;
        }
        if (collider.tag == "Door")
        {
            collider.GetComponent<Door>().targetDoor.transform.parent.gameObject.SetActive(true);
            GameManager.Instance.currentRoom = collider.transform.parent.GetComponent<Room>();
            if (collider.GetComponent<Door>().color==GameManager.DoorColor.WHITE||collider.GetComponent<Door>().color==GameManager.DoorColor.BLACK)
            {
                for (int i = 0; i < GameManager.Instance.lastRoom.hideIndex.Count; i++)
                {
                    GameManager.Instance.houseObject[(int)GameManager.Instance.lastRoom.house].transform.GetChild(GameManager.Instance.lastRoom.hideIndex[i][0]).GetChild(GameManager.Instance.lastRoom.hideIndex[i][1]).gameObject.SetActive(true);
                }
                GameManager.Instance.lastRoom.gameObject.SetActive(false);
                GameManager.Instance.lastRoom = GameManager.Instance.currentRoom;
                if (GameManager.Instance.blackDoor != null)
                {
                    GameManager.Instance.blackDoor.transform.parent.GetComponent<Room>();
                }
                else
                {
                    GameManager.Instance.currentRoom = collider.GetComponent<Door>().targetDoor.transform.parent.GetComponent<Room>();
                }
                GameManager.Instance.RefreshRoom(GameManager.Instance.startRoom);
                collider.GetComponent<Door>().targetDoor.transform.position
                   = GameManager.Instance.houseObject[(int)GameManager.Instance.startRoom.house].transform.GetChild(GameManager.Instance.hide[0][0]).GetChild(GameManager.Instance.hide[0][1]).position +
                    collider.GetComponent<Door>().targetDoor.transform.up * GameManager.Instance.wallThickness / 2;
                collider.GetComponent<Door>().targetDoor.transform.Rotate(new Vector3(0, 180, 0),Space.Self);
                Debug.Log(GameManager.Instance.houseObject[(int)GameManager.Instance.startRoom.house].transform.GetChild(GameManager.Instance.hide[0][0]).GetChild(GameManager.Instance.hide[0][1]).position);
                Debug.Log(collider.GetComponent<Door>().targetDoor.transform.up * GameManager.Instance.wallThickness / 2);
                if (GameManager.Instance.blackDoor != null)
                {
                    collider.GetComponent<Door>().targetDoor = GameManager.Instance.blackDoor;
                }
                //for (int i = 0; i < GameManager.Instance.lastRoom.hideIndex.Count; i++)
                //{
                //    GameManager.Instance.houseObject[(int)GameManager.Instance.lastRoom.house].transform.GetChild(GameManager.Instance.lastRoom.hideIndex[i][0]).GetChild(GameManager.Instance.lastRoom.hideIndex[i][1]).gameObject.SetActive(true);
               // }
                //GameManager.Instance.lastRoom.gameObject.SetActive(false);
               // GameManager.Instance.lastRoom = GameManager.Instance.currentRoom;
               // GameManager.Instance.currentRoom = collider.GetComponent<Door>().targetDoor.transform.parent.GetComponent<Room>();
               // GameManager.Instance.RefreshRoom(GameManager.Instance.startRoom);
                collider.GetComponent<Door>().targetDoor.transform.parent.gameObject.SetActive(true);
                if (GameManager.Instance.blackDoor != null)
                {
                    collider.GetComponent<Door>().targetDoor.transform.position
                        = GameManager.Instance.houseObject[(int)GameManager.Instance.startRoom.house].transform.GetChild(GameManager.Instance.hide[1][0]).GetChild(GameManager.Instance.hide[1][1]).position +
                         collider.GetComponent<Door>().targetDoor.transform.up * GameManager.Instance.wallThickness / 2;
                }
                //角色坐标转换 并且为了避免bug 所以要传送到偏前的位置
                //GameManager.Instance.player.GetComponent<Rigidbody>().velocity = GameManager.Instance.player.GetComponent<Rigidbody>().velocity.sqrMagnitude * collider.GetComponent<Door>().targetDoor.transform.up;
                GameManager.Instance.player.transform.position = collider.GetComponent<Door>().targetDoor.transform.position + collider.GetComponent<Door>().targetDoor.transform.up * 2;
                Vector3 currentVelocity = GameManager.Instance.player.GetComponent<Rigidbody>().velocity;
                Vector3 newVelocity = currentVelocity.magnitude * collider.GetComponent<Door>().targetDoor.transform.up.normalized;
                GameManager.Instance.player.GetComponent<Rigidbody>().AddForce(newVelocity - currentVelocity, ForceMode.VelocityChange);
                //Debug.Log(GameManager.Instance.player.GetComponent<Rigidbody>().velocity);

                //Debug.Log(collider.GetComponent<Door>().targetDoor.transform.position + collider.GetComponent<Door>().targetDoor.transform.up * 2);
                //Debug.Log(Time.time);
                StartCoroutine(dontMove());
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
                if (GameManager.Instance.lastRoom != null)
                {
                    if (GameManager.Instance.lastRoom.transform != targetRoom.transform && GameManager.Instance.lastRoom.transform != GameManager.Instance.currentRoom.transform)
                    {
                        GameManager.Instance.lastRoom.gameObject.SetActive(false); //关闭lastRoom的roomManager
                    }
                }
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
    IEnumerator dontMove()
    {
        GameManager.Instance.canMove = false;
        yield return new WaitForSeconds(0.04f);
        GameManager.Instance.canMove = true;
    }
}
