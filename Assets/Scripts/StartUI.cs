using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    public GameObject illustration;
    void Start()
    {
        illustration.SetActive(false);
    }
    public void startButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void illustrateButton()
    {
        illustration.SetActive(true);
    }
}
