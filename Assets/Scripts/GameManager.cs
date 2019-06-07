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
    public Text text;
    public Room currentRoom;//现在的房间 其中的house则为currentHouse
    public Room lastRoom;
    public GameObject player;
    [HideInInspector]public GameObject playerCamera;
    public enum DoorColor
    {
        RED = 0, GREEN, PURPLE, YELLOW,WHITE

    }
    //public Crayon[] crayonArray = new Crayon[4];
    public List<Crayon> crayonList = new List<Crayon>();//蜡笔类用来存储蜡笔数量，颜色
    public DoorColor[] crayonColorArray;
    public int[] crayonNumArray;
    public int currentCrayon = 0;//当前蜡笔
    public GameObject[] houseObject = new GameObject[2];//场景中房间
    public enum houseNumber
    {
        House0,House1
    }
    public bool onMiddle;  //若为true,则玩家处在两个房间的中间状态
    private houseNumber nextNumber;
    void Awake()
    {
        //isStart = true;
        _Instance = this;
    }
    void Start()
    {
        if(crayonNumArray.Length!=crayonColorArray.Length)
        {
            Debug.LogWarning("未同步蜡笔颜色和数量");
        }
        for(int i = 0; i <crayonNumArray.Length ; i++) //每关的颜色和数量在inspector面板中指定
        {
            Crayon cra = new Crayon(crayonNumArray[i], crayonColorArray[i]);
            crayonList.Add(cra);
        }
        onMiddle = false;
        player = GameObject.FindObjectOfType<PlayerControl>().gameObject;
        playerCamera = Camera.main.gameObject;
    }
    void Update()
    {
        #region MouseScroll
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentCrayon < crayonList.Count - 1)
            {
                currentCrayon++;
            }
            else
            {
                currentCrayon = 0;
            }
            UIManager.Instance.changeCrayon(currentCrayon);
        }
        if(Input.GetAxis("Mouse ScrollWheel")<0)
        {
            if (currentCrayon >= 1)
            {
                currentCrayon--;
            }
            else
            {
                currentCrayon = crayonList.Count - 1;
            }
            UIManager.Instance.changeCrayon(currentCrayon);
        }
        #endregion
    }

    public string CrayonColorName()
    {
         return Enum.GetName(crayonList[currentCrayon].color.GetType(), crayonList[currentCrayon].color);
    }
    
    public void RefreshRoom(Room room)//参数为需要更新的目标房间
    {
        //room.gameObject.SetActive(true);//开启对面房间的roomManager
        if (lastRoom != null)
        {
            lastRoom.gameObject.SetActive(false); //关闭lastRoom的roomManager
            for (int i = 0; i < lastRoom.hideIndex.Count; i++)
            {
                Instance.houseObject[(int)lastRoom.house].transform.GetChild(lastRoom.hideIndex[i][0]).GetChild(lastRoom.hideIndex[i][1]).gameObject.SetActive(true);
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
    public void setText(String s)
    {
        text.text = s;
    }
    public void houseChange(DoorColor color, Room room, Door door)
    {
        //利用door的位置计算房间位置和旋转,然后将position等属性赋给room
        switch (color)
        {
            case DoorColor.PURPLE:
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
            default:
                break;
        }
    }
    /*
public GameObject CreateRoom(Vector3 pos ,Quaternion rot)
{
    GameObject room = new GameObject("RoomManager");
    room.AddComponent<Room>();
    room.GetComponent<Room>().roomPosition = pos;
    room.GetComponent<Room>().roomRotation = rot;
    //currentRoom = room.GetComponent<Room>();
    return room;
}//创建房间时使Room类挂在room（doorparent）物体上，Room包括room物体，房间位置，旋转，门个数，房间编号
*/
}
