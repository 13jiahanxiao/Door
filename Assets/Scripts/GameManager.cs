using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject camera;
    public GameObject room1, room2;
    public enum currentRoom
    { room0,room1,room2}
    public currentRoom cr;
    public Vector3 moveDistace; //房间长宽高 确定移动的距离
    void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerControl>().gameObject;
        camera = Camera.main.gameObject;
    }
    void Update()
    {

    }

}
