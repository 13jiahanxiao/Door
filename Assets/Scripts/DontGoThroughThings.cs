
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontGoThroughThings : MonoBehaviour
{
    public bool sendTriggerMessage = false;
    public LayerMask layerMask = -1;
    public float skinWidth = 0.1f;

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    private Rigidbody myRigidbody;
    private Collider myCollider;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        previousPosition = myRigidbody.position;
        minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;
    }

    void Update()
    {
        Vector3 movementThisStep = myRigidbody.position - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;
        float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
        RaycastHit[] hit;
        hit = Physics.RaycastAll(previousPosition, movementThisStep, movementMagnitude, layerMask.value);

        //if (movementSqrMagnitude > sqrMinimumExtent)
        {
            if (hit != null)
            {
                //if(hit.Length!=0)
                // Debug.Log(hit.Length);
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].collider.tag == "Door")
                    {
                        transform.Find("Sphere").GetComponent<PlayerCollision>().DoorTrigger(hit[i].collider);
                    }
                    else if(hit[i].collider.name=="BlueArea")
                    {
                        GameManager.Instance.currentBlueArea = hit[i].transform;
                        GetComponent<PlayerControl>().setedGravityDirection = GameManager.Instance.currentBlueArea.up;
                        GameManager.Instance.player.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                    }
                }

                if (GameManager.Instance.canMove)
                {
                    for (int i = 0; i < hit.Length; i++)
                    {
                        if (!hit[i].collider.isTrigger)
                        {
                            Debug.Log("move");
                            transform.position = hit[i].point - (movementThisStep / movementMagnitude) * partialExtent;
                            //Debug.Log("位置重置"+Time.time);
                            break;
                        }
                    }
                }

            }
        }
        // else
        {
            //  if(hit!=null)
            {
                //   for (int i = 0; i < hit.Length; i++)
                {
                    //     if (hit[i].collider.isTrigger)
                    {
                        //   if (hit[i].collider.tag == "Door")
                        {
                            //         transform.Find("Sphere").GetComponent<PlayerCollision>().ooo(hit[i].collider);
                        }
                    }
                }
            }
        }
        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            previousPosition =myRigidbody.position;
        }
    }
    /*
    void LateUpdate()
    {
        Vector3 movementThisStep = myRigidbody.position - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;
        float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
        RaycastHit[] hit;
        hit = Physics.RaycastAll(previousPosition, movementThisStep, movementMagnitude, layerMask.value);
        for (int i = 0; i < hit.Length; i++)
        {
            if (!hit[i].collider.isTrigger)//if (!hitTrigger)
            {
                transform.position = hit[i].point - (movementThisStep / movementMagnitude) * partialExtent;
                //Debug.Log("位置重置"+Time.time);
                break;
            }

        }
        previousPosition = myRigidbody.position;
    }*/
}