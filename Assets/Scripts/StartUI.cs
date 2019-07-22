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
    public Button select;
    public GameObject startPanel;
    public GameObject introducePanel;
    public Text storyText;
    public Text controlText;
    public GameObject LevelPanel;
    public Button returnButton;
    public GameObject[] levelButton=new GameObject[8];

    private void Start()
    {
        start.onClick.AddListener(StartButton);
        storyInterduce.onClick.AddListener(StoryInterduce);
        controlInterduce.onClick.AddListener(ControlInterduce);
        select.onClick.AddListener(SelectLevel);
        returnButton.onClick.AddListener(Return);
        for(int i = 0; i < 8; i++)
        {
            int level = int.Parse(levelButton[i].GetComponent<Text>().text);
            levelButton[i].GetComponent<Button>().onClick.AddListener(delegate() { LevelSelect(level); });
        }
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
        select.gameObject.SetActive(true);
    }
    public void SelectLevel()
    {
        controlText.gameObject.SetActive(false);
        start.gameObject.SetActive(false);
        LevelPanel.SetActive(true);
        select.gameObject.SetActive(false);
        LevelPanel.gameObject.SetActive(true);
    }
    public void Return()
    {
        controlText.gameObject.SetActive(true);
        start.gameObject.SetActive(true);
        LevelPanel.SetActive(false);
        select.gameObject.SetActive(true);
        LevelPanel.gameObject.SetActive(false);
    }
    public void LevelSelect(int i)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + i);
    }
 }

