using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour {
    private Ray ray;
    private RaycastHit hit;
    //private Ray sRay;
    //private RaycastHit sHit;
    public Vector3 screenCenter;
    public float distance = 4.5f;
    private float sDistance;
    public Transform doorParent;
    public GameObject door;
    private GameObjectManager g;
    //public GameObject test;
	void Start ()
    {
        g = GameObject.FindObjectOfType<GameObjectManager>();
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }
	
	void Update ()
    {
        sDistance = distance;
        ray = Camera.main.ScreenPointToRay(screenCenter);
        if(Physics.Raycast(ray, out hit, distance))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            if (Input.GetMouseButtonDown(0))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                if (hit.transform.gameObject.tag == "Box")
                {
                    Debug.Log("拾取物品");
                }
                else if (hit.transform.gameObject.tag == "DoorPosition")
                {
                    Debug.Log("生成门");
                    //Debug.Log(hit.transform.up);
                    //Debug.Log(hit.transform.forward);
                    newDoor();
                }
                else
                {
                    Debug.Log("无效位置");
                }
            }
        }
    }
    void newDoor()
    {
        door = Instantiate(door, doorParent);
        door.transform.position = hit.point;
        bool doorOnGround = Vector3.Angle(g.player.transform.up, hit.transform.up) < 30;
        if (doorOnGround)
        {
            door.transform.eulerAngles = new Vector3(0, g.camera.transform.eulerAngles.y, 0);
        }
        else
        {
            door.transform.rotation = hit.transform.rotation;
        }
    }
}
