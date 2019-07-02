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
    private Text crayonNum;//文本显示蜡笔数量

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

    [HideInInspector] public Image circle;
    public float fillspeed;
    [HideInInspector] public Image restart;
    [HideInInspector] public Text text;
    public GameObject clear;

    public float appearSpeed;
    public string startText;
    public float slowAppearSpeed;

    void Awake()
    {
        _Instance = this;
    }

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        text = canvas.transform.Find("Text").GetComponent<Text>();
        circle = canvas.transform.Find("Circle").GetComponent<Image>();
        restart = canvas.transform.Find("Restart").GetComponent<Image>();
        crayonNum = GameObject.Find("crayonNum").GetComponent<Text>();
        uiActive = false;
        escUI.SetActive(false);
        setText(startText,slowAppearSpeed);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        if (Input.GetKey(KeyCode.R))
        {
            restart.fillAmount += fillspeed * Time.deltaTime;
            restart.transform.GetChild(0).gameObject.SetActive(true);
            if (restart.fillAmount > 0.999)
            {
                LevelManager.Instance.ReloadScene();
            }
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            restart.fillAmount = 0;
            restart.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    
    public void setText(string s)
    {
        text.text = s;
        StopCoroutine("textAppear");
        StartCoroutine(textAppear(text));
    }
    public void setText(string s,float speed)
    {
        text.text = s;
        StopCoroutine("textAppear");
        StartCoroutine(textAppear(text,speed));
    }
    IEnumerator textAppear(Text t)
    {
        for (t.color = new Color(1,1,1,0); t.color.a<=0.9 ; )
        {
            //Debug.Log(t.color);
            t.color += new Color(0, 0, 0,appearSpeed*Time.deltaTime );
            yield return null;
        }
        for (t.color = new Color(1,1,1,1); t.color.a >=0.1;)
        {
            t.color -= new Color(0, 0, 0, appearSpeed * Time.deltaTime);
            yield return null;
        }
        t.color = new Color(1,1,1,0);
    }
    IEnumerator textAppear(Text t,float speed)
    {
        for (t.color = new Color(1, 1, 1, 0); t.color.a <= 0.9;)
        {
            //Debug.Log(t.color);
            t.color += new Color(0, 0, 0, speed * Time.deltaTime);
            yield return null;
        }
        for (t.color = new Color(1, 1, 1, 1); t.color.a >= 0.1;)
        {
            t.color -= new Color(0, 0, 0, speed * Time.deltaTime);
            yield return null;
        }
        t.color = new Color(1, 1, 1, 0);
    }
    
    public void pickItem(string name)
    {
        if (name == "FC")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            clear.SetActive(true);
        }
        else
        {
            GameObject[] g;
            g = GameObject.FindGameObjectsWithTag("Item");
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
    public void useKey()
    {
        Image t = itemIcons.Find(target =>
         {
             if (target.name == "Key1")
             {
                 return true;
             }
             else return false;
         });
        Destroy(t);
        itemIcons.Remove(t);
    }
    
    public void iconInitiate()
    {
        for (int i = 0; i < GameManager.Instance.crayonList.Count; i++)
        {
            Image icon = Instantiate(Resources.Load<Image>("CrayonIcon"), canvas.transform);
            crayonIcons.Add(icon);
            icon.rectTransform.position = firstIconPos + new Vector3(i * iconGap, 0, 0);
            icon.rectTransform.sizeDelta = new Vector2(19.05f, 160.55f);
            icon.sprite = Resources.Load<Sprite>("CrayonIcon/" + GameManager.Instance.crayonList[i].color.ToString());
            updateNum();
        }
    }//蜡笔图标初始化
    public void updateNum()
    {
        crayonNum.text = "当前颜色蜡笔剩余:" + GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].num;
    }//显示当前蜡笔
    public void changeCrayon(int index)  //改变画笔，参数index：当前蜡笔的下标
    {
        for (int i = 0; i < GameManager.Instance.crayonList.Count; i++)
        {
            crayonIcons[(index + i) % GameManager.Instance.crayonList.Count].transform.DOMove(firstIconPos + new Vector3(i * iconGap, 0, 0), 0.5f);
        }
    }
    public void introduce(GameManager.DoorColor color)
    {

        switch (color)
        {
            case GameManager.DoorColor.RED:
                setText("红门通向一个垂直翻转的房间", 0.5f);
                break;
            case GameManager.DoorColor.WHITE:
                setText("白门通向初始房间", 0.5f);
                break;
            case GameManager.DoorColor.BLACK:
                setText("黑门不可进入，但可以将白门出口的位置设为黑门所在位置", 0.5f);
                break;
            default:
                break;
        }
    }//门的功能介绍
}
