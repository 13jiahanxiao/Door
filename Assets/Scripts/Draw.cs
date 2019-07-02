using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    public float distance = 4.5f;

    public Vector3 screenCenter;

    public Transform doorParent;
    public GameObject door;
    public bool canDraw;
    // Vector3 newDoorPosition;
    void Start()
    {
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Input.GetMouseButtonUp(0))
        {
            UIManager.Instance.circle.fillAmount = 0;
            canDraw = false;
        }
        if (Physics.Raycast(ray, out hit, distance))
        {
            if (Input.GetMouseButtonDown(0))
            {
                switch (hit.transform.gameObject.tag)
                {
                    case "Item":
                        UIManager.Instance.setText("拾取物品");
                        UIManager.Instance.pickItem(hit.transform.name);
                        break;
                    case "Lock":
                        if (UIManager.Instance.itemIcons.Count > 0)
                        {
                            if (hit.transform.parent.eulerAngles.y > 45 || hit.transform.parent.eulerAngles.y < -45)
                                UIManager.Instance.setText("开锁");
                            hit.transform.parent.parent.GetComponent<ChestDoor>().chestDoorOpen();
                            UIManager.Instance.useKey();
                        }
                        else
                        {
                            UIManager.Instance.setText("缺少钥匙");
                        }
                        break;
                    case "DoorPosition":
                        if (GameManager.Instance.onMiddle)
                        {
                            UIManager.Instance.setText("不可在此处画门");
                        }
                        else if (GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].num <= 0)
                        {
                            UIManager.Instance.setText("蜡笔耗尽");
                        }
                        else if ((GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].color == GameManager.DoorColor.WHITE ||
                           GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].color == GameManager.DoorColor.BLACK) &&
                            GameManager.Instance.currentRoom.transform == GameManager.Instance.startRoom.transform)
                        {
                            UIManager.Instance.setText("初始房间中不能画白门或黑门");
                        }
                        else if (GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].color == GameManager.DoorColor.BLACK && !GameManager.Instance.whiteExist)
                        {
                            UIManager.Instance.setText("必须存在白门才能画黑门");
                        }
                        else
                        {
                            canDraw = true;
                            UIManager.Instance.circle.color = new Color(1, 1, 1, 1);
                        }
                        break;
                    default:
                        UIManager.Instance.setText("无效位置");
                        break;
                }
            }
            if (Input.GetMouseButton(0) && canDraw == true)
            {
                if (hit.transform.gameObject.tag == "DoorPosition")
                {
                    UIManager.Instance.circle.fillAmount += UIManager.Instance.fillspeed * Time.deltaTime;
                    if (GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].color == GameManager.DoorColor.RED)
                    {
                        UIManager.Instance.circle.color += new Color(0, -1f, -1f, 0) * UIManager.Instance.fillspeed * Time.deltaTime;
                    }
                    else if (GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].color == GameManager.DoorColor.BLACK)
                    {
                        UIManager.Instance.circle.color += new Color(-1f, -1f, -1f, 0) * UIManager.Instance.fillspeed * Time.deltaTime;
                    }
                    if (UIManager.Instance.circle.fillAmount > 0.999)
                    {
                        //UIManager.Instance.setText("生成门");
                        paint(hit);
                        UIManager.Instance.circle.fillAmount = 0;
                        canDraw = false;
                    }

                }
                else
                {
                    UIManager.Instance.circle.fillAmount = 0;
                    canDraw = false;
                }
            }
        }
        else
        {
            UIManager.Instance.circle.fillAmount = 0;
            canDraw = false;
        }
    }

    public void paint(RaycastHit hit)
    {
        if (GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].num > 0)
        {
            if (!GameManager.Instance.onMiddle)
            {
                GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].num--;
                UIManager.Instance.updateNum();
                CreateDoor(hit);
            }
        }
    }
    void CreateDoor(RaycastHit hit)
    {
        Material wallMaterial = hit.transform.gameObject.GetComponent<MeshRenderer>().materials[0];

        Material color = Resources.Load<Material>("DoorColor/" + GameManager.Instance.CrayonColorName());//门颜色

        GameObject go = GameManager.Instance.currentRoom.gameObject;
        int[] hide = new int[2] { hit.transform.parent.GetSiblingIndex(), hit.transform.GetSiblingIndex() };
        go.GetComponent<Room>().hideIndex.Add(hide);  //将消去方格的下标存于room中
        hit.transform.gameObject.SetActive(false);
        GameObject door = Instantiate<GameObject>(Resources.Load<GameObject>("Door"), hit.transform.position + hit.transform.right * GameManager.Instance.wallThickness / 2, hit.transform.rotation, go.transform);
        Transform[] child = door.GetComponentsInChildren<Transform>();
        child[2].gameObject.GetComponent<MeshRenderer>().materials[0].CopyPropertiesFromMaterial(wallMaterial);

        door.GetComponent<Door>().color = GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].color;
        door.transform.eulerAngles += new Vector3(0, 0, -90);
        door.GetComponent<Door>().toStartRoom = (GameManager.Instance.currentCrayon == (int)GameManager.DoorColor.WHITE);
        door.GetComponent<Renderer>().material = color;
        if (GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].color == GameManager.DoorColor.WHITE)
        {
            GameManager.Instance.whiteDoorCalculate(door.GetComponent<Door>(), color, hit);
            GameManager.Instance.whiteExist = true;
        }
        else if (GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].color == GameManager.DoorColor.BLACK)
        {
            door.GetComponent<Collider>().isTrigger = false;
            door.tag = "Untagged";
            GameManager.Instance.whiteDoorCalculate(door.GetComponent<Door>(), color, hit);
        }
        else
        {
            CreateOtherDoor(door, color, hit);
        }
    }
    void CreateOtherDoor(GameObject door, Material color, RaycastHit hit)
    {
        Material wallMaterial = hit.transform.gameObject.GetComponent<MeshRenderer>().materials[0];
        GameObject newroom = new GameObject("RoomManager");
        newroom.transform.position = new Vector3(0, 0, 0);
        newroom.AddComponent<Room>();
        newroom.GetComponent<Room>().hideIndex.Add(new int[2] { hit.transform.parent.GetSiblingIndex(), hit.transform.GetSiblingIndex() });
        newroom.GetComponent<Room>().house = (GameManager.houseNumber)(1 - (int)GameManager.Instance.currentRoom.house);//给新房间指定另一个house
        Vector3 newDoorPos = door.transform.position - door.transform.up * GameManager.Instance.wallThickness * 2;
        GameObject otherDoor = Instantiate<GameObject>(Resources.Load<GameObject>("Door"), newDoorPos, door.transform.rotation, newroom.transform);
        Transform[] child = otherDoor.GetComponentsInChildren<Transform>();
        child[2].gameObject.GetComponent<MeshRenderer>().materials[0].CopyPropertiesFromMaterial(wallMaterial);

        GameManager.Instance.ConnectDoor(door, otherDoor);//两门的door类中互相保存对方地址
        otherDoor.GetComponent<Door>().toStartRoom = (GameManager.Instance.currentRoom.house == GameManager.houseNumber.House0); //标记是否通向初始房
        otherDoor.GetComponent<Door>().color = GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].color;
        otherDoor.transform.Rotate(new Vector3(0, 0, 180), Space.Self);
        otherDoor.GetComponent<Renderer>().material = color;
        GameManager.Instance.targetHouseCalculate(GameManager.Instance.crayonList[GameManager.Instance.currentCrayon].color, newroom.GetComponent<Room>(), otherDoor.GetComponent<Door>());
        newroom.SetActive(false);//创建时隐藏
    }
}
