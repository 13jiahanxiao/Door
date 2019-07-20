using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Esc : MonoBehaviour
{
    Button control;
    GameObject setting;
    Text introduce;
    bool isOn = false;

    private void Start()
    {
        control.onClick.AddListener(ControlIntroduce);
    }

    private void ControlIntroduce()
    {
        if (isOn)
        {
            setting.SetActive(true);
            introduce.gameObject.SetActive(false);
            isOn = false;
        }
        else
        {
            setting.SetActive(false);
            introduce.gameObject.SetActive(true);
            isOn = true;
        }
    }
}
