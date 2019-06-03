﻿using System.Collections;
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
    #endregion
    public Room currentRoom;//现在的房间 其中的house则为currentHouse
    public DoorColor lastBuff;
    //public houseNumber currentHouse;//初始为0 每次画门先判断该值是否为0
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
    public GameObject[] houseObject = new GameObject[3];//场景中房间
    public enum houseNumber
    {
        House0,House1, House2
    }
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

        player = GameObject.FindObjectOfType<PlayerControl>().gameObject;
        playerCamera = Camera.main.gameObject;
    }
    void Update()
    {
        #region MouseScroll
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentCrayon <crayonList.Count-1)
                currentCrayon++;
            else 
                currentCrayon = 0;
        }
        if(Input.GetAxis("Mouse ScrollWheel")<0)
        {
            if (currentCrayon >= 1)
                currentCrayon--;
            else if (currentCrayon < 1)
                currentCrayon = crayonList.Count - 1;
        }
        #endregion
    }

    public string CrayonColorName()
    {
         return Enum.GetName(crayonList[currentCrayon].color.GetType(), crayonList[currentCrayon].color);
    }
    
    //public houseNumber currentHouse;

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
    public void RefreshRoom(Room room)//参数为需要更新的目标房间
    {
        //Debug.Log((int)room.house);
       // Debug.Log((int)currentRoom.house);
        houseObject[(int)room.house].transform.position = room.housePosition;
        houseObject[(int)room.house].transform.eulerAngles = room.houseRotationEular;
        //让第三者离远点避免重叠
        houseObject[3 - (int)room.house - (int)currentRoom.house].transform.position = room.housePosition + new Vector3(0, 100, 0);
    }//刷新房间
    
}
