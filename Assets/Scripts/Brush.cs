using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush :MonoBehaviour
{
    //public Door doorPrefeb; //prefab
    private GameManager g;
    public int num;  //画笔个数
    public GameManager.DoorColor color;//画笔颜色
    void Awake()
    {
        //g = GameObject.FindObjectOfType<GameManager>();
        //Debug.Log("666");
    }
    public void paint(RaycastHit hit)
    {
        g = GameObject.FindObjectOfType<GameManager>();
        if (num > 0)
        {
            num--;
            Debug.Log("剩余数量" + num);
            GameObject door = Instantiate(Resources.Load("Door",typeof(GameObject)), g.currentParent.transform) as GameObject; //door为此次生成的门，otherDoor是同时生成出的对面房间的门
            door.transform.position = hit.point;
            door.GetComponent<Door>().color = this.color;
            bool doorOnGround = Vector3.Angle(g.player.transform.up, hit.transform.up) < 30;
            if (doorOnGround)
            {
                door.transform.eulerAngles = new Vector3(180, g.camera.transform.eulerAngles.y, 0); //保证transform.up方向统一
            }
            else
            {
                door.transform.rotation = hit.transform.rotation;
            }
            Debug.Log(door.transform.up);

            door.GetComponent<Door>().doorParent = new GameObject("DoorParent"); //生成新房间的doorParent并赋给此房间的新门
            GameObject otherDoor = Instantiate(Resources.Load("Door", typeof(GameObject)), door.GetComponent<Door>().doorParent.transform) as GameObject;
            otherDoor.GetComponent<Door>().undo = true;
            otherDoor.GetComponent<Door>().color = this.color;
            //确定对面门的位置+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            otherDoor.gameObject.transform.localPosition = door.gameObject.transform.localPosition;
            otherDoor.GetComponent<Door>().doorParent = door.transform.parent.gameObject;
            door.GetComponent<Door>().doorParent.SetActive(false);
            //Debug.Log(door.GetComponent<Door>().doorParent.activeSelf);
            //switch..case..color
            //改变门对应材质贴图+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        }
        else
        {
            Debug.Log("画笔耗尽");
        }
    }
}