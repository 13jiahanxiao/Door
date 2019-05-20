using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Rigidbody player;
    Vector3 camAngle;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Rigidbody>();
        camAngle = player.transform.eulerAngles;
    }
    private void Update()
    {
        AngleChange();
        PositionChange();       
    }

    void AngleChange()
    {
        float y = Input.GetAxis("Mouse X");
        float x = Input.GetAxis("Mouse Y");
        camAngle.x -= x;
        camAngle.y += y;

        player.transform.eulerAngles = camAngle;
    }
    void PositionChange()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        player.transform.Translate(new Vector3(h, 0, v) * 0.1f, Space.Self);
    }
}
