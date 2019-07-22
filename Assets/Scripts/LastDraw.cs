using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastDraw : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    public float distance = 4.5f;
    public Vector3 screenCenter;
    public GameObject panel;
    void Start()
    {
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out hit, distance))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(hit.transform.name=="FC")
                {
                    panel.SetActive(true);
                    Invoke("End", 26);
                }
            }
        }
    }
    public void End()
    {
        SceneManager.LoadScene(0);
    }
    
}
