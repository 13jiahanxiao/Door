using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//已将碰撞检测移至PlayJump脚本中 以避免子物体重复触发碰撞的问题
public class PlayerControl : MonoBehaviour
{
    private Quaternion rotation;
    private Rigidbody playerRB;
    private Transform cam;
    private Vector3 camForward;
    public float velocity;
    private float h, v;
    public float jumpSpeed;
    public float rotateSpeed;
    public bool onGround;
    public Vector3 defaultGravityDirection = new Vector3(0, -1, 0);
    public Vector3 setedGravityDirection;
    public Vector3 targetGravityDirection;
    public float gravity = 1;
    private Transform t;
    public bool rotating;
    private void Start()
    {
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                   "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
        }
        //rotation = this.transform.rotation;
        setedGravityDirection = defaultGravityDirection;
        playerRB = this.gameObject.GetComponent<Rigidbody>();
        rotating = false;
    }

    private void Update()
    {
        playerRB.AddForce(setedGravityDirection * (playerRB.mass * gravity));
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        transform.rotation = Quaternion.FromToRotation(Vector3.up*(-1), setedGravityDirection);
       // this.transform.rotation = rotation;
        if (cam != null)
        {
            camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            if (new Vector2(playerRB.velocity.x, playerRB.velocity.z).magnitude <= new Vector2(velocity, velocity).magnitude)
            {
                playerRB.velocity = v * camForward * velocity + h * cam.right * velocity + new Vector3(0, playerRB.velocity.y, 0);
            }
        }
        else
        {
            playerRB.velocity = new Vector3(h * velocity, playerRB.velocity.y, v * velocity);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround && playerRB.velocity.y >= -0.1)
            {
                playerRB.velocity = new Vector3(playerRB.velocity.x, jumpSpeed, playerRB.velocity.z);
                onGround = false;
            }
        }
        if(Input.GetKey(KeyCode.M))
        {
            setedGravityDirection = Quaternion.AngleAxis(rotateSpeed * Time.deltaTime, Vector3.Cross(setedGravityDirection, new Vector3(-1, 0, 0))) * setedGravityDirection;
            //if (this.transform.up.x > -0.999)
            {
            //    blueRotate(GameManager.Instance.setedGravityDirection, new Vector3(-1, 0, 0), rotateSpeed);
            }
        }
    }
    public void blueRotate(Vector3 startUp, Vector3 endUp, float speed)
    {
        rotating= true;
        Debug.Log("!");
        t = this.transform;
        t.Rotate(Vector3.Cross(startUp, endUp), Vector3.Angle(startUp, endUp)*speed*Time.deltaTime, Space.Self);
        this.transform.rotation = t.rotation;
    }
}
