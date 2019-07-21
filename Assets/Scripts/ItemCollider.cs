using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemCollider : MonoBehaviour
{
    int model;
    Vector3 setedGravityDirection;
    Transform currentBlue;
    public int isCollider = 0;
    void Start()
    {
        model = 0;
        isCollider = 0;
    }
    void Update()
    {
        if (model != 0)
        {
            GetComponent<Rigidbody>().useGravity = false;
            if (isCollider == 0)
            {
                this.transform.Translate(setedGravityDirection * 2.6f * Time.deltaTime, Space.World);
            }
            else
            {
                GetComponent<Rigidbody>().velocity = new Vector3();
            }
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
                transform.parent.DOMove(new Vector3(this.transform.parent.position.x, collider.transform.position.y , collider.transform.position.z ) + setedGravityDirection*0.2f, 0.5f);
            }
            if (Mathf.Abs(setedGravityDirection.y) > 0.5f)
            {
                transform.parent.DOMove(new Vector3(collider.transform.position.x, this.transform.parent.position.y, collider.transform.position.z ) + setedGravityDirection*0.2f, 0.5f);
            }
            if (Mathf.Abs(setedGravityDirection.z) > 0.5f)
            {
                transform.parent.DOMove(new Vector3(collider.transform.position.x , collider.transform.position.y , this.transform.parent.position.z) + setedGravityDirection*0.2f, 0.5f);
            }
            isCollider = 0;
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Player")
        {
            isCollider = 1;
        }
    }
}


