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
            Room room = other.GetComponent<Door>().targetDoor.GetComponentInParent<Room>();
            if (other.GetComponentInParent<Room>().house== GameManager.houseNumber.House1)
            {
                room.house = GameManager.houseNumber.House2;
            }
            else
            {
                room.house = GameManager.houseNumber.House1;
            }
            GameManager.Instance.RefreshRoom(room);

            Vector3 position= other.GetComponent<Door>().targetDoor.position;
            position.x = position.x + 1;
            this.transform.position = position;

            other.transform.parent.gameObject.SetActive(false);
        }
    }
}
