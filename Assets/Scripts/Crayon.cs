using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crayon
{
    public int count;
    public int number=1;
    public GameManager.DoorColor color;
    public Crayon(int i,GameManager.DoorColor color)
    {
        count = i;
        this.color = color;
        number = 1;
    }
}
