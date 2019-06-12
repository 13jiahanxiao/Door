using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Instance
    private static LevelManager _Instance;
    public static LevelManager Instance
    {
        get { return _Instance; }
    }
    #endregion
    void Awake()
    {
        _Instance = this;
    }
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
  public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void nextScene()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
