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
    public List<Image> itemIcons;
    private Canvas canvas;
    public Vector3 firstIconPos;
    public Vector3 firstItemPos;
    public float iconGap;
    public float iconGapy;
    public Image circle;
    public float fillspeed;

    void Awake()
    {
        _Instance = this;
    }

    void Start () {
        canvas = FindObjectOfType<Canvas>();
        circle = canvas.transform.Find("Circle").GetComponent<Image>();
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
            Image icon=Instantiate(Resources.Load<Image>("CrayonIcon"),canvas.transform);
            crayonIcons.Add(icon);
            icon.rectTransform.position = firstIconPos + new Vector3(i * iconGap, 0, 0);
            icon.rectTransform.sizeDelta = new Vector2(19.05f, 160.55f);
            icon.sprite = Resources.Load<Sprite>("CrayonIcon/" + GameManager.Instance.crayonList[i].color.ToString());
        }
    }
    public void pickItem(string name)
    {
        GameObject[] g;
        g=GameObject.FindGameObjectsWithTag("Item");
        for (int i = 0; i < g.Length; i++)
        {
            if (g[i].name == name)
            {
                Destroy(g[i]);
            }
        }
        Image icon = Instantiate(Resources.Load<Image>("ItemIcon"), canvas.transform);
        icon.name = name;
        icon.rectTransform.position = firstItemPos + new Vector3(0, itemIcons.Count * iconGapy, 0);
        itemIcons.Add(icon);
        icon.sprite = Resources.Load<Sprite>("ItemIcon/" + name);
    }
}
