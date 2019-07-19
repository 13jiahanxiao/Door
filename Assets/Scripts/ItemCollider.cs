using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemCollider : MonoBehaviour
{
    int model;
    Vector3 setedGravityDirection;
    Transform currentBlue;
    void Update()
    {
        if (model != 0)
        {
            GetComponent<Rigidbody>().useGravity = false;
            this.transform.Translate(setedGravityDirection * 2.6f * Time.deltaTime, Space.World);
        }
        else
        {
            GetComponent<Rigidbody>().useGravity = true;
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Door")
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().isTrigger = true;
        }
        else if (collider.name == "BlueArea")
        {
            if (model > 0)
            {
                model = 2;
            }
            else
            {
                model = 1;
            }
            currentBlue = collider.transform;
            setedGravityDirection = currentBlue.up;
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            if (Mathf.Abs(setedGravityDirection.x) > 0.5f)
            {
                transform.DOMove(new Vector3(this.transform.position.x, collider.transform.position.y+transform.right.y, collider.transform.position.z + transform.right.z), 0.5f);
            }
            if (Mathf.Abs(setedGravityDirection.y) > 0.5f)
            {
                transform.DOMove(new Vector3(collider.transform.position.x + transform.right.x , this.transform.position.y, collider.transform.position.z + transform.right.z ), 0.5f);
            }
            if (Mathf.Abs(setedGravityDirection.z) > 0.5f)
            {
                transform.DOMove(new Vector3(collider.transform.position.x + transform.right.x, collider.transform.position.y + transform.right.y , this.transform.position.z), 0.5f);
            }
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.name == "BlueArea")
        {
            if (collider.transform == currentBlue)
            {
                model = 0;
            }
        }
    }
}


