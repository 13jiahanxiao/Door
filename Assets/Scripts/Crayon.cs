using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crayon
{
    public int num;
    public GameManager.DoorColor color;
    public Crayon(int i,GameManager.DoorColor color)
    {
        num = i;
        this.color = color;
    }
}
