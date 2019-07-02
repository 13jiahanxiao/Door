using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    #region Instance
    private static GameManager _Instance;
    public static GameManager Instance
    {
        get { return _Instance; }
    }
    #endregion

    #region 枚举类型
    public enum DoorColor
    {
        RED = 0, GREEN, PURPLE, YELLOW, WHITE, BLACK

    }
    public enum houseNumber
    {
        House0, House1
    }
    #endregion

    #region 房间变量
    public GameObject[] houseObject = new GameObject[2];//场景中房间
    public Room startRoom;
    public Room currentRoom;//现在的房间 其中的house则为currentHouse
    public Room lastRoom;
    private houseNumber nextNumber;
    #endregion

    #region 蜡笔相关变量
    public int[] crayonNumArray;//每关蜡笔个数
    public DoorColor[] crayonColorArray;//每关蜡笔颜色
    public List<Crayon> crayonList = new List<Crayon>();//蜡笔类用来存储蜡笔数量，颜色
    public int currentCrayon = 0;//当前蜡笔
    #endregion

    public bool whiteExist;//白门是否存在
    public bool canMove; 

    public int[][] hide;

    public bool onMiddle;  //若为true,则玩家处在两个房间的中间状态

    public GameObject player;
    [HideInInspector] public GameObject playerCamera;

    public Door blackDoor;//黑门
    public float wallThickness;


    void Awake()
    {
        _Instance = this;
    }
    void Start()
    {
        #region 变量赋值
        houseObject[1] = GameObject.Instantiate(houseObject[0], new Vector3(0, 30, 0), new Quaternion(0, 0, 0, 0));
        player = GameObject.FindObjectOfType<PlayerControl>().gameObject;
        playerCamera = Camera.main.gameObject;
        whiteExist = false;
        canMove = true;
        onMiddle = false;
        hide = new int[2][];
#endregion
        startRoomInitiate();
        CrayonNumColorInitiate();
        UIManager.Instance.iconInitiate();
    }
    void Update()
    {
        MouseScroll(); 
    }

    void startRoomInitiate()
    {
        currentRoom = startRoom;
        startRoom.housePosition = new Vector3(0, 0, 0);
        startRoom.houseRotationEular = new Vector3(0, 0, 0);
        startRoom.house = houseNumber.House0;
    }
    void CrayonNumColorInitiate()
    {
        if (crayonNumArray.Length != crayonColorArray.Length)
        {
            Debug.LogWarning("未同步蜡笔颜色和数量");
        }
        for (int i = 0; i < crayonNumArray.Length; i++) //每关的颜色和数量在inspector面板中指定
        {
            Crayon cra = new Crayon(crayonNumArray[i], crayonColorArray[i]);
            crayonList.Add(cra);
        }
    }

    public string CrayonColorName()
    {
         return Enum.GetName(crayonList[currentCrayon].color.GetType(), crayonList[currentCrayon].color);
    }//返回颜色名
    
    public void RefreshRoom(Room room)//参数为需要更新的目标房间
    {
        if (lastRoom != null)
        {
            if (lastRoom.transform != currentRoom.transform)
            {
                for (int i = 0; i < lastRoom.hideIndex.Count; i++)
                {
                    houseObject[(int)lastRoom.house].transform.GetChild(lastRoom.hideIndex[i][0]).GetChild(lastRoom.hideIndex[i][1]).gameObject.SetActive(true);
                }
            }
        }
        for (int i = 0; i < room.hideIndex.Count; i++)
        {
            Instance.houseObject[(int)room.house].transform.GetChild(room.hideIndex[i][0]).GetChild(room.hideIndex[i][1]).gameObject.SetActive(false);
        }
        houseObject[(int)room.house].transform.position = room.housePosition;
        houseObject[(int)room.house].transform.eulerAngles = room.houseRotationEular;

        // houseObject[3 - (int)room.house - (int)currentRoom.house].transform.position = room.housePosition + new Vector3(0, 100, 0);
    }//刷新房间
    public void targetHouseCalculate(DoorColor color, Room room, Door door)
    {
        //利用door的位置计算房间位置和旋转,然后将position等属性赋给room
        switch (color)
        {
            case DoorColor.RED:
                room.houseRotationEular = currentRoom.houseRotationEular + new Vector3(180, 0, 0);// + new Vector3(0, 180 * door.transform.up.x, 0);
                Vector3 diff = houseObject[(int)currentRoom.house].transform.position - door.targetDoor.gameObject.transform.position;
                if (Vector3.Dot(door.transform.up, new Vector3(1, 0, 0)) > 0.1 || Vector3.Dot(door.transform.up, new Vector3(1, 0, 0)) < -0.1)
                {
                    room.houseRotationEular += new Vector3(0, 180, 0);
                    Vector3 newdiff = diff + new Vector3((-1) * diff.x, (-1) * diff.y, 0) * 2;
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
            case DoorColor.WHITE:
                room.housePosition = new Vector3(0,0,0);
                room.houseRotationEular = new Vector3(0, 0, 0);
                //player.transform.position=
                break;
            default:
                break;
        }
    }

    public void whiteDoorCalculate(Door door,Material color, RaycastHit hit)
    {
        if (door.color == DoorColor.WHITE)
        {
            hide[0] = new int[2] { hit.transform.parent.GetSiblingIndex(), hit.transform.GetSiblingIndex() };
        }
        else
        {
            hide[1] = new int[2] { hit.transform.parent.GetSiblingIndex(), hit.transform.GetSiblingIndex() };
        }
        GameManager.Instance.startRoom.hideIndex.Add(new int[2] { hit.transform.parent.GetSiblingIndex(), hit.transform.GetSiblingIndex() });
        GameObject otherDoor = Instantiate<GameObject>(Resources.Load<GameObject>("Door"), GameManager.Instance.startRoom.transform);
        otherDoor.GetComponent<Collider>().isTrigger = false;
        otherDoor.tag = "Untagged";
        otherDoor.GetComponent<Door>().color = GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].color;

        if (otherDoor.GetComponent<Door>().color==DoorColor.BLACK)
        {
            blackDoor = otherDoor.GetComponent<Door>();
        }
        //otherDoor.transform.eulerAngles = door.transform.eulerAngles - currentRoom.houseRotationEular;
        //otherDoor.transform.Rotate(GameManager.Instance.currentRoom.houseRotationEular, Space.Self);
        ConnectDoor(door.gameObject, otherDoor);
        //Vector3 diff = houseObject[(int)currentRoom.house].transform.position - door.gameObject.transform.position;
        if (Vector3.Dot(door.transform.up, new Vector3(1, 0, 0)) > 0.1 || Vector3.Dot(door.transform.up, new Vector3(1, 0, 0)) < -0.1)
        {
            otherDoor.transform.eulerAngles = door.transform.eulerAngles - currentRoom.houseRotationEular;
            //Vector3 newdiff = diff + new Vector3((-1) * diff.x, (-1) * diff.y, 0) * 2;
            // otherDoor.transform.position = startRoom.housePosition - newdiff;
        }
        else
        {
            otherDoor.transform.eulerAngles = door.transform.eulerAngles - currentRoom.houseRotationEular + new Vector3(0, 180, 0);
            //Vector3 newdiff = diff + new Vector3(0, (-1) * diff.y, (-1) * diff.z) * 2;
            //otherDoor.transform.position = startRoom.housePosition - newdiff;
        }
        
        //otherDoor.transform.position = houseObject[(int)startRoom.house].transform.GetChild(hide[0]).GetChild(hide[1]).position +
        //    otherDoor.transform.up * wallThickness/2;
        
        otherDoor.GetComponent<Renderer>().material = color;
    }

    public void ConnectDoor(GameObject door1, GameObject door2)
    {
        door1.GetComponent<Door>().position = door1.transform.position;
        door2.GetComponent<Door>().position = door2.transform.position;
        door1.GetComponent<Door>().targetDoor = door2.GetComponent<Door>();
        door2.GetComponent<Door>().targetDoor = door1.GetComponent<Door>();
    }

    public void MouseScroll()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentCrayon < crayonList.Count - 1)
            {
                currentCrayon++;
            }
            else
            {
                currentCrayon = 0;
            }
            UIManager.Instance.introduce(crayonList[currentCrayon].color);
            UIManager.Instance.updateNum();
            UIManager.Instance.changeCrayon(currentCrayon);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentCrayon >= 1)
            {
                currentCrayon--;
            }
            else
            {
                currentCrayon = crayonList.Count - 1;
            }
            UIManager.Instance.introduce(crayonList[currentCrayon].color);
            UIManager.Instance.updateNum();
            UIManager.Instance.changeCrayon(currentCrayon);
        }
    }//鼠标滑轮上下滚
    
}


