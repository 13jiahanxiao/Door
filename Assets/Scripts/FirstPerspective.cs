using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FirstPerspective : MonoBehaviour
{ //定义枚举数据结构，将名称和设置关联起来
    public enum RotationAxes
    {
        MouseXAndY = 0, MouseX = 1, MouseY = 2
    }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityHor = 9.0f;//水平旋转的速度
    public float sensitivityVert = 9.0f;//垂直旋转的速度
    public float minimumVert = -45.0f;//垂直旋转的最小角度
    public float maximumVert = 45.0f;//垂直旋转的最小角度
    private float _rotationX = 0;//为垂直角度声明一个私有变量
    private GameObject player;
    private Slider slider;
    //private float camY;
    void Start()
    { //将光标锁定到游戏窗口的中心。
        slider = FindObjectOfType<Slider>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player = GameObject.FindGameObjectWithTag("Player");
        sensitivityHor = slider.GetComponent<Slider>().value;
        sensitivityVert = slider.GetComponent<Slider>().value;
        //camY = this.transform.position.y - player.transform.position.y;
    }
    void Update()
    {
        //this.transform.position = new Vector3(0, camY, 0) + player.transform.position;
        if (axes == RotationAxes.MouseX)
        { //水平旋转代码
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
        }
        else if (axes == RotationAxes.MouseY)
        { //垂直旋转代码
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
            float rotationY = transform.localEulerAngles.y;//保持y轴与原来一样
            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }
        else
        { //水平且垂直旋转
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);//限制角度大小
            float delta = Input.GetAxis("Mouse X") * sensitivityHor;//设置水平旋转的变化量
            float rotationY = transform.localEulerAngles.y + delta;//原来的角度加上变化量
            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);//相对于全局坐标空间的角度
        } 
        //当点击esc后
       /*if (Input.GetKeyDown("escape"))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //Cursor.lockState = CursorLockMode.Confined;
            //将光标限制在游戏窗口。
        }
        if (Input.GetButtonDown("Fire1"))
        {
           Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        */
    }
}
