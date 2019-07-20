using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public Button controlInterduce;
    public Button storyInterduce;
    public Button start;
    public GameObject startPanel;
    public GameObject introducePanel;
    public Text storyText;
    public Text controlText;

    private void Start()
    {
        start.onClick.AddListener(StartButton);
        storyInterduce.onClick.AddListener(StoryInterduce);
        controlInterduce.onClick.AddListener(ControlInterduce);
    }
    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void StoryInterduce()
    {
        startPanel.SetActive(false);
        introducePanel.SetActive(true);
    }
    public void ControlInterduce()
    {
        storyText.gameObject.SetActive(false);
        controlInterduce.gameObject.SetActive(false);
        controlText.gameObject.SetActive(true);
        start.gameObject.SetActive(true);
    }
}
