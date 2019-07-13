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
    public bool hitTrigger;
    
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
        hitTrigger = false;

        //if (movementSqrMagnitude > sqrMinimumExtent)
        {
            if (hit != null)
            {
                //if(hit.Length!=0)
               // Debug.Log(hit.Length);
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].transform.name != "Sphere")
                    {
                        if (hit[i].collider.isTrigger)
                        {
                            hitTrigger = true;
                            if (hit[i].collider.tag == "Door")
                            {
                                transform.Find("Sphere").GetComponent<PlayerCollision>().ooo(hit[i].collider);
                            }
                        }
                    }
                }
                if (!hitTrigger)
                {
                    for (int i = 0; i < hit.Length; i++)
                    {
                        Debug.Log("ss");
                        if(GameManager.Instance.canMove)//if (!hitTrigger)
                        {
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
        previousPosition = myRigidbody.position;
    }
}