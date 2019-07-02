using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensivitySilider : MonoBehaviour {
    public FirstPerspective fp;
    private Slider slider;
	void Start () {
        slider = this.GetComponent<Slider>();
    }
    public void changeSensitivity()
    {
        fp.sensitivityHor = slider.value;
        fp.sensitivityVert =  slider.value;
    }
}
