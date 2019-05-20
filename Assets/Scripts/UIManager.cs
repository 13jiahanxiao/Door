using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject escUI;
    private bool uiActive;
    public GameObject slider;
    public FirstPerspective fp;
	void Start () {
        uiActive = false;
        escUI.SetActive(false);
	}
	
	void Update () {
	if(Input.GetKeyDown(KeyCode.Escape))
        {
           // Slider slider = escUI.transform.Find("SensitivitySlider").gameObject.GetComponent<Slider>();
            if (!uiActive) //开启UI
            {
                fp.enabled = false;
                escUI.SetActive(true);
                uiActive = !uiActive;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else //关闭UI
            {
                fp.enabled = true;
                escUI.SetActive(false);
                uiActive = !uiActive;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
	}
}
