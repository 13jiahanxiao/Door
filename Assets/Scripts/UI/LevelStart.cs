using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    private void Start()
    {
        Invoke("Hide", 4);
    }

    private void Hide()
    {
        this.transform.gameObject.SetActive(false);
    }
}
