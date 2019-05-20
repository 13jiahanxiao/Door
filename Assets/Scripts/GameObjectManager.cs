using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    public GameObject player;
    public GameObject camera;
    public Vector3 f;
    void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerControl>().gameObject;
        camera = Camera.main.gameObject;
    }
    void Update()
    {
        f = camera.transform.forward;
        //Debug.Log(Vector3.Angle(camera.transform.forward, new Vector3(0, 0, 1)));
        //Debug.Log(camera.transform.forward);
    }

}
