using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//问题：1.先创建门，过门时刷新房间信息，导致们的位置与房间位置不一致
//2.旋转问题未解决
//3.创建门时使墙壁消失，房间重复利用时墙壁回复的功能未写
public class GameManager : MonoBehaviour
{
    #region Instance
    private static GameManager _Instance;
    public static GameManager Instance
    {
        get { return _Instance; }
    }
    void Awake()
    {
        isStart = true;
        _Instance = this;
    }
    #endregion

    public bool isStart = true;//是否为初始房间
    public GameObject player;
    public GameObject playerCamera;

    public enum DoorColor
    {
        RED=0, GREEN, PURPLE, YELLOW
    }
    [HideInInspector] public Crayon [] crayonArray=new Crayon[4];//蜡笔雷用来存储蜡笔数量，颜色
    public int currentCrayon=0;//当前蜡笔

    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            Crayon cra = new Crayon(2, (DoorColor)i);
            crayonArray[i] = cra;//每种颜色两根
        }

        player = GameObject.FindObjectOfType<PlayerControl>().gameObject;
        playerCamera = Camera.main.gameObject;
    }
    void Update()
    {
        #region MouseScroll
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentCrayon <=2)
                currentCrayon++;
            else if(currentCrayon>=3)
                currentCrayon = 0;
        }
        if(Input.GetAxis("Mouse ScrollWheel")<0)
        {
            if (currentCrayon >= 1)
                currentCrayon--;
            else if (currentCrayon < 1)
                currentCrayon = 3;
        }
        #endregion
    }

    public string CrayonColorName()
    {
         return Enum.GetName(crayonArray[currentCrayon].color.GetType(), crayonArray[currentCrayon].color);
    }

    public Room currentRoom;//现在的房间

    public GameObject CreateRoom(Vector3 pos ,Quaternion rot)
    {
        GameObject room = new GameObject("RoomManager");
        room.AddComponent<Room>();
        room.GetComponent<Room>().room = room;
        room.GetComponent<Room>().roomPosition = pos;
        room.GetComponent<Room>().roomRotation = rot;
        currentRoom = room.GetComponent<Room>();
        return room;
    }//创建房间时使Room类挂在room（doorparent）物体上，Room包括room物体，房间位置，旋转，门个数，房间编号

    public Transform[] roomObject = new Transform[2];//场景中房间位置
    public enum houseNumber{
        House1,House2
    }//将房间编为1，2，当前用1时，更新2号房
    public void RefreshRoom(Room room)
    {
        if (room.house == houseNumber.House1)
        {
            roomObject[1].position = room.roomPosition;
            roomObject[1].rotation = room.roomRotation;
        }
        else
        {
            roomObject[0].position = room.roomPosition;
            roomObject[0].rotation = room.roomRotation;
        }
        room.room.SetActive(true);
    }//刷新房间
    
}
