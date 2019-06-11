using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    #region Instance
    private static UIManager _Instance;
    public static UIManager Instance
    {
        get { return _Instance; }
    }
    #endregion
    public GameObject escUI;
    private bool uiActive;
    public FirstPerspective fp;
    public List<Image> crayonIcons;
    private Canvas canvas;
    public Vector3 firstIconPos;
    public float iconGap;

    void Awake()
    {
        _Instance = this;
    }

    void Start () {
        canvas = FindObjectOfType<Canvas>();
        uiActive = false;
        escUI.SetActive(false);
	}
	
	void Update ()
    {
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
    public void changeCrayon(int index)  //参数index：当前蜡笔的下标
    {
        for(int i=0;i<GameManager.Instance.crayonList.Count; i++)
        {
            crayonIcons[(index + i )% GameManager.Instance.crayonList.Count].transform.DOMove(firstIconPos + new Vector3(i * iconGap, 0, 0), 0.5f);
        }
    }
    public void iconInitiate()
    {
        for(int i=0;i<GameManager.Instance.crayonList.Count;i++)
        {
            Image icon=Instantiate(Resources.Load<Image>("Icon"),canvas.transform);
            crayonIcons.Add(icon);
            icon.rectTransform.position = firstIconPos + new Vector3(i * iconGap, 0, 0);
            icon.rectTransform.sizeDelta = new Vector2(19.05f, 160.55f);
            icon.sprite = Resources.Load<Sprite>("CrayonIcon/" + GameManager.Instance.crayonList[i].color.ToString());
        }
    }
}
