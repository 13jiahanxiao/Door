using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brush : MonoBehaviour
{
    public GameObject doorParent;//prefab
    public int num;
    public Brush(int n)
    {
        num = n;
    }
    public abstract void paint(Door door);
}
public class RedBrush:Brush
{ 
   public RedBrush(int n) : base(n) { }
    public override void paint(Door door)
    {
        //改变门对应材质贴图
        door.doorParent = new GameObject("doorParent");
        
        //将door对应的另一个房间中的门实例化并作为doorParent的子物体
        door.doorParent.SetActive(false);
    }
}
public class PurpleBrush : Brush
{
    public PurpleBrush(int n) : base(n) { }
    public override void paint(Door door)
    {
        //改变门对应材质贴图
        door.doorParent = new GameObject("doorParent");     //新建doorParent
       
        //将door对应的另一个房间中的门实例化并作为doorParent的子物体
        door.doorParent.SetActive(false);
    }
}
