using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    void Start()
    {

    }
    void OnTriggerEnter(Collider collider)
    {
        transform.parent.GetComponent<PlayerControl>().onGround = true;
    }
}
