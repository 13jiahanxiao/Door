using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCCollider : MonoBehaviour
{
    void OnTriggerStay(Collider collider)
    {
        if(collider.tag=="Door")
        {
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        }
    }
}

