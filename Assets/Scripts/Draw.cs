using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour {
    private Ray ray;
    private RaycastHit hit;
    public float distance = 4.5f;

    public Vector3 screenCenter;
    
    public Transform doorParent;
    public GameObject door;
    Vector3 newDoorPosition;

    void Start ()
    { 
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }
	
	void Update ()
    {
        ray = Camera.main.ScreenPointToRay(screenCenter);
        if(Physics.Raycast(ray, out hit, distance))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.gameObject.tag == "Box")
                {
                    Debug.Log("拾取物品");
                }
                else if (hit.transform.gameObject.tag == "DoorPosition")
                {
                    Debug.Log("生成门");
                    paint(hit);
                }
                else
                {
                    Debug.Log("无效位置");
                }
            }
        }
    }

    public void paint(RaycastHit hit)
    {
        if (GameManager.Instance.crayonArray[GameManager.Instance.currentCrayon].number <= GameManager.Instance.crayonArray[GameManager.Instance.currentCrayon].count)
        {//当前蜡笔数量<=当颜色蜡笔的总数量
            GameManager.Instance.crayonArray[GameManager.Instance.currentCrayon].number++;
            CreateDoor(hit, GameManager.Instance.roomObject[0], GameManager.Instance.roomObject[1]);
        }
        else
        {
            Debug.Log("画笔耗尽");
        }
    }
    void CreateDoor(RaycastHit hit, Transform room, Transform room1)
    {
        newDoorPosition = hit.transform.position - room.transform.position + room1.transform.position;//新门位置

        Material color = Resources.Load<Material>("DoorColor/"+GameManager.Instance.CrayonColorName());//门颜色

        GameObject go;

        if (GameManager.Instance.isStart)//判断如果是第一个房间，新建room（doorparent），不是则用之前的
        {
            go = GameManager.Instance.CreateRoom(room.position,room.rotation);
            GameManager.Instance.currentRoom.house = GameManager.houseNumber.House1;
        }
        else
        {
            go = GameManager.Instance.currentRoom.room;
        }

        hit.transform.gameObject.SetActive(false);
        GameObject door = Instantiate<GameObject>(Resources.Load<GameObject>("Door"), hit.transform.position, hit.transform.rotation, go.transform);
        door.GetComponent<Renderer>().material = color;

        CreateOtherDoor(door,color,room1);
    }
    void CreateOtherDoor(GameObject door,Material color,Transform room1)
    {
        GameObject go = GameManager.Instance.CreateRoom(room1.position, room1.rotation);
        if (GameManager.Instance.isStart)
        {
            GameManager.Instance.currentRoom.house = GameManager.houseNumber.House2;
            GameManager.Instance.isStart=false;
        }
        GameObject otherDoor = Instantiate<GameObject>(Resources.Load<GameObject>("Door"), newDoorPosition, door.transform.rotation, go.transform);
        otherDoor.transform.Rotate(new Vector3(0, 0, 180), Space.Self);
        otherDoor.GetComponent<Renderer>().material = color;
        ConnectDoor(door, otherDoor);//两门的door类中互相保存对方地址
    }
    void ConnectDoor(GameObject door1,GameObject door2)
    {
        door1.GetComponent<Door>().position = door1.transform.position;
        door2.GetComponent<Door>().position = door2.transform.position;
        door1.GetComponent<Door>().targetDoor = door2.GetComponent<Door>();
        door2.GetComponent<Door>().targetDoor = door1.GetComponent<Door>();
    }
}
