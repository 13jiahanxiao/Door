using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontGoThroughThings : MonoBehaviour
{
    // Careful when setting this to true - it might cause double
    // events to be fired - but it won't pass through the trigger
    public bool sendTriggerMessage = false;

    public LayerMask layerMask = -1; //make sure we aren't in this layer 
    public float skinWidth = 0.1f; //probably doesn't need to be changed 

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    private Rigidbody myRigidbody;
    private Collider myCollider;

    //initialize values 
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
        //have we moved more than our minimum extent? 
        Vector3 movementThisStep = myRigidbody.position - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;

        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            // Debug.Log(66);
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            RaycastHit[] hit;
            hit = Physics.RaycastAll(previousPosition, movementThisStep, movementMagnitude, layerMask.value);
            if (hit != null)
            {
                //Debug.Log("33");
                bool hitTrigger = false;
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].collider.isTrigger)
                    {
                        hitTrigger = true;
                        if (hit[i].collider.tag == "Door")
                        {
                            transform.Find("Sphere").GetComponent<PlayerCollision>().OnTriggerEnter(hit[i].collider);
                            break;
                        }
                    }
                }
                if (!hitTrigger)
                {
                    for (int i = 0; i < hit.Length; i++)
                    {

                        //Debug.Log("333");
                        if(GameManager.Instance.canMove)//if (!hitTrigger)
                        {
                            myRigidbody.position = hit[i].point - (movementThisStep / movementMagnitude) * partialExtent;//有bug!!!!!!!!!!!!!
                            Debug.Log(hit[i].point - (movementThisStep / movementMagnitude) * partialExtent);
                            break;
                        }

                    }
                }


            }
            /*
            else
            {
                //Debug.Log("666");
                float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
                RaycastHit hitInfo;
                if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
                {
                    if (hitInfo.collider.isTrigger)
                    {
                        transform.Find("Sphere").SendMessage("ooo", hitInfo.collider);
                    }
                }
            }
            */
            previousPosition = myRigidbody.position;
        }
    }
}