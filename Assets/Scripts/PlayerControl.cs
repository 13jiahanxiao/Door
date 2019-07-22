using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
    public float jumpForce;
    public float rotateSpeed;
    public bool onGround;
    private Vector3 defaultGravityDirection = new Vector3(0, -1, 0);
    private Vector3 setedGravityDirection;
    public float gravity = 1;
    private Transform t;
    public bool rotating;
    public int model;
    public float blueMoveSpeed = 2.6f;
    private int isCollider = 0;

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
        rotation = this.transform.rotation;
        setedGravityDirection = defaultGravityDirection;
        playerRB = this.gameObject.GetComponent<Rigidbody>();
        rotating = false;
        model = 0;
        isCollider = 0;
    }

    private void Update()
    {

        if (model !=0)
        {
            //Debug.Log(isCollider);
            if (isCollider == 0)
            {
                this.transform.Translate(setedGravityDirection * blueMoveSpeed * Time.deltaTime, Space.World);
                
            }
            else
            {
                GetComponent<Rigidbody>().velocity = new Vector3();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                model = 0;
                UIManager.Instance.OutBlueText.SetActive(false);
               // trigger.gameObject.SetActive(false);
               // setedGravityDirection = new Vector3(0, -1, 0);
                GameManager.Instance.currentBlueArea = null;
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }
        }
        else
        {
            playerRB.AddForce(defaultGravityDirection * (playerRB.mass * gravity));
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (onGround && playerRB.velocity.y >= -0.1)
                {
                   // playerRB.AddForce(new Vector3(0, 1, 0) * jumpForce,ForceMode.Impulse);
                    playerRB.velocity = new Vector3(playerRB.velocity.x, jumpSpeed, playerRB.velocity.z);
                    onGround = false;
                }
            }
            camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            if (new Vector2(playerRB.velocity.x, playerRB.velocity.z).magnitude <= new Vector2(velocity, velocity).magnitude+0.1f)
            {
                playerRB.velocity = v * camForward * velocity + h * cam.right * velocity + new Vector3(0, playerRB.velocity.y, 0);
            }
        }
        transform.rotation = rotation;

    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "BlueArea")
        {
            UIManager.Instance.OutBlueText.SetActive(true);
            if (model >0)
            {
                model = 2;
                //trigger.gameObject.SetActive(false);
            }
            else
            {
                model = 1;
            }
            GameManager.Instance.currentBlueArea = collider.transform;
            setedGravityDirection = GameManager.Instance.currentBlueArea.up;
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            if (Mathf.Abs(setedGravityDirection.x) > 0.5f)
            {
                transform.DOMove(new Vector3(this.transform.position.x, collider.transform.position.y, collider.transform.position.z) + setedGravityDirection*0.2f, 0.5f);
            }
            if (Mathf.Abs(setedGravityDirection.y) > 0.5f)
            {
                transform.DOMove(new Vector3(collider.transform.position.x, this.transform.position.y, collider.transform.position.z) + setedGravityDirection*0.2f, 0.5f);
            }
            if (Mathf.Abs(setedGravityDirection.z) > 0.5f)
            {
                transform.DOMove(new Vector3(collider.transform.position.x, collider.transform.position.y, this.transform.position.z) + setedGravityDirection*0.2f, 0.5f);
            }
            isCollider = 0;
        }
    }
    /*
    void OnTriggerExit(Collider collider)
    {
        if (collider.name == "BlueArea")
        {
            if (collider.transform == GameManager.Instance.currentBlueArea)
            {
                model = 0;
            }
        }
    }
    */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag !="Item")
        {
            isCollider = 1;
        }
    }
}
