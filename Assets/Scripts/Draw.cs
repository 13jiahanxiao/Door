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
   // Vector3 newDoorPosition;

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
        if (GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].number <= GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].count)
        {//当前蜡笔数量<=当颜色蜡笔的总数量
            GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].number++;
            CreateDoor(hit);
        }
        else
        {
            Debug.Log("画笔耗尽");
        }
    }
    void CreateDoor(RaycastHit hit)
    {
        //newDoorPosition = hit.transform.position - room.transform.position + room1.transform.position;//新门位置

        Material color = Resources.Load<Material>("DoorColor/"+GameManager.Instance.CrayonColorName());//门颜色

        GameObject go = GameManager.Instance.currentRoom.gameObject;
        go.GetComponent<Room>().hideIndex.Add(new int[2] {hit.transform.parent.GetSiblingIndex(), hit.transform.GetSiblingIndex() });
        hit.transform.gameObject.SetActive(false);
        GameObject door = Instantiate<GameObject>(Resources.Load<GameObject>("Door"), hit.transform.position, hit.transform.rotation, go.transform);
        door.GetComponent<Door>().toStartHouse = (GameManager.Instance.currentCrayon == (int)GameManager.DoorColor.WHITE);
        door.GetComponent<Renderer>().material = color;
        Debug.Log("1cross" + Vector3.Cross(door.transform.forward,door.transform.up));
        CreateOtherDoor(door,color);
    }
    void CreateOtherDoor(GameObject door, Material color)
    {
        GameObject newroom = new GameObject("RoomManager");
        newroom.transform.position = new Vector3(0,0,0);
        newroom.AddComponent<Room>();
        switch (GameManager.Instance.currentRoom.house)
        {
            case GameManager.houseNumber.House0:
                newroom.GetComponent<Room>().house = GameManager.houseNumber.House1;
                break;
            case GameManager.houseNumber.House1:
                newroom.GetComponent<Room>().house = GameManager.houseNumber.House2;
                break;
            case GameManager.houseNumber.House2:
                newroom.GetComponent<Room>().house = GameManager.houseNumber.House1;
                break;
        }
        Vector3 newDoorPos = door.transform.position + Vector3.Cross(door.transform.forward, door.transform.up) *0.5f;
        GameObject otherDoor = Instantiate<GameObject>(Resources.Load<GameObject>("Door"), newDoorPos, door.transform.rotation,newroom.transform);
        ConnectDoor(door, otherDoor);//两门的door类中互相保存对方地址
        otherDoor.GetComponent<Door>().toStartHouse = (GameManager.Instance.currentRoom.house == GameManager.houseNumber.House0); //标记是否通向初始房
        otherDoor.transform.Rotate(new Vector3(0, 0, 180), Space.Self);
        otherDoor.GetComponent<Renderer>().material = color;
        houseChange(GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].color,newroom.GetComponent<Room>(),otherDoor.GetComponent<Door>());
        Debug.Log("2cross" + Vector3.Cross(otherDoor.transform.forward,otherDoor.transform.up));
        newroom.SetActive(false);//创建时隐藏
    }
    void ConnectDoor(GameObject door1,GameObject door2)
    {
        door1.GetComponent<Door>().position = door1.transform.position;
        door2.GetComponent<Door>().position = door2.transform.position;
        door1.GetComponent<Door>().targetDoor = door2.GetComponent<Door>();
        door2.GetComponent<Door>().targetDoor = door1.GetComponent<Door>();
    }

    void houseChange(GameManager.DoorColor color,Room room,Door door)
    {
        Debug.Log("利用door的位置计算房间位置和旋转,然后将position等属性赋给room");
        switch (color)
        {
            case GameManager.DoorColor.PURPLE:
                room.houseRotationEular = GameManager.Instance.currentRoom.houseRotationEular + new Vector3(180,0,0);
                Vector3 diff = GameManager.Instance.houseObject[(int)GameManager.Instance.currentRoom.house].transform.position - door.targetDoor.gameObject.transform.position;
                Vector3 cross = Vector3.Cross(door.transform.forward, door.transform.up);
                Vector3 newdiff = diff + new Vector3(cross.x * diff.x, cross.y * diff.y, cross.z * diff.z) * 2;
                room.housePosition = newdiff + door.gameObject.transform.position;
                //otherDoor.transform.position
                //room.houseposition..
                break;
            default:
                break;
        }
    }
}
