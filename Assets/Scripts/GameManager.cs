using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//包含各脚本中的变量 存储画笔 画笔切换模块
public class GameManager : MonoBehaviour
{
    public enum DoorColor
    {
        red, green, purple,yellow
    }
    public GameObject player;
    public GameObject camera;
    public GameObject room1, room2;
    //public GameObject previousParent;//玩家去过的上一个房间parent,可能要用来判断是否来回于同两个房间
    public DoorColor previousDoorColor;//走过的上一道门的颜色，即之前房间的颜色，用来调用该颜色undo进行转换至此房间状态，以便基于此房间进行一次变化转变成下一房间
    public GameObject currentParent; //当前房间的parent

    [HideInInspector]public List<Brush> brush;//所有画笔
    public List<DoorColor> brushColor;//画笔颜色
    public List<int> brushNum;//画笔数量

    public int currentBrush;//当前画笔在list中的下标
    public Draw draw;
    public enum currentRoom
    { room0, room1, room2 }
    public currentRoom cr;
    public Vector3 moveDistace; //房间长宽高 确定移动的距离
    void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerControl>().gameObject;
        camera = Camera.main.gameObject;
    }
    void Start()
    {
        Debug.Log(brushColor.Count);
        if (brushColor.Count == brushNum.Count)
        {
            for (int i = 0; i < brushColor.Count; i++)
            {
                brush.Add(new Brush { num = brushNum[i], color=brushColor[i] });
                Debug.Log(brushColor[i]);
            }

        }
        else
        {
            Debug.LogWarning("wtf数量不一样");
        }
        currentBrush = 0;
    }
    void Update()
    {
        // Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentBrush < brush.Count-1)
            {
                currentBrush++;
            }
            else
            {
                currentBrush = 0;
            }
        }
        if(Input.GetAxis("Mouse ScrollWheel")<0)
        {
            if (currentBrush > 0)
                currentBrush--;
            else
                currentBrush = brush.Count - 1;
        }
    }

}
