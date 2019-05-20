using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensivitySilider : MonoBehaviour {
    public FirstPerspective fp;
    private Slider slider;
	void Start () {
        slider = this.GetComponent<Slider>();
        //fp.sensitivityHor = slider.value;
       // fp.sensitivityVert = slider.value;  //游戏开始时ui关闭 所以无效
        // slider.onValueChanged.AddListener(delegate { changeSensitivity(); });
    }
	
	void Update () {
		
	}
    public void changeSensitivity()
    {
        fp.sensitivityHor = slider.value;
        fp.sensitivityVert =  slider.value;
    }
}
