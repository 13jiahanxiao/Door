using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    private Quaternion rotation;
    private Rigidbody playerRB;
    private Transform cam;
    private Vector3 camForward;
    public float velocity;
    private float h, v;
    public float jumpSpeed;
    [HideInInspector] public bool onGround;
    public float gravity = 1;
	private void Start ()
    {
        if(Camera.main!=null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                   "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
        }
        rotation = this.transform.rotation;
        playerRB = this.gameObject.GetComponent<Rigidbody>();
	}

    private void Update()
    {
        playerRB.AddForce(0, -playerRB.mass*gravity, 0);
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        this.transform.rotation = rotation;
        if(cam!=null)
        {
            camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            playerRB.velocity = v * camForward * velocity + h * cam.right * velocity + new Vector3(0, playerRB.velocity.y,0);
        }
        else
        {
            playerRB.velocity = new Vector3(h * velocity, playerRB.velocity.y, v * velocity);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround)
            {
                playerRB.velocity = new Vector3(playerRB.velocity.x, jumpSpeed, playerRB.velocity.z);
                onGround = false;
        }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Door")
        {
            if(GameManager.Instance.currentRoom.transform!=other.GetComponentInParent<Room>().transform)//两者若相等 则意味着更新前的currentRoom就是门所在的room，即角色没有去另一个房间
            {
                GameManager.Instance.lastRoom = GameManager.Instance.currentRoom;       //将上一个currentRoom更新为lastRoom
                GameManager.Instance.currentRoom = other.transform.parent.GetComponent<Room>();  //更新currentRoom
            }
            other.GetComponent<Door>().targetDoor.transform.parent.gameObject.SetActive(true);
            Room targetRoom = other.GetComponent<Door>().targetDoor.GetComponentInParent<Room>();
            GameManager.Instance.RefreshRoom(targetRoom);
            // other.transform.parent.gameObject.SetActive(true);
            //GameManager.Instance.RefreshRoom(room);

            //Vector3 position= other.GetComponent<Door>().targetDoor.position;
            //position.x = position.x + 1;
            //this.transform.position = position;

        }
    }
}
