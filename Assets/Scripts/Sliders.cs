using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour {
    public FirstPerspective fp;
    private Slider slider;
	void Start () {
        fp = FindObjectOfType<FirstPerspective>();
        slider = this.GetComponent<Slider>();
    }
    public void changeSensitivity()
    {
        fp.sensitivityHor = slider.value;
        fp.sensitivityVert =  slider.value;
    }
    public void changeVolume()
    {

    }
}
