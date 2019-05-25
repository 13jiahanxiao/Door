using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject doorParent; //door所创建的新房间对应的所有门的parent,即对面房间的doorParrnt
    public GameManager g;
    public List<GameManager.DoorColor> nextRoomAllChanges; //对面房间的所有属性
    public GameManager.DoorColor color;  //此门的颜色
    public bool undo;   //标记门的效果是变化还是恢复 true则恢复
    public void Start()
    {
        g = GameObject.FindObjectOfType<GameManager>();
    }
    public void OnTriggerEnter(Collider collider)//改变房间以及墙壁碰撞
    {
        //switch..case..判断颜色
        if (collider.tag == "Player")
        {
            if (!undo)
            {
                // purpleChange();不仅仅如此
            }
            else
            {
                // purpleUndo(); 不仅仅如此
            }
            doorParent.SetActive(true);
        }
        void purpleChange()
        {
            Debug.Log("生成紫");
            if (g.cr == GameManager.currentRoom.room1) //change room2+++++++++++++++++++++++++++++++++++++++++++
            {
                // g.room2.transform.eulerAngles = new Vector3(0, 180, 180);
                //垂直翻转+水平翻转 门位置拼接（非镜像）
            }
            else //change room1+++++++++++++++++++++++++++++++++++++++++++
            {
                // g.room1.gameObject.transform.position = g.room2.transform.position
                //    + new Vector3(transform.up.x * g.moveDistace.x, transform.up.y * g.moveDistace.y, transform.up.z * g.moveDistace.z); //moveDistance为房间长宽高
                g.room1.transform.eulerAngles = new Vector3(0, 180, 180);
            }
        }
        void purpleUndo()
        {
            Debug.Log("撤销紫");
            //与purpleChange相反的改变+++++++++++++++++++++++++++++++
        }
        void yellowChange()
        {
            Debug.Log("生成黄");//++++++++++++++++++++++++++++++++++++
        }
        void yellowUndo()
        {
            Debug.Log("撤销黄");//+++++++++++++++++++++++++++++++++++++++++++
        }
    }
}
