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
                    GameManager.Instance.setText("拾取物品");
                }
                else if (hit.transform.gameObject.tag == "DoorPosition")
                {
                    GameManager.Instance.setText("生成门");
                    paint(hit);
                }
                else
                {
                    GameManager.Instance.setText("无效位置");
                }
            }
        }
    }

    public void paint(RaycastHit hit)
    {
        if (GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].number <= GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].count)
        {//当前蜡笔数量<=当颜色蜡笔的总数量
            GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].number++;
            if (!GameManager.Instance.onMiddle)
            {
                CreateDoor(hit);
            }
            else
            {
                GameManager.Instance.setText("不可在此处画门");
            }
        }
        else
        {
            GameManager.Instance.setText("蜡笔耗尽");
        }
    }
    void CreateDoor(RaycastHit hit)
    {
        Material color = Resources.Load<Material>("DoorColor/"+GameManager.Instance.CrayonColorName());//门颜色

        GameObject go = GameManager.Instance.currentRoom.gameObject;
        go.GetComponent<Room>().hideIndex.Add(new int[2] {hit.transform.parent.GetSiblingIndex(), hit.transform.GetSiblingIndex() });  //将消去方格的下标存于room中
        hit.transform.gameObject.SetActive(false);
        GameObject door = Instantiate<GameObject>(Resources.Load<GameObject>("Door"), hit.transform.position+hit.transform.right*1f, hit.transform.rotation, go.transform);
        door.transform.eulerAngles += new Vector3(0, 0, -90);
        door.GetComponent<Door>().toStartRoom = (GameManager.Instance.currentCrayon == (int)GameManager.DoorColor.WHITE);
        door.GetComponent<Renderer>().material = color;
        CreateOtherDoor(door,color,hit);
    }
    void CreateOtherDoor(GameObject door, Material color,RaycastHit hit)
    {
        GameObject newroom = new GameObject("RoomManager");
        newroom.transform.position = new Vector3(0,0,0);
        newroom.AddComponent<Room>();
        newroom.GetComponent<Room>().hideIndex.Add(new int[2] { hit.transform.parent.GetSiblingIndex(), hit.transform.GetSiblingIndex() });
        newroom.GetComponent<Room>().house = (GameManager.houseNumber)(1 - (int)GameManager.Instance.currentRoom.house);//给新房间指定另一个house
        Vector3 newDoorPos = door.transform.position - door.transform.up * 4;
        GameObject otherDoor = Instantiate<GameObject>(Resources.Load<GameObject>("Door"), newDoorPos, door.transform.rotation,newroom.transform);
        ConnectDoor(door, otherDoor);//两门的door类中互相保存对方地址
        otherDoor.GetComponent<Door>().toStartRoom = (GameManager.Instance.currentRoom.house == GameManager.houseNumber.House0); //标记是否通向初始房
        otherDoor.transform.Rotate(new Vector3(0, 0, 180), Space.Self);
        otherDoor.GetComponent<Renderer>().material = color;
        GameManager.Instance.houseChange(GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].color,newroom.GetComponent<Room>(),otherDoor.GetComponent<Door>());
        newroom.SetActive(false);//创建时隐藏
    }
    void ConnectDoor(GameObject door1,GameObject door2)
    {
        door1.GetComponent<Door>().position = door1.transform.position;
        door2.GetComponent<Door>().position = door2.transform.position;
        door1.GetComponent<Door>().targetDoor = door2.GetComponent<Door>();
        door2.GetComponent<Door>().targetDoor = door1.GetComponent<Door>();
    }
    /*  //已移至GameManager
    void houseChange(GameManager.DoorColor color,Room room,Door door)
    {
        //利用door的位置计算房间位置和旋转,然后将position等属性赋给room
        switch (color)
        {
            case GameManager.DoorColor.PURPLE:
                room.houseRotationEular = GameManager.Instance.currentRoom.houseRotationEular + new Vector3(180, 0, 0);// + new Vector3(0, 180 * door.transform.up.x, 0);
                Vector3 diff = GameManager.Instance.houseObject[(int)GameManager.Instance.currentRoom.house].transform.position - door.targetDoor.gameObject.transform.position;
                if (Vector3.Dot(door.transform.up, new Vector3(1, 0, 0)) > 0.1 || Vector3.Dot(door.transform.up, new Vector3(1, 0, 0)) < -0.1)
                {
                    room.houseRotationEular += new Vector3(0, 180, 0);
                    Vector3 newdiff = diff + new Vector3((-1)*diff.x, (-1) * diff.y, 0) * 2;
                    room.housePosition = newdiff + door.gameObject.transform.position;
                }
                else
                {
                    Vector3 newdiff = diff + new Vector3(0, (-1) * diff.y, (-1) * diff.z) * 2;
                    room.housePosition = newdiff + door.gameObject.transform.position;
                }
                //otherDoor.transform.position
                //room.houseposition..
                break;
            default:
                break;
        }
    }
    */
}
