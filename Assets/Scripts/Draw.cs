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
    private GameManager g;
    //public GameObject test;
	void Start ()
    {
        g = GameObject.FindObjectOfType<GameManager>();
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
                    g.currentBrush.paint(hit);
                }
                else
                {
                    Debug.Log("无效位置");
                }
            }
        }
    }
}
