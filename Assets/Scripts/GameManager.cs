using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //包含各脚本中的变量 存储画笔 画笔切换模块
    public enum DoorColor
    {
        red, green, purple,yellow
    }
    public GameObject player;
    public GameObject camera;
    public GameObject room1, room2;
    public GameObject previousParent;//玩家去过的上一个房间parent
    public GameObject currentParent; //当前房间的parent
    public List<Brush> brush;//所有画笔
    public Brush currentBrush;//当前画笔
    public Draw draw;
    public enum currentRoom
    { room0,room1,room2}
    public currentRoom cr;
    public Vector3 moveDistace; //房间长宽高 确定移动的距离
    void Awake()
    {
        currentBrush = new Brush
        {
            num = 1
        };
        //测试用
        player = GameObject.FindObjectOfType<PlayerControl>().gameObject;
        camera = Camera.main.gameObject;
    }
    void Update()
    {

    }

}
