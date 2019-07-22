using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public bool uiActive;
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

    private Slider volumeSlider;
    private Slider sensitivitySlider;
    GameObject control;
    GameObject setting;
    GameObject introduceText;
    bool isOn = false;
    Slider brightness;
    Slider saturation;
    Slider contrast;
    RectTransform handImage;
    public GameObject OutBlueText;
    GameObject returnButton;
    Transform iconParent;

    void Awake()
    {
        _Instance = this;
        crayonNum = GameObject.Find("crayonNum").GetComponent<Text>();
    }

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        text = canvas.transform.Find("Text").GetComponent<Text>();
        circle = canvas.transform.Find("Circle").GetComponent<Image>();
        restart = canvas.transform.Find("Restart").GetComponent<Image>();
        uiActive = false;
        fp = GameObject.FindObjectOfType<Camera>().GetComponent<FirstPerspective>();

        //escUI = GameObject.Find("Canvas/Esc");
        escUI.SetActive(true);

        Transform obt;
        if (obt = canvas.transform.Find("OutBlueText"))
        {
            OutBlueText = obt.gameObject;
        }
        else
        {
            OutBlueText = new GameObject();
            Debug.LogWarning("OutBlueText未赋值！");
        }
        iconParent = canvas.transform.Find("crayonIcons");
        OutBlueText.gameObject.SetActive(false);
        // setText(startText,slowAppearSpeed);
        handImage = escUI.transform.Find("Hand").GetComponent<RectTransform>();
        volumeSlider = escUI.transform.Find("Setting").Find("Volume").Find("VolumeSlider").GetComponent<Slider>();
        sensitivitySlider = escUI.transform.Find("Setting").Find("Sensitivity").Find("SensitivitySlider").GetComponent<Slider>();
        fp.sensitivityHor = sensitivitySlider.value;
        fp.sensitivityVert = sensitivitySlider.value;
        Camera.main.GetComponent<AudioSource>().loop = true;
        Camera.main.GetComponent<AudioSource>().volume = volumeSlider.value;

        control =GameObject.Find("Canvas/Esc/Control");
        control.GetComponent<Button>().onClick.AddListener(ControlIntroduce);

        returnButton = GameObject.Find("Canvas/Esc/BackMain");
        returnButton.GetComponent<Button>().onClick.AddListener(BackMain);

        isOn = false;
        setting=GameObject.Find("Canvas/Esc/Setting");
        introduceText = GameObject.Find("Canvas/Esc/Introduce");
        introduceText.SetActive(false);

        brightness = GameObject.Find("Canvas/Esc/Setting/Brightness").GetComponentInChildren<Slider>();
        brightness.onValueChanged.AddListener((float value) => Bright(value));
        saturation = GameObject.Find("Canvas/Esc/Setting/Saturation").GetComponentInChildren<Slider>();
        saturation.onValueChanged.AddListener((float value) => Bright(value));
        contrast = GameObject.Find("Canvas/Esc/Setting/Contrast").GetComponentInChildren<Slider>();
        contrast.onValueChanged.AddListener((float value) => Bright(value));

        escUI.SetActive(false);
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
                ReloadScene();
            }
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            restart.fillAmount = 0;
            restart.transform.GetChild(0).gameObject.SetActive(false);
        }
        if(uiActive)
        {
            handPosChange(handImage);
        }
    }

    public void changeSensitivity()
    {
        fp.sensitivityHor = sensitivitySlider.value;
        fp.sensitivityVert = sensitivitySlider.value;
    }
    public void changeVolume()
    {
        Camera.main.GetComponent<AudioSource>().volume = volumeSlider.value;
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
            ///Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;
            GameWin();
        }
        else
        {
            UIManager.Instance.setText("拾取物品");
            GameObject[] g;
            g = GameObject.FindGameObjectsWithTag("Item");
            for (int i = 0; i < g.Length; i++)
            {
                if (g[i].name == name)
                {
                    Destroy(g[i]);
                }
            }
            Transform iconParent = canvas.transform.Find("crayonIcons");
            Image icon = Instantiate(Resources.Load<Image>("ItemIcon"), iconParent);
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
            Image icon = Instantiate(Resources.Load<Image>("CrayonIcon"),iconParent);
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
                setText("白门通向初始房间的相同位置", 0.5f);
                break;
            case GameManager.DoorColor.BLACK:
                setText("黑门不可进入，但可以将白门出口的位置设为黑门所在位置", 0.5f);
                break;
            case GameManager.DoorColor.BLUE:
                setText("蓝门可以开启一个让人和某些物体缓慢传送的区域", 0.5f);
                break;
        }
    }//门的功能介绍
    private void handPosChange(RectTransform hand)
    {
        float interpolation = 5f * Time.deltaTime;
        if (Input.mousePosition.y - Screen.height / 2 < 0)
        {
            hand.localPosition = new Vector3(hand.localPosition.x, Mathf.Lerp(hand.localPosition.y, Input.mousePosition.y - Screen.height / 2, interpolation), hand.localPosition.z);
            //hand.localPosition = new Vector3(hand.localPosition.x, Input.mousePosition.y - Screen.height / 2, hand.localPosition.z);
        }
        else
        {
            hand.localPosition = new Vector3(hand.localPosition.x, Mathf.Lerp(hand.localPosition.y, 0, interpolation), hand.localPosition.z);
        }
    }
    
    private void GameWin()
    {
        clear.SetActive(true);
        clear.GetComponentInChildren<Text>().text = "第" + (SceneManager.GetActiveScene().buildIndex+1).ToString() + "关";
        Invoke("NextScene", 5);
    }
    private void ControlIntroduce()
    {
        if (isOn)
        {
            setting.SetActive(true);
            introduceText.SetActive(false);
            isOn = false;
            control.GetComponentInChildren<Text>().text = "操作说明";
        }
        else
        {
            setting.SetActive(false);
            introduceText.SetActive(true);
            isOn = true;
            control.GetComponentInChildren<Text>().text = "设置";
        }
    }
    private void Bright(float value)
    {
        Camera.main.GetComponent<Brightness>().brightness = value;
    }
    private void Saturation(float value)
    {
        Camera.main.GetComponent<Brightness>().saturation = value;
    }
    private void Contrast(float value)
    {
        Camera.main.GetComponent<Brightness>().contrast = value;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void quitGame()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#else

            Application.Quit();

#endif
    }

    public void BackMain()
    {
        SceneManager.LoadScene(0);
    }
}
