using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    private void Start()
    {
        Invoke("Hide", 2);
    }

    private void Hide()
    {
        this.transform.gameObject.SetActive(false);
    }
}
