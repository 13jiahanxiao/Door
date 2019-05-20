using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager:MonoBehaviour
{
    static DoorManager _Instance;
    private void Awake()
    {
        _Instance = this;
    }
    
    static public DoorManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    public List<GameObject> doorManagerList=new List<GameObject>();

    public void DoorAdd(GameObject door)
    {
        doorManagerList.Add(door);
    }

    public void DoorMove(GameObject door)
    {
        doorManagerList.Remove(door);
    }
}
